using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private SpinMovement spinMovement;
    [SerializeField] private Transform muzzle;
    [SerializeField] private GameObject bulletPrefab;

    [Header("Bullet")]
    [SerializeField] private float bulletSpeed = 12f;
    [SerializeField] private float shootCooldown = 0.15f;

    private float nextShootTime = 0f;

    void Reset()
    {
        spinMovement = GetComponent<SpinMovement>();
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Fire();
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == UnityEngine.TouchPhase.Began)
        {
            Fire();
        }
    }

    private void Fire()
    {
        if (Time.time < nextShootTime) return;
        nextShootTime = Time.time + shootCooldown;

        // Spawn bullet out of the front (muzzle)
        GameObject bullet = Instantiate(bulletPrefab, muzzle.position, muzzle.rotation);

        // Push bullet forward (2D forward = up)
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = muzzle.up * bulletSpeed;

        // Recoil / buck as a result of firing
        if (spinMovement != null)
            spinMovement.BuckNow();
    }
}