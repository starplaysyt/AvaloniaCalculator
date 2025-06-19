using System;
using System.Data;
using System.Globalization;

namespace AvaloniaCalculator.Models;

public class Calculator
{
    //Eminent datatable instance to reduce memory allocations
    private DataTable _dataTable = new DataTable();
    
    public string Evaluate(string expression)
    {
        Console.WriteLine($"CASTED EXPR: {expression}");
        string str;
        try
        {
            str = Convert.ToSingle(_dataTable.Compute(expression, "")).ToString(CultureInfo.CurrentCulture)
                .Replace(',', '.');
        }
        catch (Exception e) when (e is OverflowException or DivideByZeroException or FormatException)
        {
            return "Error";
        }

        Console.WriteLine($"Result: {str}");
        Console.WriteLine("==========================");
        return str;
    }
}