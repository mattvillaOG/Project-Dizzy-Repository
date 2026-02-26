using UnityEngine;
using System;

public class BasicPowerUp : MonoBehaviour
{
    [SerializeField] private float speedIncrease = 25f;
    [SerializeField] private string playerTag = "Player";

    public Action OnDisabled;

    private void OnDisable()
    {
        OnDisabled?.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        SpinMovement player = other.GetComponentInParent<SpinMovement>();

        if (player != null)
        {
            player.rotationSpeed += speedIncrease;
            Debug.Log("Speed increased by " + speedIncrease);
        }
        else
        {
            Debug.LogWarning("SpinMovement not found!");
        }

        Destroy(gameObject);
    }
}