using UnityEngine;
using System;

public class BasicPowerUp : MonoBehaviour
{
    [Header("Player Detection")]
    [SerializeField] private string playerTag = "Player";

    [Header("Spin Movement Buffs")]
    [SerializeField] private float rotationSpeedIncrease = 25f;
    [SerializeField] private float newBuckBackDistance = 1.5f;
    [SerializeField] private float newBuckDuration = 1.5f;
    [SerializeField] private float newBuckCooldown = 0.15f;

    [Header("Bullet Switch")]
    [SerializeField] private char bulletType = 'B'; // 'A', 'B', or 'C'
    //[SerializeField] private float speed_set;
    //[SerializeField] private float maxDistance_set;

    public Action OnDisabled;

    private void OnDisable()
    {
        OnDisabled?.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        SpinMovement spin = other.GetComponentInParent<SpinMovement>();
        PlayerShooting shooting = other.GetComponentInParent<PlayerShooting>();
        

        if (spin != null)
        {
            spin.rotationSpeed += rotationSpeedIncrease;
            spin.buckBackDistance = newBuckBackDistance;
            spin.buckDuration = newBuckDuration;
            spin.buckCooldown = newBuckCooldown;

            Debug.Log("SpinMovement stats updated.");
        }
        else
        {
            Debug.LogWarning("SpinMovement not found!");
        }

        if (shooting != null)
        {
            shooting.SwitchBulletType(bulletType);
            //shooting.SetBulletTuning(speed_set, maxDistance_set);
            Debug.Log($"Bullet switched to '{bulletType}' and tuning applied.");
        }
        else
        {
            Debug.LogWarning("PlayerShooting not found!");
        }

        Destroy(gameObject);
    }
}