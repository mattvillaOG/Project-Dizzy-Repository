using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI ghostCountText;
    public int enemyCount = 10;

    [Header("UI")]
    [SerializeField] private GameObject winMenu;
    [SerializeField] private GameObject loseMenu;


    void Start()
    {
        // Count all enemies at the start of the scene
        //enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        Debug.Log("Enemies Remaining: " + enemyCount);
        UpdateUI();
        Time.timeScale = 1f; // UN-freeze the game
        AudioMachine.Instance.PlayMusic("Main Theme");
    }

    public void EnemyDefeated()
    {
        enemyCount--;

        UpdateUI();
        Debug.Log("Enemies Remaining: " + enemyCount);

        if (enemyCount <= 0)
        {
            WinFunction();
        }
    }

    void UpdateUI()
    {
        ghostCountText.text = "Ghosts: " + enemyCount;
    }

    void WinFunction()
    {
        Debug.Log("YOU WIN!");
        // Add win logic here (UI, load next scene, etc.)
        StartCoroutine(WinSequence());
        int current = SceneManager.GetActiveScene().buildIndex;
        LevelProgress.UnlockNextFrom(current);
        AudioMachine.Instance.PlaySFX("Win");
        AudioMachine.Instance.StopMusic();
    }

    public void LoseFunction()
    {
        Time.timeScale = 0f; // freeze the game
        //Show Lose UI
        if (loseMenu != null)
        {
            loseMenu.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Lose Menu not assigned in GameManager.");
        }
    }

    private IEnumerator WinSequence() {

        //Disable all Enemy Spawners
        GameObject[] spawners = GameObject.FindGameObjectsWithTag("Enemy Spawner");

        foreach (GameObject spawner in spawners)
        {
            spawner.SetActive(false);
        }

        //Wait ONE frame
        yield return null;

        //Disable all active Enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            enemy.SetActive(false);
        }

        //Show Win UI
        if (winMenu != null)
        {
            winMenu.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Win Menu not assigned in GameManager.");
        }

    }

    public void NextButton() { SceneManager.LoadScene("LevelSelect"); } // load the level select menu

    public void RestartLevel() { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); } // restart the level
}