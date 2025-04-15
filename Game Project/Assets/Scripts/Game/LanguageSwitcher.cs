using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LanguageSwitcher : MonoBehaviour
{
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
                yield break;
            }
        }
    }
}
