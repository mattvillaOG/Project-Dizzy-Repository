using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Tuning")]
    [SerializeField] private float speed = 12f;
    [SerializeField] private float maxDistance = 8f;

    private Vector3 startPosition;
    private bool isActive;

    // Called by the pool/shooter whenever we "fire" this bullet
    public void Fire(Vector3 position, Quaternion rotation)
    {
        transform.SetPositionAndRotation(position, rotation);

        startPosition = position;
        isActive = true;

        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (!isActive) return;

        // Move forward based on current rotation
        transform.position += transform.up * speed * Time.deltaTime;

        // Deactivate after traveling far enough
        float distanceTraveled = Vector3.Distance(startPosition, transform.position);
        if (distanceTraveled >= maxDistance)
        {
            Deactivate();
        }
    }

    public void Deactivate()
    {
        isActive = false;
        gameObject.SetActive(false);
    }
}