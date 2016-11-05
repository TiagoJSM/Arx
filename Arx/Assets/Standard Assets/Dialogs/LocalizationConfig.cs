using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class LocalizationConfig
{
    static LocalizationConfig()
    {
        DefaultLanguage = "EN-GB";
    }

    public static string DefaultLanguage { get; set; }
    public static string CurrentLanguage { get; set; }
}
