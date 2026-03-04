using UnityEngine;

public class GhostDamage : MonoBehaviour
{
    [SerializeField] private float damage = 50f;
    [SerializeField] private string playerTag = "Player";

    //public int hits to take = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger entered with: " + collision.name);

        if (collision.CompareTag(playerTag))
        {
            SpinMovement player = collision.GetComponent<SpinMovement>();

            if (player != null)
            {
                player.TakeDamage(damage);
                FindObjectOfType<GameManager>().EnemyDefeated();
                Debug.Log("Damage applied!");
            }
            else
            {
                Debug.Log("SpinMovement not found!");
            }

            //Destroy(gameObject);
            gameObject.SetActive(false);
        }

        if (collision.CompareTag("Projectile"))
        {
            FindObjectOfType<GameManager>().EnemyDefeated();
            //Destroy(gameObject);
            gameObject.SetActive(false);
        }
    }
}