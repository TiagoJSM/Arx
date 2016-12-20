using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class LanguageGroup
{
    public string Language;
    public LocalizedText Localizations;
}

[Serializable]
public class Translation
{
    public string Key;
    public string Translated;
}

[Serializable]
public class LocalizedText
{
    [SerializeField]
    public Translation[] translation = new Translation[0];

    public string this[string textKey]
    {
        get
        { 
            if(translation == null)
            {
                return null;
            }

            for(var idx = 0; idx < translation.Length; idx++)
            {
                if(translation[idx].Key == textKey)
                {
                    return translation[idx].Translated;
                }
            }

            return null;
        }
    }

    public string this[int idx]
    {
        get
        {
            if (translation == null)
            {
                return null;
            }

            return translation[idx].Translated;
        }
    }
}

[CreateAssetMenu(fileName = "LocalizedTexts", menuName = "Localized Texts", order = 1)]
public class LocalizedTexts : ScriptableObject
{
    [SerializeField]
    public LanguageGroup[] localizations = new LanguageGroup[0];
    public string sheetUrl;

    public string this[string textKey]
    {
        get
        {
            var currentLang = LocalizationConfig.CurrentLanguage;
            var text = GetLocalizedText(currentLang, textKey);
            if (text == null && LocalizationConfig.CurrentLanguage != LocalizationConfig.DefaultLanguage)
            {
                text = GetLocalizedText(LocalizationConfig.DefaultLanguage, textKey);
            }
            if(text != null)
            {
                return text;
            }
            Debug.Log("Translation not found for " + textKey);
            return null;
        }
    }

    public string this[int idx]
    {
        get
        {
            var currentLang = LocalizationConfig.CurrentLanguage;
            var text = GetLocalizedText(currentLang, idx);
            if (text == null && LocalizationConfig.CurrentLanguage != LocalizationConfig.DefaultLanguage)
            {
                text = GetLocalizedText(LocalizationConfig.DefaultLanguage, idx);
            }
            if (text != null)
            {
                return text;
            }
            Debug.Log("Translation not found for conversation index " + idx);
            return null;
        }
    }

    private string GetLocalizedText(string language, string textKey)
    {
        for(var idx = 0; idx < localizations.Length; idx++)
        {
            if(localizations[idx].Language == language)
            {
                return localizations[idx].Localizations[textKey];
            }
        }

        return null;
    }

    private string GetLocalizedText(string language, int dialogIdx)
    {
        for (var idx = 0; idx < localizations.Length; idx++)
        {
            if (localizations[idx].Language == language)
            {
                return localizations[idx].Localizations[dialogIdx];
            }
        }

        return null;
    }

}
