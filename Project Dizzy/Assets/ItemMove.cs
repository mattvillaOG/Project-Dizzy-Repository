using UnityEngine;

public class ItemMove : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float height = 0.5f;   // How high it moves
    [SerializeField] private float speed = 2f;      // How fast it moves

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * speed) * height;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}
