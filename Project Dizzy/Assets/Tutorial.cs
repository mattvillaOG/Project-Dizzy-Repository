using UnityEngine;

public class Tutorial : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = 0f; // Freeze the game
    }

    public void StartButton() { Time.timeScale = 1f; Destroy(gameObject); } // UN-freeze the game + disable start menu 
}
