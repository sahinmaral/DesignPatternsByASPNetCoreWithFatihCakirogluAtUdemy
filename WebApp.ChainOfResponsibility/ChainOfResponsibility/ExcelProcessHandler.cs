using System.Data;
using ClosedXML.Excel;

namespace WebApp.ChainOfResponsibility.ChainOfResponsibility;

public class ExcelProcessHandler<T> : ProcessHandler
{
    private DataTable GetTable(object o)
    {
        var table = new DataTable();
        var type = typeof(T);
        
        foreach (var propertyInfo in type.GetProperties().ToList())
        {
            table.Columns.Add(propertyInfo.Name, propertyInfo.PropertyType);
        }

        var list = o as List<T>;
        foreach (var listItem in list)
        {
            var values = type.GetProperties().Select(info => info.GetValue(listItem, null)).ToArray();
            table.Rows.Add(values);
        }

        return table;
    }
    public override object Handle(object o)
    {
        var wb = new XLWorkbook();
        var ds = new DataSet();
        ds.Tables.Add(GetTable(o));
        wb.Worksheets.Add(ds);

        var excelMemoryStream = new MemoryStream();
        wb.SaveAs(excelMemoryStream);
        
        return base.Handle(excelMemoryStream);
    }
}