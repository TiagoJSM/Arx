using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Standard_Assets.Dialogs.Editor.Utils
{
    public static class LocalizedTextsExtensions
    {
        public static void PopulateWith(this LocalizedTexts localized, ExcelSheet sheet)
        {
            localized.localizations = sheet.Headers.Select((header, languageIdx) =>
            {
                var keys = sheet.Values.Select(value => value[0]).ToArray();

                return new LanguageGroup()
                {
                    Language = header,
                    Localizations = new LocalizedText()
                    {
                        translation = keys.Select((key, rowIdx) =>
                        {
                            return new Translation()
                            {
                                Key = key,
                                Translated = sheet.GetRow(rowIdx)[languageIdx]
                            };
                        })
                        .ToArray()
                    }
                };
            })
            .ToArray();
        }
    }
}
