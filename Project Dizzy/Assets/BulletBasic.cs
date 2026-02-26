using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float maxDistance = 8f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float distanceTraveled = Vector3.Distance(startPosition, transform.position);

        if (distanceTraveled >= maxDistance)
        {
            Destroy(gameObject);
        }
    }
}