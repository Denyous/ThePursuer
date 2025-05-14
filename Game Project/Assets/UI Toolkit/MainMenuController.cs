using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour
{
    public VisualElement MainMenu;
    public VisualElement TutorialPanel;
    public VisualElement MainMenuPanel;
    public VisualElement LanguagePanel;
    public VisualElement SoundPanel;
    public VisualElement CreditsPanel;

    public VisualElement LanguageArrow;
    public VisualElement SoundArrow;
    private bool isLanguageRotated = false;
    private bool isSoundRotated = false;

    public Button PlayButton;
    public Button TutorialButton;
    public Button CreditsButton;
    public Button QuitButton;
    public Button LanguageButton;
    public Button SoundButton;
    public Button TutorialPrevButton;
    public Button TutorialNextButton;
    public Button TutorialBackButton;
    public Button CreditsBackButton;

    public Button EnglishButton;
    public Button TurkishButton;
    public Button GermanButton;
    public Button FrenchButton;
    public Button ChineseButton;
    public Button JapaneseButton;
    public Button ArabicButton;
    public Button PolishButton;
    public Button RussianButton;
    public Button ItalianButton;
    public Button SpanishButton;

    public Slider MusicSlider;
    public Slider SfxSlider;
    public new AudioManager audio;

    public Sprite[] TutorialImages;
    public VisualElement TutorialImageContainer;
    public Label TutorialNumber;
    public int TutorialCount = 1;
    public int TutorialIndex = 0;

    private void Awake()
    {
       GetReferencesForPanels();
       LocalizationSettings.InitializationOperation.WaitForCompletion();
    }
    //These should be in the start because audio mixer should be initialized
    private void Start()
    {
        SetSound();
    }
    private void OnEnable()
    {
        GetReferencesForUIElements();
        RegisterCallBacks();
    }
    private void OnDisable()
    {
        UnRegisterCallBacks();
    }



    #region Events

    //Language Buttons
    private void OnSpanishButtonClicked()
    {
        SetLanguage("es");
        SetLocalizedTextForMainMenu();
    }
    private void OnItalianButtonClicked()
    {
        SetLanguage("it");
        SetLocalizedTextForMainMenu();
    }
    private void OnRussianButtonClicked()
    {
        SetLanguage("ru");
        SetLocalizedTextForMainMenu();
    }
    private void OnPolishButtonClicked()
    {
        SetLanguage("pl");
        SetLocalizedTextForMainMenu();
    }
    private void OnArabicButtonClicked()
    {
        //SetLanguage("yok");
        //SetLocalizedTextForMainMenu();
    }
    private void OnJapaneseButtonClicked()
    {
        SetLanguage("ja");
        SetLocalizedTextForMainMenu();
    }
    private void OnChineseButtonClicked()
    {
        //SetLanguage("yok");
        //SetLocalizedTextForMainMenu();
    }
    private void OnFrenchButtonClicked()
    {
        SetLanguage("fr");
        SetLocalizedTextForMainMenu();
    }
    private void OnGermanButtonClicked()
    {
        SetLanguage("de");
        SetLocalizedTextForMainMenu();
    }
    private void OnTurkishButtonClicked()
    {
        SetLanguage("tr");
        SetLocalizedTextForMainMenu();
    }
    private void OnEnglishButtonClicked()
    {
        SetLanguage("en");
        SetLocalizedTextForMainMenu();
    }


    private void OnSoundButtonClicked()
    {
        isSoundRotated = !isSoundRotated;
        RotateArrow(SoundArrow, isSoundRotated);
        TogglePanel(SoundPanel);
    }
    private void OnLanguageButtonClicked()
    {
        isLanguageRotated = !isLanguageRotated;
        RotateArrow(LanguageArrow, isLanguageRotated);
        TogglePanel(LanguagePanel);
    }
    private void OnCreditsBackButtonClicked()
    {
        TogglePanels(MainMenuPanel, CreditsPanel);
    }
    private void OnTutorialBackButtonClicked()
    {
        TogglePanels(MainMenuPanel, TutorialPanel);
    }
    private void OnTutorialNextButtonClicked()
    {
        TutorialCount++;
        TutorialIndex++;

        if (TutorialIndex >= TutorialImages.Length - 1)
        {
            TutorialNextButton.SetEnabled(false);
        }
        else if (TutorialIndex > 0 && TutorialIndex < TutorialImages.Length - 1)
        {
            TutorialPrevButton.SetEnabled(true);
        }

        TutorialImageContainer.style.backgroundImage = new StyleBackground(TutorialImages[TutorialIndex]);
        TutorialNumber.text = TutorialCount + " / " + TutorialImages.Length;
    }
    private void OnTutorialPrevButtonClicked()
    {
        TutorialCount--;
        TutorialIndex--;

        if (TutorialIndex < 1)
        {
            TutorialPrevButton.SetEnabled(false);
        }
        else if (TutorialIndex > 0 && TutorialIndex < TutorialImages.Length - 1)
        {
            TutorialNextButton.SetEnabled(true);
        }

        TutorialImageContainer.style.backgroundImage = new StyleBackground(TutorialImages[TutorialIndex]);
        TutorialNumber.text = TutorialCount + " / " + TutorialImages.Length;
    }
    private void OnPlayButtonClicked()
    {
        SceneManager.LoadScene("Game");
    }
    private void OnTutorialButtonClicked()
    {
        TutorialCount = 1;
        TutorialIndex = 0;
        TutorialPrevButton.SetEnabled(false);
        TutorialNextButton.SetEnabled(true);
        TogglePanels(TutorialPanel, MainMenuPanel);
        TutorialImageContainer.style.backgroundImage = new StyleBackground(TutorialImages[TutorialIndex]);
        TutorialNumber.text = TutorialCount + " / " + TutorialImages.Length;
    }
    private void OnCreditsButtonClicked()
    {
        TogglePanels(CreditsPanel, MainMenuPanel);
    }
    private void OnQuitButtonClicked()
    {
        Application.Quit();

#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
    }
    #endregion
    #region HelperFunctions
    private void SetLocalizedTextForMainMenu()
    {
        SetLocalizedText(PlayButton, "MainMenuStart");
        SetLocalizedText(QuitButton, "MainMenuQuit");
        SetLocalizedText(CreditsButton, "MainMenuCredits");
    }
    private void SetLocalizedText(VisualElement element, string entryKey)
    {
        var localizedString = new LocalizedString
        {
            TableReference = "MainMenu",
            TableEntryReference = entryKey
        };

        // Register callback so it updates immediately and on language switch
        localizedString.StringChanged += (localizedValue) =>
        {
            if (element is Label label) label.text = localizedValue;
            else if (element is Button button) button.text = localizedValue;
        };
    }
    private void SetSound()
    {
        if (PlayerPrefs.HasKey("Music") && PlayerPrefs.HasKey("SFX"))
        {
            MusicSlider.value = PlayerPrefs.GetFloat("Music");
            SfxSlider.value = PlayerPrefs.GetFloat("SFX");
        }

        audio.SetMusicVolume(MusicSlider.value);
        audio.SetSFXVolume(SfxSlider.value);
    }
    private void TogglePanels(VisualElement showPanel, VisualElement hidePanel)
    {
        showPanel.style.display = DisplayStyle.Flex;
        hidePanel.style.display = DisplayStyle.None;
    }
    private void TogglePanel(VisualElement panel)
    {
        bool isVisible = panel.style.display == DisplayStyle.Flex;

        if (isVisible)
        {
            panel.style.display = DisplayStyle.None;
        }
        else
        {
            panel.style.display = DisplayStyle.Flex;
        }
    }
    private void RotateArrow(VisualElement arrow, bool rotate)
    {
        arrow.style.rotate = new Rotate(new Angle(rotate ? -90 : 0));
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
                yield break;
            }
        }
    }
    #endregion
    #region Event Registers and UnRegisters and References
    private void GetReferencesForPanels()
    {
        MainMenu = GetComponent<UIDocument>().rootVisualElement;
        MainMenuPanel = MainMenu.Q<VisualElement>("MainMenuPanel");
        MainMenu.Insert(0, MainMenuPanel);
        TutorialPanel = MainMenu.Q<VisualElement>("TutorialPanel");
        LanguagePanel = MainMenu.Q<VisualElement>("LanguageMenu");
        SoundPanel = MainMenu.Q<VisualElement>("SoundMenu");
        CreditsPanel = MainMenu.Q<VisualElement>("CreditsPanel");

        LanguageArrow = MainMenu.Q<VisualElement>("LanguageSelectorImage");
        SoundArrow = MainMenu.Q<VisualElement>("SoundSelectorImage");

        Application.targetFrameRate = 60;
        int highscore = PlayerPrefs.GetInt("HighScore");
        //highScoreText.text = "HIGH SCORE: " + highscore.ToString() + " POINTS";

        // Get the reference for all the sliders
        MusicSlider = MainMenu.Q<Slider>("MusicSlider");
        SfxSlider = MainMenu.Q<Slider>("SfxSlider");
    }
    private void GetReferencesForUIElements()
    {
        // Get the reference for all the buttons
        PlayButton = MainMenu.Q<Button>("StartButton");
        TutorialButton = MainMenu.Q<Button>("TutorialButton");
        CreditsButton = MainMenu.Q<Button>("CreditsButton");
        QuitButton = MainMenu.Q<Button>("QuitButton");
        LanguageButton = MainMenu.Q<Button>("LanguageSelectorButton");
        SoundButton = MainMenu.Q<Button>("SoundSelectorButton");
        TutorialPrevButton = MainMenu.Q<Button>("PrevButton");
        TutorialNextButton = MainMenu.Q<Button>("NextButton");
        TutorialBackButton = MainMenu.Q<Button>("BackButton");
        CreditsBackButton = MainMenu.Q<Button>("CreditsBackButton");

        // Get the reference for all the language buttons
        EnglishButton = MainMenu.Q<Button>("EnglishButton");
        TurkishButton = MainMenu.Q<Button>("TurkishButton");
        GermanButton = MainMenu.Q<Button>("GermanButton");
        FrenchButton = MainMenu.Q<Button>("FrenchButton");
        ChineseButton = MainMenu.Q<Button>("ChineseButton");
        JapaneseButton = MainMenu.Q<Button>("JapaneseButton");
        ArabicButton = MainMenu.Q<Button>("ArabicButton");
        PolishButton = MainMenu.Q<Button>("PolishButton");
        RussianButton = MainMenu.Q<Button>("RussianButton");
        ItalianButton = MainMenu.Q<Button>("ItalianButton");
        SpanishButton = MainMenu.Q<Button>("SpanishButton");



        //Get the reference for the tutorial image container and number
        TutorialImageContainer = MainMenu.Q<VisualElement>("TutorialImageContainer");
        TutorialNumber = MainMenu.Q<Label>("TutorialNumber");
    }
    private void RegisterCallBacks()
    {
        // Events for Sliders
        MusicSlider.RegisterValueChangedCallback(e =>
        {
            float volume = e.newValue;
            audio.SetMusicVolume(volume);
        });

        SfxSlider.RegisterValueChangedCallback(e =>
        {
            float volume = e.newValue;
            audio.SetSFXVolume(volume);
        });

        MainMenuPanel.RegisterCallback<PointerDownEvent>(evt =>
        {
            if (LanguagePanel.style.display == DisplayStyle.Flex)
            {
                LanguagePanel.style.display = DisplayStyle.None;
                isLanguageRotated = !isLanguageRotated;
                RotateArrow(LanguageArrow, isLanguageRotated);
            }

            if (SoundPanel.style.display == DisplayStyle.Flex)
            {
                SoundPanel.style.display = DisplayStyle.None;
                isSoundRotated = !isSoundRotated;
                RotateArrow(SoundArrow, isSoundRotated);
            }
        });


        // Events for Main Menu Buttons
        PlayButton.RegisterCallback<ClickEvent>(e => OnPlayButtonClicked());
        TutorialButton.RegisterCallback<ClickEvent>(e => OnTutorialButtonClicked());
        CreditsButton.RegisterCallback<ClickEvent>(e => OnCreditsButtonClicked());
        QuitButton.RegisterCallback<ClickEvent>(e => OnQuitButtonClicked());
        LanguageButton.RegisterCallback<ClickEvent>(e => OnLanguageButtonClicked());
        SoundButton.RegisterCallback<ClickEvent>(e => OnSoundButtonClicked());

        // Events for Language Buttons
        EnglishButton.RegisterCallback<ClickEvent>(e => OnEnglishButtonClicked());
        TurkishButton.RegisterCallback<ClickEvent>(e => OnTurkishButtonClicked());
        GermanButton.RegisterCallback<ClickEvent>(e => OnGermanButtonClicked());
        FrenchButton.RegisterCallback<ClickEvent>(e => OnFrenchButtonClicked());
        ChineseButton.RegisterCallback<ClickEvent>(e => OnChineseButtonClicked());
        JapaneseButton.RegisterCallback<ClickEvent>(e => OnJapaneseButtonClicked());
        ArabicButton.RegisterCallback<ClickEvent>(e => OnArabicButtonClicked());
        PolishButton.RegisterCallback<ClickEvent>(e => OnPolishButtonClicked());
        RussianButton.RegisterCallback<ClickEvent>(e => OnRussianButtonClicked());
        ItalianButton.RegisterCallback<ClickEvent>(e => OnItalianButtonClicked());
        SpanishButton.RegisterCallback<ClickEvent>(e => OnSpanishButtonClicked());

        // Events for Tutorial Buttons
        TutorialPrevButton.RegisterCallback<ClickEvent>(e => OnTutorialPrevButtonClicked());
        TutorialNextButton.RegisterCallback<ClickEvent>(e => OnTutorialNextButtonClicked());
        TutorialBackButton.RegisterCallback<ClickEvent>(e => OnTutorialBackButtonClicked());

        // Events for Credits Buttons
        CreditsBackButton.RegisterCallback<ClickEvent>(e => OnCreditsBackButtonClicked());
    }
    private void UnRegisterCallBacks()
    {
        MusicSlider.UnregisterValueChangedCallback(e =>
        {
            float volume = e.newValue;
            audio.SetMusicVolume(volume);
        });

        SfxSlider.UnregisterValueChangedCallback(e =>
        {
            float volume = e.newValue;
            audio.SetSFXVolume(volume);
        });

        MainMenuPanel.UnregisterCallback<PointerDownEvent>(evt =>
        {
            if (LanguagePanel.style.display == DisplayStyle.Flex)
            {
                LanguagePanel.style.display = DisplayStyle.None;
                isLanguageRotated = !isLanguageRotated;
                RotateArrow(LanguageArrow, isLanguageRotated);
            }

            if (SoundPanel.style.display == DisplayStyle.Flex)
            {
                SoundPanel.style.display = DisplayStyle.None;
                isSoundRotated = !isSoundRotated;
                RotateArrow(SoundArrow, isSoundRotated);
            }
        });

        // Unregister callbacks for Language Buttons
        EnglishButton.UnregisterCallback<ClickEvent>(e => OnEnglishButtonClicked());
        TurkishButton.UnregisterCallback<ClickEvent>(e => OnTurkishButtonClicked());
        GermanButton.UnregisterCallback<ClickEvent>(e => OnGermanButtonClicked());
        FrenchButton.UnregisterCallback<ClickEvent>(e => OnFrenchButtonClicked());
        ChineseButton.UnregisterCallback<ClickEvent>(e => OnChineseButtonClicked());
        JapaneseButton.UnregisterCallback<ClickEvent>(e => OnJapaneseButtonClicked());
        ArabicButton.UnregisterCallback<ClickEvent>(e => OnArabicButtonClicked());
        PolishButton.UnregisterCallback<ClickEvent>(e => OnPolishButtonClicked());
        RussianButton.UnregisterCallback<ClickEvent>(e => OnRussianButtonClicked());
        ItalianButton.UnregisterCallback<ClickEvent>(e => OnItalianButtonClicked());
        SpanishButton.UnregisterCallback<ClickEvent>(e => OnSpanishButtonClicked());


        // Unregister all callbacks
        PlayButton.UnregisterCallback<ClickEvent>(e => OnPlayButtonClicked());
        TutorialButton.UnregisterCallback<ClickEvent>(e => OnTutorialButtonClicked());
        CreditsButton.UnregisterCallback<ClickEvent>(e => OnCreditsButtonClicked());
        QuitButton.UnregisterCallback<ClickEvent>(e => OnQuitButtonClicked());
        LanguageButton.UnregisterCallback<ClickEvent>(e => OnLanguageButtonClicked());
        SoundButton.UnregisterCallback<ClickEvent>(e => OnSoundButtonClicked());
        // Unregister for Tutorial Buttons
        TutorialPrevButton.UnregisterCallback<ClickEvent>(e => OnTutorialPrevButtonClicked());
        TutorialNextButton.UnregisterCallback<ClickEvent>(e => OnTutorialNextButtonClicked());
        TutorialBackButton.UnregisterCallback<ClickEvent>(e => OnTutorialBackButtonClicked());
        // Unregister for Credits Buttons
        CreditsBackButton.UnregisterCallback<ClickEvent>(e => OnCreditsBackButtonClicked());
    }
    #endregion
}
