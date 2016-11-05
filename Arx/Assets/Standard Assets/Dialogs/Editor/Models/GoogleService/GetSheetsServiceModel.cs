using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public class SheetPropertiesServiceModel
{
    public string title;
}

[Serializable]
public class SheetServiceModel
{
    public SheetPropertiesServiceModel properties;
}

[Serializable]
public class GetSheetsServiceModel
{
    public SheetServiceModel[] sheets;
}

