using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IDialogEditorContext
{
    string Code { get; set; }
    string ClientId { get; set; }
    string ClientSecret { get; set; }
    string Token { get; set; }
    DialogViewState ViewState { get; set; }
    ExcelFile ExcelFile { get; set; }
    DialogFileList FileList { get; set; }
}
