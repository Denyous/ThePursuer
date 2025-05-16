using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using TMPro;

public class LanguageSwitcher : MonoBehaviour
{
    public List<TextMeshProUGUI> textFields;

    [SerializeField] private TMP_FontAsset ArabicFont;
    [SerializeField] private TMP_FontAsset ChineseFont;
    [SerializeField] private TMP_FontAsset JapaneseFont;
    [SerializeField] private TMP_FontAsset HindiFont;
    [SerializeField] private TMP_FontAsset DefaultFont;

    private void Start()
    {
        string languageCode = PlayerPrefs.GetString("Language");
        SetLanguage(languageCode);
    }
    public void SetLanguage(string languageCode)
    {
        StartCoroutine(SetLocale(languageCode));
    }

    private System.Collections.IEnumerator SetLocale(string languageCode)
    {
        yield return LocalizationSettings.InitializationOperation;

        var locales = LocalizationSettings.AvailableLocales.Locales;

        foreach (var locale in locales)
        {
            if (locale.Identifier.Code == languageCode)
            {
                LocalizationSettings.SelectedLocale = locale;
                SetFont(languageCode);
                yield break;
            }
        }
    }

    private void SetFont(string LanguageCode)
    {
        switch (LanguageCode)
        {
            case "ar":
                SetFontForUIElements(ArabicFont);
                break;
            case "zh":
                SetFontForUIElements(ChineseFont);
                break;
            case "ja":
                SetFontForUIElements(JapaneseFont);
                break;
            case "hi":
                SetFontForUIElements(HindiFont);
                break;
            default:
                SetFontForUIElements(DefaultFont);
                break;
        }
    }

    private void SetFontForUIElements(TMP_FontAsset font)
    {
        foreach (var text in textFields)
        {
            text.font = font;
        }
    }
}
