using System.Text;
using ClosedXML.Excel;
using Core.DTOs.Expense;
using Core.Interfaces.Repository;
using Core.Interfaces.Service;
using Core.Request;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using QRCoder;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Infrastructure.Services;

public class ExpenseService : IExpenseService
{
    private readonly IExpenseRepository _expenseRepository;

#nullable disable
    public ExpenseService() {}
#nullable enable

    public ExpenseService(IExpenseRepository expenseRepository)
    {
        _expenseRepository = expenseRepository;
    }

    #region Crud Region
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

    public async Task<ExpenseResponseDto> UpdateExpense(int id, UpdateExpenseDto updateExpenseDto, CancellationToken cancellationToken)
    {
        ValidateId(id);
        var updatedExpense = await _expenseRepository.UpdateExpense(id, updateExpenseDto, cancellationToken)
            ?? throw new NullReferenceException($"No se pudo actualizar el gasto con el id: {id}");

        return updatedExpense;
    }
    #endregion

    #region Documents Create Region
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
                        columns.RelativeColumn(2);
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

    public async Task<byte[]> GenerateExpenseExcelReport(int range, CancellationToken CancellationToken)
    {
        ValidateId(range);

        var products = await _expenseRepository.GetProductRange(range, CancellationToken)
            ?? throw new ArgumentNullException($"No se encontraron gastos en el rango de {range}");

        using var workbook = new XLWorkbook();

        var worksheet = workbook.AddWorksheet("REPORTE DE GASTOS");

        worksheet.Cell(1, 1).Value = "Id";
        worksheet.Cell(1, 2).Value = "User";
        worksheet.Cell(1, 3).Value = "Category";
        worksheet.Cell(1, 4).Value = "Amount";

        int row = 2;
        foreach (var product in products)
        {
            worksheet.Cell(row, 1).Value = product.UserId.ToString();
            worksheet.Cell(row, 2).Value = product.UserName;
            worksheet.Cell(row, 3).Value = product.Category ?? "Sin categoría";
            worksheet.Cell(row, 4).Value = product.Amount.ToString();
            row++;
        }

        worksheet.Columns().AdjustToContents();

        using var memoryStream = new MemoryStream();
        workbook.SaveAs(memoryStream);
        return memoryStream.ToArray();
    }

    public async Task<byte[]> GenerateExpenseChart(CancellationToken cancellationToken)
    {
        var expenseCategoriesTotalDto = await _expenseRepository.ChartExpenseData(cancellationToken)
            ?? throw new ArgumentNullException("No se encontraron datos para generar el grafico");

        var plotModel = new PlotModel { Title = "Gastos por categoría" };

        var barSeries = new BarSeries
        {
            Title = "Gastos",
            StrokeColor = OxyColors.Black,
            StrokeThickness = 1
        };

        foreach (var product in expenseCategoriesTotalDto)
        {
            barSeries.Items.Add(new BarItem { Value = (double)product.TotalAmount, Color = OxyColor.Parse("#FF5733") });
        }

        plotModel.Series.Add(barSeries);

        var categoryAxis = new CategoryAxis
        {
            Position = AxisPosition.Bottom,
            Title = "Categoría",
        };

        foreach (var expense in expenseCategoriesTotalDto)
        {
            categoryAxis.Labels.Add(expense.Category);
        }

        plotModel.Axes.Add(categoryAxis);

        var pngExporter = new PdfExporter { Width = 1280, Height = 720 };
        using var stream = new MemoryStream();
        pngExporter.Export(plotModel, stream);
        return stream.ToArray();
    }
    #endregion

    #region Private Methods
    private static void ValidateId(int id)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(id);
    }
    #endregion
}