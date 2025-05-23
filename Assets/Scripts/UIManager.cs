using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using System.Collections;

public class UIManager : Manager<UIManager>
{

    [SerializeField] private UIDocument uiDocument; // Reference to the UIDocument component

    private VisualElement root; // Reference to the root VisualElement of the UI
    private VisualElement TitleScreen;
    private VisualElement PauseMenu;
    private VisualElement GameOverPanel;

    private VisualElement GameOverlayPanel;
    private ProgressBar progressBar; // Reference to the ProgressBar component in the GameOverlayPanel
    private Label scoreLabel;

    private Label SubTitleLabel;

    private VisualElement IntroPanel;


    private VisualElement credits;
    private Label robs;
    private Label emi;


    [SerializeField] Sprite WinningScreen;
    [SerializeField] Sprite LosingScreen;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        root = uiDocument.rootVisualElement; // Get the root VisualElement from the UIDocument
        TitleScreen = root.Q<VisualElement>("TitleScreen"); // Find the TitleScreen VisualElement by name
        credits = root.Q<VisualElement>("credits"); // Find the Credits VisualElement by name
        robs = credits.Q<Label>("rob"); // Find the robs Label in the Credits VisualElement
        emi = credits.Q<Label>("emi"); // Find the robs Label in the Credits VisualElement

        //Click to open link on web browser
        robs.RegisterCallback<ClickEvent>(e =>
        {
            Application.OpenURL("https://linktr.ee/robscatch?utm_source=linktree_admin_share");
        });

        emi.RegisterCallback<ClickEvent>(e =>
        {
            Application.OpenURL("https://soundcloud.com/thesouthwestsax");
        });

        PauseMenu = root.Q<VisualElement>("PauseMenu"); // Find the PauseMenu VisualElement by name
        PauseMenu.style.display = DisplayStyle.None; // Initially hide the PauseMenu

        GameOverPanel = root.Q<VisualElement>("GameOverPanel"); // Find the GameOverPanel VisualElement by name
        GameOverPanel.style.display = DisplayStyle.None; // Initially hide the GameOverPanel

        GameOverlayPanel = root.Q<VisualElement>("GameOverlayPanel"); // Find the GameOverlayPanel VisualElement by name
        progressBar = GameOverlayPanel.Q<ProgressBar>("ProgressBar"); // Find the ProgressBar in the GameOverlayPanel
        scoreLabel = GameOverlayPanel.Q<Label>("ScoreLabel"); // Find the ScoreLabel in the GameOverlayPanel


        IntroPanel = root.Q<VisualElement>("IntroPanel"); // Find the IntroPanel VisualElement by name

        GameOverlayPanel.style.display = DisplayStyle.None; // Initially hide the GameOverlayPanel


        //after the animation is done, hide the TitleScreen
        TitleScreen.RegisterCallback<TransitionEndEvent>(e =>
        {
            Debug.Log("TitleScreen TransitionEndEvent triggered");
            foreach (var styleProperty in e.stylePropertyNames)
            {
                if (styleProperty == "opacity" && e.target == TitleScreen)
                {
                    Debug.Log("Animation finished, hiding TitleScreen.");
                    TitleScreen.style.display = DisplayStyle.None; // Hide the TitleScreen after the animation is done

                    //Start the Game
                    GameManager.Instance.InitGame(); // Call the InitGame method from the GameManager
                    GameOverlayPanel.style.display = DisplayStyle.Flex; // Show the GameOverlayPanel
                }
            }
        });


        SubTitleLabel = TitleScreen.Q<Label>("SubTitleLabel"); // Find the SubTitleLabel Label in the TitleScreen


