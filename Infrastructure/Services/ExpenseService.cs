using System.Text;
using Core.DTOs.CategoryExpense;
using Core.DTOs.Expense;
using Core.Interfaces.Repository;
using Core.Interfaces.Service;
using Core.Request;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Wpf;
using QRCoder;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Infrastructure.Services;

public class ExpenseService : IExpenseService
{
    private readonly IExpenseRepository _expenseRepository;

    public ExpenseService(IExpenseRepository expenseRepository)
    {
        _expenseRepository = expenseRepository;
    }

    public async Task<ExpenseResponseDto> CreateExpense(CreateExpenseRequest createExpenseRequest, CancellationToken cancellationToken)
    {
        var createdExpense = await _expenseRepository.CreateExpense(createExpenseRequest, cancellationToken)
            ?? throw new NullReferenceException("No se pudo crear el gasto");

        return createdExpense;
    }

    public async Task<ExpenseResponseDto> DeleteExpense(int id, CancellationToken cancellationToken)
    {
        ValidateId(id);
        var deletedExpense = await _expenseRepository.DeleteExpense(id, cancellationToken)
            ?? throw new NullReferenceException("No se pudo eliminar el gasto");

        return deletedExpense;
    }

    public async Task<List<ExpenseResponseDto>> GetAllExpense(PaginationRequest paginationRequest, CancellationToken cancellationToken)
    {
        ValidateId(paginationRequest.ExpenseCategoryId);
        if (paginationRequest.PageSize <= 0 && paginationRequest.Page <= 0) 
            throw new ArgumentOutOfRangeException("El tamaño de la pagina y el numero de paginas no pueden ser valores negativos");

        var expenseList = await _expenseRepository.GetAllExpense(paginationRequest, cancellationToken)
            ?? throw new NullReferenceException("No se pudo obtener la lista de los gastos");

        return expenseList;
    }

    public async Task<ExpenseResponseDto> SearchExpense(int id, CancellationToken cancellationToken)
    {
        ValidateId(id);
        var searchedExpense = await _expenseRepository.SearchExpense(id, cancellationToken)
            ?? throw new ArgumentNullException($"No se encontro el gasto con el id: {id}");

        return searchedExpense;
    }

