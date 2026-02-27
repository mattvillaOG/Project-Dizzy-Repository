using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private SpinMovement spinMovement;
    [SerializeField] private Transform muzzle;

    [Header("Bullet Selection")]
    [SerializeField] private char currentBulletID = 'A';

    [SerializeField] private Bullet bulletA;
    [SerializeField] private Bullet bulletB;
    [SerializeField] private Bullet bulletC;

    private Bullet currentBulletPrefab;

    [Header("Pooling")]
    [SerializeField] private int poolSize = 3;
    private Bullet[] pool;

    private float nextShootTime = 0f;

    private void Awake()
    {
        SelectBulletPrefab();
        BuildPool();
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

        if (Touchscreen.current != null &&
            Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            Fire();
        }
    }

    private void Fire()
    {
        if (Time.time < nextShootTime) return;
        //nextShootTime = Time.time + shootCooldown;
        nextShootTime = Time.time + spinMovement.buckCooldown;

        Bullet bullet = GetAvailableBullet();
        if (bullet == null) return;

        bullet.Fire(muzzle.position, muzzle.rotation);

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

    // ------------------------
    // Bullet Switching Logic
    // ------------------------

    private void SelectBulletPrefab()
    {
        switch (currentBulletID)
        {
            case 'B':
                currentBulletPrefab = bulletB;
                break;
            case 'C':
                currentBulletPrefab = bulletC;
                break;
            default:
                currentBulletPrefab = bulletA;
                break;
        }
    }

    private void BuildPool()
    {
        pool = new Bullet[poolSize];

        for (int i = 0; i < poolSize; i++)
        {
            Bullet b = Instantiate(currentBulletPrefab);
            b.gameObject.SetActive(false);
            pool[i] = b;
        }
    }

    public void SwitchBulletType(char newID)
    {
        currentBulletID = newID;

        // Destroy old pooled bullets
        if (pool != null)
        {
            for (int i = 0; i < pool.Length; i++)
            {
                if (pool[i] != null)
                    Destroy(pool[i].gameObject);
            }
        }

        SelectBulletPrefab();
        BuildPool();
    }

    public void SetBulletTuning(float newSpeed, float newMaxDistance)
    {
        // Apply to bullets already created in the pool
        if (pool != null)
        {
            for (int i = 0; i < pool.Length; i++)
            {
                if (pool[i] != null)
                    pool[i].SetTuning(newSpeed, newMaxDistance);
            }
        }
    }
}