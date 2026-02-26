using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private SpinMovement spinMovement;
    [SerializeField] private Transform muzzle;
    [SerializeField] private Bullet bulletPrefab;

    [Header("Bullet")]
    [SerializeField] private float bulletSpeed = 12f;
    [SerializeField] private float shootCooldown = 0.15f;

    [Header("Pooling")]
    [SerializeField] private int poolSize = 3;
    private Bullet[] pool;

    private float nextShootTime = 0f;

    private void Awake()
    {
        // Create 3 bullets up front
        pool = new Bullet[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            Bullet b = Instantiate(bulletPrefab);
            b.gameObject.SetActive(false);
            pool[i] = b;
        }
    }

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


        Bullet bullet = GetAvailableBullet();
        if (bullet == null)
        {
            // All 3 are currently active -> no shot (your design choice)
            return;
        }

        bullet.Fire(muzzle.position, muzzle.rotation);

        // Spawn bullet out of the front (muzzle)
        //GameObject bullet = Instantiate(bulletPrefab, muzzle.position, muzzle.rotation);

        // Push bullet forward (2D forward = up)
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = muzzle.up * bulletSpeed;

        // Recoil / buck as a result of firing
        if (spinMovement != null)
            spinMovement.BuckNow();
    }

    private Bullet GetAvailableBullet()
    {
        for (int i = 0; i < pool.Length; i++)
        {
            if (!pool[i].gameObject.activeInHierarchy)
                return pool[i];
        }
        return null;
    }
}