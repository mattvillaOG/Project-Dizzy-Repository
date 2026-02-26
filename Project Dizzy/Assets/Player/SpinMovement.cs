using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpinMovement : MonoBehaviour
{
    [Header("Spin")]
    [SerializeField] public float rotationSpeed = 300f;

    [Header("Buck Back")]
    [SerializeField] private float buckBackDistance = 1f;
    [SerializeField] private float buckDuration = 0.08f;
    [SerializeField] private float buckCooldown = 0.25f;

    [Header("Screen Bounds")]
    [SerializeField] private float padding = 0.25f;

    [Header("Screen Shake")]
    [SerializeField] private float shakeDuration = 0.08f;
    [SerializeField] private float shakeStrength = 0.08f;

    private Camera mainCam;
    private Vector3 camBasePos;

    private float nextBuckTime = 0f;
    private bool isBucking = false;

    void Awake()
    {
        mainCam = Camera.main;
        if (mainCam != null) camBasePos = mainCam.transform.position;
    }

    void Update()
    {
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
        ClampToCamera();

        if (rotationSpeed <= 0f) { SceneManager.LoadScene("Game Over"); }//Checks to see if the player is still moving
    }

    // Call this from your shooting input
    public bool BuckNow()
    {
        if (Time.time < nextBuckTime) return false;
        if (isBucking) return false;

        nextBuckTime = Time.time + buckCooldown;

        StartCoroutine(BuckRoutine());
        if (mainCam != null)
            StartCoroutine(ShakeRoutine());

        return true;
    }

    private IEnumerator BuckRoutine()
    {
        isBucking = true;

        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + (-transform.up * buckBackDistance);

        float t = 0f;
        float duration = Mathf.Max(0.0001f, buckDuration);

        while (t < duration)
        {
            t += Time.deltaTime;
            float u = t / duration;
            float eased = u * u * (3f - 2f * u); // smoothstep

            transform.position = Vector3.LerpUnclamped(startPos, endPos, eased);
            ClampToCamera();

            yield return null;
        }

        transform.position = endPos;
        ClampToCamera();

        isBucking = false;
    }

    private IEnumerator ShakeRoutine()
    {
        float t = 0f;
        float duration = Mathf.Max(0.0001f, shakeDuration);

        camBasePos = mainCam.transform.position;

        while (t < duration)
        {
            t += Time.deltaTime;
            Vector2 offset = Random.insideUnitCircle * shakeStrength;
            mainCam.transform.position = camBasePos + new Vector3(offset.x, offset.y, 0f);
            yield return null;
        }

        mainCam.transform.position = camBasePos;
    }

    private void ClampToCamera()
    {
        if (mainCam == null || !mainCam.orthographic) return;

        float halfHeight = mainCam.orthographicSize;
        float halfWidth = halfHeight * mainCam.aspect;

        Vector3 camPos = mainCam.transform.position;
        Vector3 pos = transform.position;

        pos.x = Mathf.Clamp(pos.x, camPos.x - halfWidth + padding, camPos.x + halfWidth - padding);
        pos.y = Mathf.Clamp(pos.y, camPos.y - halfHeight + padding, camPos.y + halfHeight - padding);

        transform.position = pos;
    }

    public void TakeDamage(float amount)
    {
        rotationSpeed -= amount;

        if (rotationSpeed < 0f)
            rotationSpeed = 0f;

        Debug.Log("Player took damage! New rotationSpeed: " + rotationSpeed);
    }

    public void GainHealth(float amount)
    {
        rotationSpeed += amount;

        if (rotationSpeed < 0f)
            rotationSpeed = 0f;

        Debug.Log("Player gained health! New rotationSpeed: " + rotationSpeed);
    }
}