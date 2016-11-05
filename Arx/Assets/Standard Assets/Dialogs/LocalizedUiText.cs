using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum Language
{
    EN, PT, FR
}

[RequireComponent(typeof(Text))]
public class LocalizedUiText : MonoBehaviour {

    private Text _text;

    [SerializeField]
    private LocalizedTexts _localization;
    [SerializeField]
    private string _key;
    [SerializeField]
    private Language _language;

    // Update is called once per frame
    void Start () {
        LocalizationConfig.DefaultLanguage = Language.EN.ToString();
        LocalizationConfig.CurrentLanguage = _language.ToString();
        _text = GetComponent<Text>();
        _text.text = _localization[_key];
	}
}