    public async Task<byte[]> GenerateExpenseQr(int id, CancellationToken cancellationToken)
    {
        ValidateId(id);
        var generateExpenseQr = await _expenseRepository.SearchExpense(id, cancellationToken)
            ?? throw new ArgumentNullException($"No se encontro el gasto con el id: {id}");

        var expenseDataFormatted = new StringBuilder();
        expenseDataFormatted.AppendLine($"Nombre: {generateExpenseQr.UserName}");
        expenseDataFormatted.AppendLine($"Monto: {generateExpenseQr.Amount:F2}");
        expenseDataFormatted.AppendLine($"Fecha: {generateExpenseQr.Date}");
        expenseDataFormatted.AppendLine($"Descripción: {generateExpenseQr.Description}");

        var data = expenseDataFormatted.ToString();

        // var expenseData = JsonSerializer.Serialize(expenseDataFormatted);

        using (QRCodeGenerator qrCodeGenerator = new())
        using (QRCodeData qrCodeData = qrCodeGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q))
        using (PngByteQRCode qrCode = new(qrCodeData))
        {
            byte[] qrCodeImage = qrCode.GetGraphic(5);

            return qrCodeImage;
        }
    }

    public async Task<ExpenseResponseDto> UpdateExpense(int id, UpdateExpenseDto updateExpenseDto, CancellationToken cancellationToken)
    {
        ValidateId(id);
        var updatedExpense = await _expenseRepository.UpdateExpense(id, updateExpenseDto, cancellationToken)
            ?? throw new NullReferenceException($"No se pudo actualizar el gasto con el id: {id}");

        return updatedExpense;
    }

    public async Task<byte[]> GenerateExpensePdfReport(int range, CancellationToken cancellationToken)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        ValidateId(range);

        var productsRange = await _expenseRepository.GetProductRange(range, cancellationToken)
            ?? throw new ArgumentNullException($"No se encontraron gastos en el rango de {range}");

        var document = Document.Create(c =>
        {
            c.Page(page =>
            {
                page.Margin(30);
                page.Size(PageSizes.A4);
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Header().Text("REPORTE DE GASTOS").Bold().FontSize(22).AlignCenter();
                page.Content().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(1);
                        //columns.RelativeColumn(3);
                        columns.RelativeColumn(2);
                        //columns.RelativeColumn(3);
                        columns.RelativeColumn(1);
                        columns.RelativeColumn(2);
                    });

                    table.Header(header =>
                    {
                        header.Cell().Text("Id").Bold();
                        header.Cell().Text("User").Bold();
                        header.Cell().Text("Category").Bold();
                        header.Cell().Text("Amount").Bold();
                    });

                    foreach (var product in productsRange)
                    {
                        table.Cell().Text(product.UserId.ToString());
                        table.Cell().Text(product.UserName);
                        table.Cell().Text(product.Category ?? "Sin categoría");
                        table.Cell().Text(product.Amount.ToString());
                    }
                });

                page.Footer().AlignCenter().Text(page => { page.CurrentPageNumber().Italic(); });
                // page.Footer().Text($"Reporte generado el {DateTime.UtcNow:dd/MM/yyyy}").Italic().AlignCenter();
            });
        });

        using MemoryStream stream = new();
        document.GeneratePdf(stream);
        return stream.ToArray();
    }

    private static void ValidateId(int id)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(id);
    }

    public async Task<byte[]> GenerateExpenseChartImg(List<ExpenseCategoryTotalDto> expenseCategoryTotalDtos, CancellationToken cancellationToken)
    {
        var plotModel = new PlotModel { Title = "Totales por categoría" };

        var barSeries = new BarSeries
        {
            ItemsSource = expenseCategoryTotalDtos.Select(e => new BarItem { Value = (double)e.Amount }).ToList(),
            LabelPlacement = LabelPlacement.Outside,
            LabelFormatString = "{0:..00}"
        };

        plotModel.Series.Add(barSeries);

        plotModel.Axes.Add(new CategoryAxis
        {
            Position = AxisPosition.Bottom,
            Key = "Categorias",
            ItemsSource = expenseCategoryTotalDtos.Select(x => x.Category).ToList()
        });

        using (var memoryStream = new MemoryStream())
        {
            var exporter = new PngExporter { Width = 600, Height = 400 };
            exporter.Export(plotModel, memoryStream);
            return memoryStream.ToArray();
        }
    }

    public async Task<byte[]> GenerateExpenseChartImg(int userId, CancellationToken cancellationToken)
    {
        var expenseCategoriesTotalDto = await _expenseRepository.GetExpenseTotalsByCategory(userId, cancellationToken)
            ?? throw new ArgumentNullException($"No se encontró ningun gasto para el usuario con el id: {userId}");

        var plotModel = new PlotModel { Title = "Totales por categoría" };

        var barSeries = new BarSeries
        {
            ItemsSource = expenseCategoriesTotalDto.Select(e => new BarItem { Value = (double)e.Amount }).ToList(),
            LabelPlacement = LabelPlacement.Outside,
            LabelFormatString = "{0:F2}"
        };

        plotModel.Series.Add(barSeries);

        plotModel.Axes.Add(new CategoryAxis
        {
            Position = AxisPosition.Bottom,
            Key = "Categorias",
            ItemsSource = expenseCategoriesTotalDto.Select(x => x.Category).ToList()
        });

        // Generar la imagen del gráfico en memoria
        using (var memoryStream = new MemoryStream())
        {
            var exporter = new PngExporter { Width = 600, Height = 400 };
            exporter.Export(plotModel, memoryStream);
            return memoryStream.ToArray();
        }
    }
}
