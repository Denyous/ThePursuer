using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour
{
    //All Main Menu Containers
    public VisualElement MainMenu;
    public VisualElement TutorialPanel;
    public VisualElement MainMenuPanel;
    public ScrollView LanguagePanel;
    public VisualElement SoundPanel;
    public VisualElement CreditsPanel;
    public VisualElement NoAdsPanel;

    //Language and Sound Button Variables
    public VisualElement LanguageArrow;
    public VisualElement SoundArrow;
    private bool isLanguageRotated = false;
    private bool isSoundRotated = false;
    public Slider MusicSlider;
    public Slider SfxSlider;
    private AudioManager audioManager;

    //All Main Menu and Sub Menu Buttons
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
    public Button NoAdsButton;
    public Button NoAdsBuyButton;
    public Button NoAdsCancelButton;

    //Language Buttons
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
    public Button HindiButton;
    public Button PortugueseButton;

    //Tutorial Variables
    public Sprite[] TutorialImages;
    public VisualElement TutorialImageContainer;
    public UnityEngine.UIElements.Label TutorialNumber;
    public UnityEngine.UIElements.Label TutorialText;
    private int TutorialCount = 1;
    private int TutorialIndex = 0;

    //Localization Variables
    [SerializeField] private LocalizedString LocalizedHighScore;
    private int highScore;
    public UnityEngine.UIElements.Label HighScoreText;
    public UnityEngine.UIElements.Label LanguageText;


    [SerializeField] private Font ArabicFont;
    [SerializeField] private Font ChineseFont;
    [SerializeField] private Font JapaneseFont;
    [SerializeField] private Font HindiFont;
    [SerializeField] private Font DefaultFont;


    public void InitiateSecond()
    {
        GetReferencesForUIElements();
        RegisterCallBacks();
        LocalizedHighScore.StringChanged += UpdateHighScore;
    }
    public void InitiateFirst()
    {
        GetReferencesForPanels();
        audioManager = FindAnyObjectByType<AudioManager>();
        //PlayerPrefs.DeleteKey("HighScore");
    }

    //SetSound() should be in the start because audio mixer should be initialized
    public void InitiateThird()
    {
        LocalizationSettings.InitializationOperation.WaitForCompletion();
        if (!PlayerPrefs.HasKey("LanguageInitialized"))
        {
            // First time: use system locale
            Locale systemLocale = LocalizationSettings.SelectedLocale;

            // Optionally save the code (e.g., "en", "tr") for later use
            PlayerPrefs.SetString("Language", systemLocale.Identifier.Code);
            PlayerPrefs.SetInt("LanguageInitialized", 1);
            PlayerPrefs.Save();

            string language = systemLocale.Identifier.Code;

            SetLanguageButtonText(language);
            SetLanguage(language);
            SetLocalizedTextForUIElements();
        }
        else
        {
            string savedLangCode = PlayerPrefs.GetString("Language");

            SetLanguageButtonText(savedLangCode);
            SetLanguage(savedLangCode);
            SetLocalizedTextForUIElements();
        }
        SetSound();
    }
    
    private void OnDisable()
    {
        UnRegisterCallBacks();
        LocalizedHighScore.StringChanged -= UpdateHighScore;
    }


    #region LanguageButtonEvents

    private void OnSpanishButtonClicked()
    {
        ClickSound();
        SetLanguage("es");
        SetLocalizedTextForUIElements();
    }
    private void OnItalianButtonClicked()
    {
        ClickSound();
        SetLanguage("it");
        SetLocalizedTextForUIElements();
    }
    private void OnRussianButtonClicked()
    {
        ClickSound();
        SetLanguage("ru");
        SetLocalizedTextForUIElements();
    }
    private void OnPolishButtonClicked()
    {
        ClickSound();
        SetLanguage("pl");
        SetLocalizedTextForUIElements();
    }
    private void OnArabicButtonClicked()
    {
        ClickSound();
        SetLanguage("ar");
        SetLocalizedTextForUIElements();
    }
    private void OnJapaneseButtonClicked()
    {
        ClickSound();
        SetLanguage("ja");
        SetLocalizedTextForUIElements();
    }
    private void OnChineseButtonClicked()
    {
        ClickSound();
        SetLanguage("zh");
        SetLocalizedTextForUIElements();
    }
    private void OnFrenchButtonClicked()
    {
        ClickSound();
        SetLanguage("fr");
        SetLocalizedTextForUIElements();
    }
    private void OnGermanButtonClicked()
    {
        ClickSound();
        SetLanguage("de");
        SetLocalizedTextForUIElements();
    }
    private void OnTurkishButtonClicked()
    {
        ClickSound();
        SetLanguage("tr");
        SetLocalizedTextForUIElements();
    }
    private void OnEnglishButtonClicked()
    {
        ClickSound();
        SetLanguage("en");
        SetLocalizedTextForUIElements();
    }
    private void OnHindiButtonClicked()
    {
        ClickSound();
        SetLanguage("hi");
        SetLocalizedTextForUIElements();
    }
    private void OnPortugueseButtonClicked()
    {
        ClickSound();
        SetLanguage("pt");
        SetLocalizedTextForUIElements();
    }

    #endregion

    #region TutorialButtonEvents

    private void OnTutorialBackButtonClicked()
    {
        ClickSound();
        TogglePanels(MainMenuPanel, TutorialPanel);
    }
    private void OnTutorialNextButtonClicked()
    {
        ClickSound();
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

        ChangeTutorialText();
    }
    private void ChangeTutorialText()
    {
        switch (TutorialIndex)
        {
            case 0:
                TutorialText.text = LocalizationSettings.StringDatabase.GetLocalizedString("MainMenu", "MainMenuTutorialText1");
                break;
            case 1:
                TutorialText.text = LocalizationSettings.StringDatabase.GetLocalizedString("MainMenu", "MainMenuTutorialText2");
                break;
            case 2:
                TutorialText.text = LocalizationSettings.StringDatabase.GetLocalizedString("MainMenu", "MainMenuTutorialText3");
                break;
            case 3:
                TutorialText.text = LocalizationSettings.StringDatabase.GetLocalizedString("MainMenu", "MainMenuTutorialText4");
                break;
            case 4:
                TutorialText.text = LocalizationSettings.StringDatabase.GetLocalizedString("MainMenu", "MainMenuTutorialText5");
                break;
            case 5:
                TutorialText.text = LocalizationSettings.StringDatabase.GetLocalizedString("MainMenu", "MainMenuTutorialText6");
                break;
            case 6:
                TutorialText.text = LocalizationSettings.StringDatabase.GetLocalizedString("MainMenu", "MainMenuTutorialText7");
                break;
            case 7:
                TutorialText.text = LocalizationSettings.StringDatabase.GetLocalizedString("MainMenu", "MainMenuTutorialText8");
                break;
            case 8:
                TutorialText.text = LocalizationSettings.StringDatabase.GetLocalizedString("MainMenu", "MainMenuTutorialText9");
                break;
            default:
                break;
        }
    }
    private void OnTutorialPrevButtonClicked()
    {
        ClickSound();
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
        ChangeTutorialText();
    }
    private void OnTutorialButtonClicked()
    {
        ClickSound();
        TutorialCount = 1;
        TutorialIndex = 0;
        TutorialPrevButton.SetEnabled(false);
        TutorialNextButton.SetEnabled(true);
        TogglePanels(TutorialPanel, MainMenuPanel);
        TutorialImageContainer.style.backgroundImage = new StyleBackground(TutorialImages[TutorialIndex]);
        TutorialNumber.text = TutorialCount + " / " + TutorialImages.Length;
        ChangeTutorialText();
    }

    #endregion

    #region NoAdsButtonEvents
    private void OnNoAdsButtonClicked()
    {
        ClickSound();
        TogglePanels(NoAdsPanel, MainMenuPanel);
    }

    private void OnNoAdsbuyButtonClicked()
    {
        ClickSound();
        IAPManager.instance.Purchase();
        TogglePanels(MainMenuPanel, NoAdsPanel);
    }

    private void OnNoAdsCancelButtonClicked()
    {
        ClickSound();
        TogglePanels(MainMenuPanel, NoAdsPanel);
    }

    #endregion

    #region ButtonEvents

    //Language Buttons

    private void OnSoundButtonClicked()
    {
        ClickSound();
        isSoundRotated = !isSoundRotated;
        RotateArrow(SoundArrow, isSoundRotated);
        TogglePanel(SoundPanel);
    }
    private void OnLanguageButtonClicked()
    {
        ClickSound();
        isLanguageRotated = !isLanguageRotated;
        RotateArrow(LanguageArrow, isLanguageRotated);
        TogglePanel(LanguagePanel);
    }
    private void OnCreditsBackButtonClicked()
    {
        ClickSound();
        TogglePanels(MainMenuPanel, CreditsPanel);
    }
    private void OnPlayButtonClicked()
    {
        ClickSound();
        if (PlayerPrefs.GetInt("Tutorialplayed", 0) == 1)
        {
            PlayerPrefs.SetInt("Tutorialplayed", 1);
            PlayerPrefs.Save();

            SceneManager.LoadScene("Tutorial");
        }
        else
        {
            SceneManager.LoadScene("Game");
            GameStats.StartTimer();
        }
    }
    private void OnCreditsButtonClicked()
    {
        ClickSound();
        TogglePanels(CreditsPanel, MainMenuPanel);
    }
    private void OnQuitButtonClicked()
    {
        ClickSound();
        Application.Quit();

#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
    }
    #endregion

    #region HelperFunctions
    private void UpdateHighScore(string value)
    {
        HighScoreText.text = value;
    }
    public void ClickSound()
    {
        if (audioManager != null)
        {
            audioManager.SFXPlayOneShot(audioManager.buttonClickSFX);
        }
    }
    private void SetLocalizedTextForUIElements()
    {
        var buttons = MainMenu.Query<Button>().ToList();
        var labels = MainMenu.Query<UnityEngine.UIElements.Label>().ToList();
        var sliders = MainMenu.Query<Slider>().ToList();

        foreach (Slider slider in sliders)
        {
            switch (slider.name)
            {
                case "MusicSlider":
                    SetLocalizedText(slider, "MainMenuMusic");
                    break;
                case "SfxSlider":
                    SetLocalizedText(slider, "MainMenuSound");
                    break;
                default:
                    break;
            }
        }

        foreach (UnityEngine.UIElements.Label label in labels)
        {
            switch (label.name)
            {
                case "SoundSelectorLabel":
                    SetLocalizedText(label, "MainMenuSoundButtonLabel");
                    break;
                case "TutorialTitle":
                    SetLocalizedText(label, "MainMenuTutorialTitle");
                    break;
                case "GameDesignTitle":
                    SetLocalizedText(label, "MainMenuCreditsGameDesign");
                    break;
                case "ProgrammingTitle":
                    SetLocalizedText(label, "MainMenuCreditsDevelopers");
                    break;
                case "GraphicDesignTitle":
                    SetLocalizedText(label, "MainMenuCreditsGraphicDesign");
                    break;
                case "AudioDesignTitle":
                    SetLocalizedText(label, "MainMenuCreditsAudioDesign");
                    break;
                default:
                    break;
            }
        }
        foreach (Button button in buttons)
        {
            switch (button.name)
            {
                case "StartButton":
                    SetLocalizedText(button, "MainMenuStart");
                    break;
                case "TutorialButton":
                    SetLocalizedText(button, "MainMenuTutorial");
                    break;
                case "CreditsButton":
                    SetLocalizedText(button, "MainMenuCredits");
                    break;
                case "QuitButton":
                    SetLocalizedText(button, "MainMenuQuit");
                    break;
                default:
                    break;
            }
        }
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
            if (element is UnityEngine.UIElements.Label label) label.text = localizedValue;
            else if (element is Button button) button.text = localizedValue;
            else if (element is Slider slider) slider.label = localizedValue;
        };
    }
    private void SetSound()
    {
        if (PlayerPrefs.HasKey("Music") && PlayerPrefs.HasKey("SFX"))
        {
            MusicSlider.value = PlayerPrefs.GetFloat("Music");
            SfxSlider.value = PlayerPrefs.GetFloat("SFX");
        }

        audioManager.SetMusicVolume(MusicSlider.value);
        audioManager.SetSFXVolume(SfxSlider.value);
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
                PlayerPrefs.SetString("Language", languageCode);
                PlayerPrefs.Save();
                SetLanguageButtonText(languageCode);
                SetFont(languageCode);
                highScore = PlayerPrefs.GetInt("HighScore");
                LocalizedHighScore.Arguments = new object[] { highScore };
                HighScoreText.text = LocalizedHighScore.GetLocalizedString();
                yield break;
            }
        }
    }
    private void SetLanguageButtonText(string LanguageCode)
    {
        switch (LanguageCode)
        {
            case "ar":
                LanguageText.text = "اللغة العربية";
                break;
            case "zh":
                LanguageText.text = "中文";
                break;
            case "en":
                LanguageText.text = "English";
                break;
            case "fr":
                LanguageText.text = "Français";
                break;
            case "de":
                LanguageText.text = "Deutsch";
                break;
            case "it":
                LanguageText.text = "Italiano";
                break;
            case "ja":
                LanguageText.text = "日本語";
                break;
            case "pl":
                LanguageText.text = "Polski";
                break;
            case "ru":
                LanguageText.text = "Русский";
                break;
            case "es":
                LanguageText.text = "Español";
                break;
            case "tr":
                LanguageText.text = "Türkçe";
                break;
            case "hi":
                LanguageText.text = "हिन्दी";
                break;
            case "pt":
                LanguageText.text = "Português";
                break;
            default:
                break;
        }
    }
    private void SetFont(string LanguageCode)
    {
        var buttons = MainMenu.Query<Button>().ToList();
        var labels = MainMenu.Query<UnityEngine.UIElements.Label>().ToList();
        var sliders = MainMenu.Query<Slider>().ToList();

        switch (LanguageCode)
        {
            case "ar":
                SetFontForUIElements(ArabicFont, buttons, labels, sliders, true);
                break;
            case "zh":
                SetFontForUIElements(ChineseFont, buttons, labels, sliders, false);
                break;
            case "ja":
                SetFontForUIElements(JapaneseFont, buttons, labels, sliders, false);
                break;
            case "hi":
                SetFontForUIElements(HindiFont, buttons, labels, sliders, false);
                break;
            default:
                SetFontForUIElements(DefaultFont, buttons, labels, sliders, false);
                break;
        }
    }
    private void SetFontForUIElements(Font font, List<Button> buttons, List<UnityEngine.UIElements.Label> labels, 
        List<Slider> sliders, bool isArabic)
    {
        foreach (var button in buttons)
        {
            if (button.name != "ArabicButton" &&
                button.name != "JapaneseButton" &&
                button.name != "ChineseButton" &&
                button.name != "EnglishButton" &&
                button.name != "TurkishButton" &&
                button.name != "FrenchButton" && 
                button.name != "ItalianButton" &&
                button.name != "PolishButton" &&
                button.name != "SpanishButton" &&
                button.name != "RussianButton" &&
                button.name != "GermanButton" &&
                button.name != "HindiButton" &&
                button.name != "PortugueseButton")
            {
                button.style.unityFont = font;
                if (isArabic)
                {
                    button.languageDirection = UnityEngine.UIElements.LanguageDirection.RTL;
                }
                else
                {
                    button.languageDirection = UnityEngine.UIElements.LanguageDirection.LTR;
                }
            }
        }

        foreach (var label in labels)
        {
            if (label.name != "GameTitle" &&
                label.name != "Deniz" &&
                label.name != "Gorkem" &&
                label.name != "Oguz" &&
                label.name != "Ates")
            {
                label.style.unityFont = font;
                if (isArabic)
                {
                    label.languageDirection = UnityEngine.UIElements.LanguageDirection.RTL;
                }
                else
                {
                    label.languageDirection = UnityEngine.UIElements.LanguageDirection.LTR;
                }
            }
        }

        foreach (var slider in sliders)
        {
            slider.style.unityFont = font;
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
        LanguagePanel = MainMenu.Q<ScrollView>("LanguageMenu");
        SoundPanel = MainMenu.Q<VisualElement>("SoundMenu");
        CreditsPanel = MainMenu.Q<VisualElement>("CreditsPanel");
        NoAdsPanel = MainMenu.Q<VisualElement>("NoAdsPanel");

        LanguageArrow = MainMenu.Q<VisualElement>("LanguageSelectorImage");
        SoundArrow = MainMenu.Q<VisualElement>("SoundSelectorImage");

        Application.targetFrameRate = 60;
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
        NoAdsButton = MainMenu.Q<Button>("NoAdsButton");
        NoAdsBuyButton = MainMenu.Q<Button>("BuyButton");
        NoAdsCancelButton = MainMenu.Q<Button>("CancelButton");

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
        HindiButton = MainMenu.Q<Button>("HindiButton");
        PortugueseButton = MainMenu.Q<Button>("PortugueseButton");

        //Get the reference for the tutorial image container and number
        TutorialImageContainer = MainMenu.Q<VisualElement>("TutorialImageContainer");
        TutorialNumber = MainMenu.Q<UnityEngine.UIElements.Label>("TutorialNumber");
        TutorialText = MainMenu.Q<UnityEngine.UIElements.Label>("TutorialText");

        // Get the reference for the HighScore text
        HighScoreText = MainMenu.Q<UnityEngine.UIElements.Label>("HighScore");
        LanguageText = MainMenu.Q<UnityEngine.UIElements.Label>("LanguageSelectorLabel");
    }
    private void RegisterCallBacks()
    {
        // Events for Sliders
        MusicSlider.RegisterValueChangedCallback(e =>
        {
            float volume = e.newValue;
            audioManager.SetMusicVolume(volume);
        });

        SfxSlider.RegisterValueChangedCallback(e =>
        {
            float volume = e.newValue;
            audioManager.SetSFXVolume(volume);
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
        NoAdsButton.RegisterCallback<ClickEvent>(e => OnNoAdsButtonClicked());
        NoAdsBuyButton.RegisterCallback<ClickEvent>(e => OnNoAdsbuyButtonClicked());
        NoAdsCancelButton.RegisterCallback<ClickEvent>(e => OnNoAdsCancelButtonClicked());

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
        PortugueseButton.RegisterCallback<ClickEvent>(e => OnPortugueseButtonClicked());
        HindiButton.RegisterCallback<ClickEvent>(e => OnHindiButtonClicked());

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
            audioManager.SetMusicVolume(volume);
        });

        SfxSlider.UnregisterValueChangedCallback(e =>
        {
            float volume = e.newValue;
            audioManager.SetSFXVolume(volume);
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
        PortugueseButton.UnregisterCallback<ClickEvent>(e => OnPortugueseButtonClicked());
        HindiButton.UnregisterCallback<ClickEvent>(e => OnHindiButtonClicked());


        // Unregister all callbacks
        PlayButton.UnregisterCallback<ClickEvent>(e => OnPlayButtonClicked());
        TutorialButton.UnregisterCallback<ClickEvent>(e => OnTutorialButtonClicked());
        CreditsButton.UnregisterCallback<ClickEvent>(e => OnCreditsButtonClicked());
        QuitButton.UnregisterCallback<ClickEvent>(e => OnQuitButtonClicked());
        LanguageButton.UnregisterCallback<ClickEvent>(e => OnLanguageButtonClicked());
        SoundButton.UnregisterCallback<ClickEvent>(e => OnSoundButtonClicked());
        NoAdsButton.UnregisterCallback<ClickEvent>(e => OnNoAdsButtonClicked());

        // Unregister for Tutorial Buttons
        TutorialPrevButton.UnregisterCallback<ClickEvent>(e => OnTutorialPrevButtonClicked());
        TutorialNextButton.UnregisterCallback<ClickEvent>(e => OnTutorialNextButtonClicked());
        TutorialBackButton.UnregisterCallback<ClickEvent>(e => OnTutorialBackButtonClicked());
        // Unregister for Credits Buttons
        CreditsBackButton.UnregisterCallback<ClickEvent>(e => OnCreditsBackButtonClicked());
    }

    #endregion
}
