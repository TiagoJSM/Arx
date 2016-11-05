using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public class ListFilesItemsServiceModel
{
    public string id;
    public string title;
    public string mimeType;
}

[Serializable]
public class ListFilesServiceModel
{
    public ListFilesItemsServiceModel[] items;
}

