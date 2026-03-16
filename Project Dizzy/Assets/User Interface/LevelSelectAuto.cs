using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelSelectAuto : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform gridParent;     // a GridLayoutGroup object
    [SerializeField] private Button levelButtonPrefab; // prefab with a Button on it

    [Header("Build Index Range")]
    [SerializeField] private int firstPlayableBuildIndex = 2;

    [Header("Optional Visuals")]
    [SerializeField] private Sprite lockedSprite;      // optional
    [SerializeField] private Sprite unlockedSprite;    // optional

    private void Start()
    {
        BuildButtons();
        AudioMachine.Instance.PlayMusic("Select Music");
    }

    private void BuildButtons()
    {
        // Clear existing children (so you can hit Play multiple times without duplicates)
        for (int i = gridParent.childCount - 1; i >= 0; i--)
            Destroy(gridParent.GetChild(i).gameObject);

        int sceneCount = SceneManager.sceneCountInBuildSettings;

        for (int buildIndex = firstPlayableBuildIndex; buildIndex < sceneCount; buildIndex++)
        {
            Button btn = Instantiate(levelButtonPrefab, gridParent);

            int capturedIndex = buildIndex; // important: capture for the lambda

            bool unlocked = LevelProgress.IsUnlocked(capturedIndex);
            btn.interactable = unlocked;

            // Label the button (supports TextMeshPro OR legacy Text)
            if (unlocked)
                SetButtonLabel(btn, $"{capturedIndex - firstPlayableBuildIndex + 1}");
            else
                SetButtonLabel(btn, "");

            // Optional: change sprite for locked/unlocked
            Image img = btn.GetComponent<Image>();
            if (img != null)
            {
                if (unlocked && unlockedSprite != null) img.sprite = unlockedSprite;
                if (!unlocked && lockedSprite != null) img.sprite = lockedSprite;
            }

            btn.onClick.AddListener(() => LoadLevel(capturedIndex));
        }
    }

    private void SetButtonLabel(Button btn, string label)
    {
        // Try TMP first
        TMP_Text tmp = btn.GetComponentInChildren<TMP_Text>();
        if (tmp != null)
        {
            tmp.text = label;
            return;
        }

        // Fallback to legacy UI Text
        Text legacy = btn.GetComponentInChildren<Text>();
        if (legacy != null)
        {
            legacy.text = label;
        }
    }

    public void LoadLevel(int buildIndex)
    {
        if (!LevelProgress.IsUnlocked(buildIndex)) return;
        SceneManager.LoadScene(buildIndex);
    }

    // Optional: hook to a Reset button for testing
    public void ResetProgress()
    {
        LevelProgress.ResetProgress();
        BuildButtons();
    }
}