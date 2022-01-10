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
    float translationAmount, translationSpeed;

    [SerializeField]
    float extraRotationRate, extraRotationRateAcceleration;

    [SerializeField]
    float cameraHorizontalOffsetToAngleProportion, cameraVerticalOffsetToAngleProportion;

    int directionMultiplier = 1;
    float desiredAngle;
    float initialExtraRotationRate;
    Vector3 desiredPos;


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

    void Update()
    {
        transform.position = Vector3.Lerp(
            transform.position, 
            desiredPos, 
            transitionSpeed * Time.deltaTime
            );

        extraRotationRate += extraRotationRateAcceleration * Time.deltaTime;
        desiredAngle += extraRotationRate * Time.deltaTime;

        desiredAngle = Mathf.Min(desiredAngle, maxAngleAfterAcceleration);

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, desiredAngle * directionMultiplier), rotationSpeed * Time.deltaTime);

        /*if (Mathf.Abs(transform.position.x) < xPositionFromPlayer + translationAmount)
        {
            Debug.Log(desiredPos);
            desiredPos += transform.right * directionMultiplier * translationSpeed * Time.deltaTime;
        }*/
        
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
    }
}
