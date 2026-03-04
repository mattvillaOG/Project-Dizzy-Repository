using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tutorial : MonoBehaviour
{
    [System.Serializable]
    public class TutorialPage
    {
        [TextArea(3, 10)]
        public string bodyText;

        [Tooltip("Animator state name to play for this page (ex: Page0, Page1)")]
        public string animationState;
    }

    [Header("Pages")]
    [SerializeField] private TutorialPage[] pages;

    [Header("UI References")]
    [SerializeField] private TMP_Text bodyTextUI;

    [Header("Animated Visual")]
    [SerializeField] private Animator visualAnimator; // Animator on your UI Image

    [Header("Buttons")]
    [SerializeField] private Button nextButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Button startButton;

    private int pageIndex = 0;

    private void Awake()
    {
        if (nextButton != null) nextButton.onClick.AddListener(Next);
        if (backButton != null) backButton.onClick.AddListener(Back);
        if (startButton != null) startButton.onClick.AddListener(StartGame);
    }

    private void OnEnable()
    {
        pageIndex = 0;
        Refresh();
    }

    void Update()
    {
        Time.timeScale = 0f; // Freeze the game
    }

    public void Next()
    {
        if (pages == null || pages.Length == 0) return;
        pageIndex = Mathf.Min(pageIndex + 1, pages.Length - 1);
        Refresh();
    }

    public void Back()
    {
        if (pages == null || pages.Length == 0) return;
        pageIndex = Mathf.Max(pageIndex - 1, 0);
        Refresh();
    }

    private void Refresh()
    {
        if (pages == null || pages.Length == 0) return;

        bool isFirst = pageIndex == 0;
        bool isLast = pageIndex == pages.Length - 1;

        if (bodyTextUI != null)
            bodyTextUI.text = pages[pageIndex].bodyText;

        if (visualAnimator != null && !string.IsNullOrEmpty(pages[pageIndex].animationState))
        {
            visualAnimator.Play(pages[pageIndex].animationState, 0, 0f);
        }

        if (backButton != null) backButton.gameObject.SetActive(!isFirst);
        if (nextButton != null) nextButton.gameObject.SetActive(!isLast);
        if (startButton != null) startButton.gameObject.SetActive(isLast);
    }

    public void StartGame()
    {
        Time.timeScale = 1f; Destroy(gameObject); // UN-freeze the game + disable start menu 
    }
}