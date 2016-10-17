using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ExcelSheet
{
    private string[][] _data;

    public string Name { get; set; }

    public string[][] Data
    {
        get
        {
            return _data;
        }
        set
        {
            _data = value;
            Values = _data.Skip(1).ToArray();
        }
    }

    public string[][] Values { get; private set; }

    public int RowCount
    {
        get
        {
            return Values.Length;
        }
    }

    public int ColumnCount
    {
        get
        {
            return Data[0].Length;
        }
    }

    public string[] GetRow(int idx)
    {
        return Values[idx];
    }

    public string[] Headers
    {
        get
        {
            return Data[0];
        }
    }
}

public class ExcelFile
{
    public ExcelSheet[] Sheets { get; set; }
}
