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


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        root = uiDocument.rootVisualElement; // Get the root VisualElement from the UIDocument
        TitleScreen = root.Q<VisualElement>("TitleScreen"); // Find the TitleScreen VisualElement by name

        PauseMenu = root.Q<VisualElement>("PauseMenu"); // Find the PauseMenu VisualElement by name
        PauseMenu.style.display = DisplayStyle.None; // Initially hide the PauseMenu

        GameOverPanel = root.Q<VisualElement>("GameOverPanel"); // Find the GameOverPanel VisualElement by name
        GameOverPanel.style.display = DisplayStyle.None; // Initially hide the GameOverPanel

        GameOverlayPanel = root.Q<VisualElement>("GameOverlayPanel"); // Find the GameOverlayPanel VisualElement by name
        progressBar = GameOverlayPanel.Q<ProgressBar>("ProgressBar"); // Find the ProgressBar in the GameOverlayPanel
        scoreLabel = GameOverlayPanel.Q<Label>("ScoreLabel"); // Find the ScoreLabel in the GameOverlayPanel

        GameOverlayPanel.style.display = DisplayStyle.None; // Initially hide the GameOverlayPanel


        //after the animation is done, hide the TitleScreen
        TitleScreen.RegisterCallback<TransitionEndEvent>(e =>
        {
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


        //TODO THis isnt working
        var SubTitleLabel = TitleScreen.Q<Label>("SubTitleLabel"); // Find the SubTitleLabel Label in the TitleScreen
        SubTitleLabel.RegisterCallback<TransitionEndEvent>(e =>
        {
            foreach (var styleProperty in e.stylePropertyNames)
            {
                if (styleProperty == "opacity" && e.target == SubTitleLabel)
                {
                    if (SubTitleLabel.resolvedStyle.opacity >= 100)
                    {
                        SubTitleLabel.style.opacity = 50; // Set the opacity of the SubTitleLabel to 0 (invisible)
                    }
                    else if (SubTitleLabel.resolvedStyle.opacity <= 50)
                    {
                        SubTitleLabel.style.opacity = 100; // Set the opacity of the SubTitleLabel to 1 (visible)
                    }
                }
            }
        });


        GameOverPanel.RegisterCallback<TransitionEndEvent>(e =>
        {
            foreach (var styleProperty in e.stylePropertyNames)
            {
                if (styleProperty == "opacity" && e.target == GameOverPanel)
                {
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


        StartCoroutine(DelayedInit()); // Start the DelayedInit coroutine to initialize the UI after a delay
    }

    IEnumerator DelayedInit()
    {
        yield return new WaitForSeconds(0.5f); // Wait for 0.5 seconds before executing the next line
        var SubTitleLabel = TitleScreen.Q<Label>("SubTitleLabel"); // Find the SubTitleLabel Label in the TitleScreen
        SubTitleLabel.style.opacity = 0; // Set the opacity of the SubTitleLabel to 0 (invisible)

    }


    public void GameOver(string message)
    {
        Time.timeScale = 0; // Stop the game time
        GameOverPanel.style.display = DisplayStyle.Flex; // Show the GameOverPanel
        var gameOverText = GameOverPanel.Q<Label>("GameOverText"); // Find the GameOverText Label in the GameOverPanel
        gameOverText.text = message; // Set the text of the GameOverText Label to the provided message
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
            TitleScreen.style.opacity = 0; // Set the opacity of the TitleScreen to 0 (invisible)
            return;
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame && GameOverPanel.style.display != DisplayStyle.None && GameManager.Instance.playerIsDead)
        {
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
