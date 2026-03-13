using UnityEngine;

public class RainbowCircle : MonoBehaviour
{
    [Header("Player Detection")]
    [SerializeField] private string playerTag = "Player";
    private Transform player;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;

    [Header("Circle Motion")]
    [SerializeField] private float radius = 1.5f;
    [SerializeField] private float rotationSpeed = 4f;

    private float angle;

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag(playerTag);

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("Player not found with tag: " + playerTag);
        }
    }

    void Update()
    {
        if (player == null) return;

        // Move toward player
        Vector3 direction = (player.position - transform.position).normalized;
        Vector3 move = direction * moveSpeed * Time.deltaTime;

        // Circular movement
        angle += rotationSpeed * Time.deltaTime;
        float x = Mathf.Cos(angle) * radius;
        float y = Mathf.Sin(angle) * radius;
        Vector3 circle = new Vector3(x, y, 0);

        // Combine motions
        transform.position += move + circle * Time.deltaTime;
    }
}
