using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Ghost : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private Transform target;

    [Header("Homing")]
    [SerializeField] private float moveSpeed = 3.5f;

    [Header("Bobbing")]
    public float bobSpeed = 4f;          // speed of bobbing (cycles per second-ish)
    public double bobDistance = 0.25;    // distance of bobbing (units)

    private Rigidbody2D rb;
    private float bobTimeOffset;         // staggers each ghost so they don't sync

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        bobTimeOffset = Random.Range(0f, 1000f);
    }

    private void Start()
    {
        if (target == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag(playerTag);
            if (playerObj != null) target = playerObj.transform;
        }
    }

    private void FixedUpdate()
    {
        if (target == null) return;

        Vector2 currentPos = rb.position;
        Vector2 targetPos = target.position;

        Vector2 toTarget = targetPos - currentPos;
        Vector2 dir = toTarget.sqrMagnitude > 0.0001f ? toTarget.normalized : Vector2.zero;

        // Perpendicular to direction (2D "sideways" vector)
        Vector2 perp = new Vector2(-dir.y, dir.x);

        // Bobbing wave (-1 to 1)
        float wave = Mathf.Sin((Time.time + bobTimeOffset) * bobSpeed);

        // Convert double distance -> float for Unity math
        float bob = (float)bobDistance * wave;

        // Final movement: forward + sideways bob
        Vector2 velocity = (dir * moveSpeed) + (perp * bob);

        // Move using MovePosition (stable with physics)
        rb.MovePosition(currentPos + velocity * Time.fixedDeltaTime);
    }
}