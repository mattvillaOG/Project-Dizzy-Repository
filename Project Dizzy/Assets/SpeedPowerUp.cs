using UnityEngine;

public class SpeedPowerUp : MonoBehaviour
{
    [SerializeField] private float health = 50f;
    [SerializeField] private string playerTag = "Player";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag(playerTag)) return;

        Debug.Log("Trigger entered with: " + collision.name);

        SpinMovement player = collision.GetComponentInParent<SpinMovement>();
        if (player == null)
        {
            Debug.LogWarning("SpinMovement not found on Player or its parents!");
            return;
        }

        player.GainHealth(health);

        // Only keep this if you REALLY mean to call EnemyDefeated on pickup
        GameManager gm = FindFirstObjectByType<GameManager>();
        if (gm != null) gm.EnemyDefeated();

        Debug.Log("Power-up applied!");
        Destroy(gameObject);
    }
}