using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    float xPositionFromPlayer = 4;

    [SerializeField]
    float angle;

    [SerializeField]
    float angleIncrement;

    [SerializeField]
    float maxAngle, maxAngleAfterAcceleration;

    [SerializeField]
    float transitionSpeed;

    [SerializeField]
    float rotationSpeed;

    [SerializeField]
    float extraRotationRate, extraRotationRateAcceleration;

    [SerializeField]
    float cameraHorizontalOffsetToAngleProportion, cameraVerticalOffsetToAngleProportion;

    [SerializeField]
    GameObject atmosphereEffects;

    int directionMultiplier = 1;
    float desiredAngle;
    float initialExtraRotationRate;
    Vector3 desiredPos;
    bool shaking;

    void OnEnable()
    {
        PortalCollision.OnPortalCollision += ChangePosition;
    }

    void OnDisable()
    {
        PortalCollision.OnPortalCollision -= ChangePosition;
    }

    void Start()
    {
        initialExtraRotationRate = extraRotationRate;
        desiredPos = new Vector3(directionMultiplier * xPositionFromPlayer, transform.position.y, transform.position.z);
    }

    void LateUpdate()
    {
        if (!shaking)
        {
            transform.position = Vector3.Lerp(
                transform.position,
                desiredPos,
                transitionSpeed * Time.deltaTime
                );
        }

        extraRotationRate += extraRotationRateAcceleration * Time.deltaTime;
        desiredAngle += extraRotationRate * Time.deltaTime;

        desiredAngle = Mathf.Min(desiredAngle, maxAngleAfterAcceleration);

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, desiredAngle * directionMultiplier), rotationSpeed * Time.deltaTime);
        
        if (desiredAngle < maxAngleAfterAcceleration)
        {
            desiredPos += transform.up * desiredAngle / 360f * cameraVerticalOffsetToAngleProportion;
            desiredPos += transform.right * directionMultiplier * -1 * desiredAngle / 360f * cameraHorizontalOffsetToAngleProportion;
        }
    }

    void ChangePosition()
    {
        directionMultiplier *= -1;
        if (angle < maxAngle)
        {
            angle += angleIncrement;
        }
        desiredPos = new Vector3(directionMultiplier * xPositionFromPlayer, 0, transform.position.z);
        desiredAngle = angle;
        extraRotationRate = initialExtraRotationRate;
        SwitchEffects();
    }

    void SwitchEffects()
    {
        atmosphereEffects.transform.localScale = new Vector2(directionMultiplier, 1);
    }

    public void StartShake(float magnitude, float duration)
    {
        shaking = true;
        StartCoroutine(Shake(magnitude, duration, magnitude / duration));
    }

    IEnumerator Shake (float magnitude, float duration, float fadeTime)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.position += new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            magnitude = Mathf.MoveTowards(magnitude, 0f, fadeTime * Time.deltaTime);

            if (Time.timeScale == 0)
            {
                yield break;
            }

            yield return null;
        }

        shaking = false;
    }
}
