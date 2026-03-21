using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioMachine.Instance.PlayMusic("Select Music");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Trasition() { Debug.Log("close title and open intro."); }
    public void BeginGame() { SceneManager.LoadScene("LevelSelect"); }
}
