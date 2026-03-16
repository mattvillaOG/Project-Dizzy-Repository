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

        public Sprite image;
    }

    [Header("Pages")]
    [SerializeField] private TutorialPage[] pages;

    [Header("UI References")]
    [SerializeField] private TMP_Text bodyTextUI;   // or use Text if you prefer
    [SerializeField] private Image imageUI;

    [Header("Buttons")]
    [SerializeField] private Button nextButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Button startButton;

    [Header("Optional")]
    [Tooltip("If true, tutorial starts on page 0 when enabled.")]
    [SerializeField] private bool resetOnEnable = true;

    private int pageIndex = 0;

    private void OnEnable()
    {
        if (resetOnEnable) pageIndex = 0;
        Refresh();
    }

    private void Awake()
    {
        // Wire button clicks (so you don't need to add OnClick events manually)
        if (nextButton != null) nextButton.onClick.AddListener(Next);
        if (backButton != null) backButton.onClick.AddListener(Back);
        // Start button: you can also assign this in inspector if you want.
        if (startButton != null) startButton.onClick.AddListener(StartGame);
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
        AudioMachine.Instance.PlaySFX("BUTTON");

    }

    public void Back()
    {
        if (pages == null || pages.Length == 0) return;

        pageIndex = Mathf.Max(pageIndex - 1, 0);
        Refresh();
        AudioMachine.Instance.PlaySFX("BUTTON");
    }

    private void Refresh()
    {
        if (pages == null || pages.Length == 0)
        {
            if (bodyTextUI != null) bodyTextUI.text = "";
            if (imageUI != null)
            {
                imageUI.sprite = null;
                imageUI.enabled = false;
            }

            SetButtonVisible(backButton, false);
            SetButtonVisible(nextButton, false);
            SetButtonVisible(startButton, true); // or false if you prefer
            return;
        }

        // Clamp in case pages changed in editor while playing
        pageIndex = Mathf.Clamp(pageIndex, 0, pages.Length - 1);

        // Update text
        if (bodyTextUI != null)
            bodyTextUI.text = pages[pageIndex].bodyText;

        // Update image
        if (imageUI != null)
        {
            imageUI.sprite = pages[pageIndex].image;
            imageUI.enabled = (pages[pageIndex].image != null);
        }

        bool isFirst = pageIndex == 0;
        bool isLast = pageIndex == pages.Length - 1;

        SetButtonVisible(backButton, !isFirst);
        SetButtonVisible(nextButton, !isLast);
        SetButtonVisible(startButton, isLast);
    }

    private void SetButtonVisible(Button btn, bool visible)
    {
        if (btn == null) return;
        btn.gameObject.SetActive(visible);
    }

    public void StartGame()
    {
        Time.timeScale = 1f; Destroy(gameObject); // UN-freeze the game + disable start menu
        AudioMachine.Instance.PlaySFX("BUTTON");
    }
}