        GameOverPanel.RegisterCallback<TransitionEndEvent>(e =>
        {
            Debug.Log("GameOverPanel TransitionEndEvent triggered");
            foreach (var styleProperty in e.stylePropertyNames)
            {
                if (styleProperty == "opacity" && e.target == GameOverPanel)
                {
                    if (GameOverPanel.style.opacity == 100)
                    {
                        Debug.Log("GameOver panel is fading in");
                        return;
                    }
                    Debug.Log("Animation finished, hiding GameOverPanel.");
                    GameOverPanel.style.display = DisplayStyle.None; // Hide the GameOverPanel after the animation is done

                    GameManager.Instance.InitGame();
                    GameOverlayPanel.style.display = DisplayStyle.Flex; // Show the GameOverlayPanel
                }
            }
        });

        var ResumeButton = PauseMenu.Q<Button>("ResumeButton"); // Find the ResumeButton in the PauseMenu
        ResumeButton.RegisterCallback<ClickEvent>(e =>
        {
            ResumeGame(); // Call the ResumeGame method when the button is clicked
        });

        var QuitButton = PauseMenu.Q<Button>("QuitButton"); // Find the QuitButton in the PauseMenu
        QuitButton.RegisterCallback<ClickEvent>(e =>
        {
            GameManager.Instance.QuitGame(); // Call the QuitGame method from the GameManager when the button is clicked
        });


    }

    public void DisplayIntroPanel()
    {
        IntroPanel.style.display = DisplayStyle.Flex; // Show the IntroPanel
    }

    public void HideIntroPanel()
    {
        IntroPanel.style.display = DisplayStyle.None; // Hide the IntroPanel
    }

    public void GameOver(string message, bool playerWin)
    {
        GameOverPanel.style.opacity = 100; // Set the opacity of the GameOverPanel to 100 (fully visible)

        GameOverlayPanel.style.display = DisplayStyle.None; // Hide the GameOverlayPanel
        var gameOverText = GameOverPanel.Q<Label>("GameOverText"); // Find the GameOverText Label in the GameOverPanel
        gameOverText.text = message; // Set the text of the GameOverText Label to the provided message

        GameOverPanel.style.backgroundImage = new StyleBackground( playerWin ? WinningScreen : LosingScreen); // Set the background image of the GameOverPanel based on playerWin


        Debug.Log("GameOver called about to display panel"); // Log the GameOver action
        GameOverPanel.style.display = DisplayStyle.Flex; // Show the GameOverPanel
    }
    void UpdateProgressBar(float value)
    {
        if (progressBar == null)
        {
            return;
        }
        progressBar.value = value; // Update the value of the ProgressBar
    }

    void UpdateScoreLabel(int score)
    {
        if (scoreLabel == null)
        {
            return;
        }
        scoreLabel.text = $"Boards: {score}"; // Update the text of the ScoreLabel
    }

    private void ResumeGame()
    {
        PauseMenu.style.display = DisplayStyle.None; // Hide the PauseMenu
        GameManager.Instance.ResumeGame(); // Call the ResumeGame method from the GameManager when the button is clicked
    }

    // Update is called once per frame
    void Update()
    {

        if (Keyboard.current.spaceKey.wasPressedThisFrame && TitleScreen.style.display != DisplayStyle.None)
        {
            Debug.Log("TitleScreen space key pressed");
            TitleScreen.style.opacity = 0; // Set the opacity of the TitleScreen to 0 (invisible)
            return;
        }
        else if (Keyboard.current.spaceKey.wasPressedThisFrame && GameOverPanel.style.display != DisplayStyle.None && GameManager.Instance.playerIsDead)
        {
            Debug.Log("GameOverPanel space key pressed");
            GameOverPanel.style.opacity = 0;// Hide the GameOverPanel
            return;
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (GameManager.Instance.IsGamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseMenu.style.display = DisplayStyle.Flex; // Show the PauseMenu
                GameManager.Instance.PauseGame(); // Pause the game if it's running
            }
        }

        UpdateProgressBar(GameManager.Instance.GetTimeRemaining() * 100.0f / GameManager.Instance.GetShiftDuration());

        UpdateScoreLabel(GameManager.Instance.NumBoardsCleared); // Update the score label with the number of boards cleared

    }
}
