using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScript : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private Animator animator;

    public void PlayMusic()
    {
        if (!musicSource.isPlaying)
            musicSource.Play();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Animator animator = GetComponent<Animator>();
        animator.SetBool("OpenStory", false);
    }

    void Update()
    {

    }

    public void Trasition() { animator.SetTrigger("PlayTitleScreenB"); Debug.Log("close title and open intro."); }
    public void BeginGame() { SceneManager.LoadScene("LevelSelect"); }
}
