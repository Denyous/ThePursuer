using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class TutorialController : MonoBehaviour
{
    public VisualElement panel;
    public VisualElement startTutorialPanel;
    public VisualElement tutorialPanel;
    public VisualElement skipTutorialPanel;
    public VisualElement endTutorialPanel;
    public VisualElement tutorialImagePanel;

    public Sprite[] TutorialImages;

    public Button goToTutorialButton;
    public Button prevButton;
    public Button nextButton;
    public Button yesButton;
    public Button noButton;
    public Button skipButton;
    public Button goToGameButton;

    public UnityEngine.UIElements.Label welcomeTitle;
    public UnityEngine.UIElements.Label tutorialText;
    public UnityEngine.UIElements.Label tutorialSkipTitle;
    public UnityEngine.UIElements.Label tutorialEndTitle;

    [SerializeField] private Font ArabicFont;
    [SerializeField] private Font ChineseFont;
    [SerializeField] private Font JapaneseFont;
    [SerializeField] private Font HindiFont;
    [SerializeField] private Font DefaultFont;

    private int TutorialIndex = 0;
    private string PanelName = string.Empty;

    // Start is called before the first frame update
    void Start()
    {
        PanelName = "Start";
        GetReferencesForUIElements();
        string languageCode = PlayerPrefs.GetString("Language");
        SetFont(languageCode);
        SetLocalizedTextForUIElements();
        tutorialImagePanel.style.backgroundImage = new StyleBackground(TutorialImages[TutorialIndex]);
        ShowTutorialText(TutorialIndex);
    }

    private void TogglePanels(VisualElement showPanel, VisualElement hidePanel)
    {
        showPanel.style.display = DisplayStyle.Flex;
        hidePanel.style.display = DisplayStyle.None;
    }

    private void SetLocalizedText(VisualElement element, string entryKey)
    {
        var localizedString = new LocalizedString
        {
            TableReference = "Tutorial",
            TableEntryReference = entryKey
        };

        // Register callback so it updates immediately and on language switch
        localizedString.StringChanged += (localizedValue) =>
        {
            if (element is UnityEngine.UIElements.Label label) label.text = localizedValue;
            else if (element is Button button) button.text = localizedValue;
        };
    }

    private void SetLocalizedTextForUIElements()
    {
        var buttons = panel.Query<Button>().ToList();
        var labels = panel.Query<UnityEngine.UIElements.Label>().ToList();

        foreach (UnityEngine.UIElements.Label label in labels)
        {
            switch (label.name)
            {
                case "Txt_WelcomeTitle":
                    SetLocalizedText(label, "Tutorial_Welcome");
                    break;
                case "Txt_TutorialSkipTitle":
                    SetLocalizedText(label, "Tutorial_SkipText");
                    break;
                case "Txt_TutorialEndTitle":
                    SetLocalizedText(label, "Tutorial_FinishedText");
                    break;
                default:
                    break;
            }
        }
        foreach (Button button in buttons)
        {
            switch (button.name)
            {
                case "Btn_Skip":
                    SetLocalizedText(button, "Tutorial_SkipButtonText");
                    break;
                case "Btn_Yes":
                    SetLocalizedText(button, "Tutorial_YesButton");
                    break;
                case "Btn_No":
                    SetLocalizedText(button, "Tutorial_NoButton");
                    break;
                case "Btn_GoToGame":
                    SetLocalizedText(button, "Tutorial_Okbutton");
                    break;
                default:
                    break;
            }
        }
    }

    private void SetFont(string LanguageCode)
    {
        var buttons = panel.Query<Button>().ToList();
        var labels = panel.Query<UnityEngine.UIElements.Label>().ToList();

        switch (LanguageCode)
        {
            case "ar":
                SetFontForUIElements(ArabicFont, buttons, labels, true);
                break;
            case "zh":
                SetFontForUIElements(ChineseFont, buttons, labels, false);
                break;
            case "ja":
                SetFontForUIElements(JapaneseFont, buttons, labels, false);
                break;
            case "hi":
                SetFontForUIElements(HindiFont, buttons, labels, false);
                break;
            default:
                SetFontForUIElements(DefaultFont, buttons, labels, false);
                break;
        }
    }

    private void SetFontForUIElements(Font font, List<Button> buttons, List<UnityEngine.UIElements.Label> labels, bool isArabic)
    {
        foreach (var button in buttons)
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

        foreach (var label in labels)
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

    private void GetReferencesForUIElements()
    {
        panel = GetComponent<UIDocument>().rootVisualElement;
        panel.Insert(0, startTutorialPanel);
        startTutorialPanel = panel.Q<VisualElement>("Panel_StartTutorial");
        tutorialPanel = panel.Q<VisualElement>("Panel_Tutorial");
        skipTutorialPanel = panel.Q<VisualElement>("Panel_SkipTutorial");
        endTutorialPanel = panel.Q<VisualElement>("Panel_EndTutorial");
        tutorialImagePanel = panel.Q<VisualElement>("Image_Tutorial");
        goToTutorialButton = panel.Q<Button>("Btn_GoToTutorial");
        prevButton = panel.Q<Button>("Btn_Prev");
        nextButton = panel.Q<Button>("Btn_Next");
        yesButton = panel.Q<Button>("Btn_Yes");
        noButton = panel.Q<Button>("Btn_No");
        skipButton = panel.Q<Button>("Btn_Skip");
        goToGameButton = panel.Q<Button>("Btn_GoToGame");
        welcomeTitle = panel.Q<UnityEngine.UIElements.Label>("Txt_WelcomeTitle");
        tutorialText = panel.Q<UnityEngine.UIElements.Label>("Txt_TutorialText");
        tutorialSkipTitle = panel.Q<UnityEngine.UIElements.Label>("Txt_TutorialSkipTitle");
        tutorialEndTitle = panel.Q<UnityEngine.UIElements.Label>("Txt_TutorialEndTitle");
        RegisterCallbacks();
    }

    private void RegisterCallbacks()
    {
        goToTutorialButton.clicked += OnGoToTutorialButtonClicked;
        skipButton.clicked += OnSkipButtonClicked;
        yesButton.clicked += OnYesButtonClicked;
        noButton.clicked += OnNoButtonClicked;
        goToGameButton.clicked += OnGoToGameButtonClicked;
        prevButton.clicked += () => OnPrevButtonClicked();
        nextButton.clicked += () => OnNextButtonClicked();
    }

    private void OnGoToGameButtonClicked()
    {
        SceneManager.LoadScene("Game");
    }

    private void OnNoButtonClicked()
    {
        if (PanelName == "Start")
        {
            skipButton.style.display = DisplayStyle.Flex;
            TogglePanels(startTutorialPanel, skipTutorialPanel);
        }
        else if (PanelName == "Tutorial")
        {
            skipButton.style.display = DisplayStyle.Flex;
            TogglePanels(tutorialPanel, skipTutorialPanel);
        }
        AudioManager.instance.SFXPlayOneShot(AudioManager.instance.buttonClickSFX);
    }

    private void OnYesButtonClicked()
    {
        AudioManager.instance.SFXPlayOneShot(AudioManager.instance.buttonClickSFX);
        SceneManager.LoadScene("Game");
    }

    private void OnSkipButtonClicked()
    {
        if (PanelName == "Start")
        {
            skipButton.style.display = DisplayStyle.None;
            TogglePanels(skipTutorialPanel, startTutorialPanel);
        }
        else if (PanelName == "Tutorial")
        {
            skipButton.style.display = DisplayStyle.None;
            TogglePanels(skipTutorialPanel, tutorialPanel);
        }
        AudioManager.instance.SFXPlayOneShot(AudioManager.instance.buttonClickSFX);
    }

    private void OnGoToTutorialButtonClicked()
    {
        AudioManager.instance.SFXPlayOneShot(AudioManager.instance.buttonClickSFX);
        TogglePanels(tutorialPanel, startTutorialPanel);
        PanelName = "Tutorial";
    }

    private void OnNextButtonClicked()
    {
        if (TutorialIndex >= TutorialImages.Length - 1)
        {
            skipButton.style.display = DisplayStyle.None;
            TogglePanels(endTutorialPanel, tutorialPanel);
        }
        else
        {
            TutorialIndex++;
            ShowTutorialImages(TutorialIndex);
            ShowTutorialText(TutorialIndex);
        }
        AudioManager.instance.SFXPlayOneShot(AudioManager.instance.buttonClickSFX);
    }

    private void OnPrevButtonClicked()
    {
        if (TutorialIndex > 0)
        {
            TutorialIndex--;
            ShowTutorialImages(TutorialIndex);
            ShowTutorialText(TutorialIndex);
        }
        AudioManager.instance.SFXPlayOneShot(AudioManager.instance.buttonClickSFX);
    }

    private void ShowTutorialText(int tutorialIndex)
    {
        string localizedTextKey = $"MainMenuTutorialText{tutorialIndex+1}";
        tutorialText.text = LocalizationSettings.StringDatabase.GetLocalizedString("MainMenu", localizedTextKey);
    }

    private void ShowTutorialImages(int tutorialIndex)
    {
        tutorialImagePanel.style.backgroundImage = new StyleBackground(TutorialImages[TutorialIndex]);
    }
}
