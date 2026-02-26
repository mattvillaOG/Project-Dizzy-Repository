using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI ghostCountText;

    private int enemyCount;

    void Start()
    {
        // Count all enemies at the start of the scene
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        Debug.Log("Enemies Remaining: " + enemyCount);
        UpdateUI();
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
    }
}