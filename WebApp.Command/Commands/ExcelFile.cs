using System.Data;
using ClosedXML.Excel;

namespace WebApp.Command.Commands;

public class ExcelFile<T>
{
    public readonly List<T> _list;
    public string FileName => $"{typeof(T).Name}.xlsx";
    public string FileType => $"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    
    public ExcelFile(List<T> list)
    {
        _list = list;
    }

    public MemoryStream Create()
    {
        var wb = new XLWorkbook();
        var ds = new DataSet();
        ds.Tables.Add(GetTable());
        wb.Worksheets.Add(ds);

        var excelMemory = new MemoryStream();
        wb.SaveAs(excelMemory);

        return excelMemory;
    }

    private DataTable GetTable()
    {
        var table = new DataTable();
        var type = typeof(T);
        foreach (var propertyInfo in type.GetProperties().ToList())
        {
            table.Columns.Add(propertyInfo.Name, propertyInfo.PropertyType);
        }

        foreach (var listItem in _list)
        {
            var values = type.GetProperties().Select(propertyInfo => propertyInfo.GetValue(listItem, null)).ToArray();
            table.Rows.Add(values);
        }

        return table;
    }
}