using UnityEngine;

public class PersistentMusic : MonoBehaviour
{
    private static PersistentMusic instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // prevents duplicate music objects
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}