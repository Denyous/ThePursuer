using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System;

public class TutorialController : MonoBehaviour
{
    a
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

    public Label welcomeTitle;
    public Label tutorialText;
    public Label tutorialSkipTitle;
    public Label tutorialEndTitle;

    [SerializeField] private Font ArabicFont;
    [SerializeField] private Font ChineseFont;
    [SerializeField] private Font JapaneseFont;
    [SerializeField] private Font HindiFont;
    [SerializeField] private Font DefaultFont;

    private int TutorialIndex = 1;

    // Start is called before the first frame update
    void Start()
    {
        SetLocalizedTextForUIElements();
        GetReferencesForUIElements();
        string languageCode = PlayerPrefs.GetString("Language", "en");
        SetFont(languageCode);
        tutorialImagePanel.style.backgroundImage = new StyleBackground(TutorialImages[TutorialIndex]);
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
            if (element is Label label) label.text = localizedValue;
            else if (element is Button button) button.text = localizedValue;
        };
    }

    private void SetLocalizedTextForUIElements()
    {
        var buttons = panel.Query<Button>().ToList();
        var labels = panel.Query<Label>().ToList();

        foreach (Label label in labels)
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
                    SetLocalizedText(button, "Tutorial_OkButton");
                    break;
                default:
                    break;
            }
        }
    }

    private void SetFont(string LanguageCode)
    {
        var buttons = panel.Query<Button>().ToList();
        var labels = panel.Query<Label>().ToList();

        switch (LanguageCode)
        {
            case "ar":
                SetFontForUIElements(ArabicFont, buttons, labels);
                break;
            case "zh":
                SetFontForUIElements(ChineseFont, buttons, labels);
                break;
            case "ja":
                SetFontForUIElements(JapaneseFont, buttons, labels);
                break;
            case "hi":
                SetFontForUIElements(HindiFont, buttons, labels);
                break;
            default:
                SetFontForUIElements(DefaultFont, buttons, labels);
                break;
        }
    }

    private void SetFontForUIElements(Font font, List<Button> buttons, List<Label> labels)
    {
        foreach (var button in buttons)
        {
            button.style.unityFont = font;
        }

        foreach (var label in labels)
        {
            label.style.unityFont = font;
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
        welcomeTitle = panel.Q<Label>("Txt_WelcomeTitle");
        tutorialText = panel.Q<Label>("Txt_TutorialText");
        tutorialSkipTitle = panel.Q<Label>("Txt_TutorialSkipTitle");
        tutorialEndTitle = panel.Q<Label>("Txt_TutorialEndTitle");
        RegisterCallbacks();
    }

    private void RegisterCallbacks()
    {
        goToTutorialButton.clicked += () => TogglePanels(tutorialPanel, startTutorialPanel);
        skipButton.clicked += () => TogglePanels(tutorialPanel, startTutorialPanel);
        yesButton.clicked += () => TogglePanels(endTutorialPanel, tutorialPanel);
        noButton.clicked += () => TogglePanels(skipTutorialPanel, tutorialPanel);
        goToTutorialButton.clicked += () => TogglePanels(tutorialPanel, startTutorialPanel);
        goToTutorialButton.clicked += () => TogglePanels(tutorialPanel, startTutorialPanel);
        prevButton.clicked += () => ShowPreviousImage();
        nextButton.clicked += () => ShowNextImage();
    }

    private void ShowNextImage()
    {
        if (TutorialIndex >= TutorialImages.Length) return;
        TutorialIndex++;
        ShowTutorialImages(TutorialIndex);
        ShowTutorialText(TutorialIndex);
    }

    private void ShowPreviousImage()
    {
        if (TutorialIndex <= 1) return;
        TutorialIndex--;
        ShowTutorialImages(TutorialIndex);
        ShowTutorialText(TutorialIndex);
    }

    private void ShowTutorialText(int tutorialIndex)
    {
        string localizedTextKey = $"MainMenuTutorialText{tutorialIndex}";
        tutorialText.text = LocalizationSettings.StringDatabase.GetLocalizedString("MainMenu", localizedTextKey);
    }

    private void ShowTutorialImages(int tutorialIndex)
    {
        tutorialImagePanel.style.backgroundImage = new StyleBackground(TutorialImages[TutorialIndex]);
    }
   
}
