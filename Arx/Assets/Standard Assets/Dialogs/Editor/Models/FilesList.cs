using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class DialogFile
{
    public string Id { get; set; }
    public string Name { get; set; }
    public Texture Image { get; set; }
}

public class DialogFileList
{
    public DialogFile[] Files { get; set; }
}
