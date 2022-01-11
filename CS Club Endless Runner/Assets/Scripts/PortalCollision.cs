using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PortalCollision : MonoBehaviour
{
    [SerializeField]
    LayerMask portalLayer;

    [SerializeField]
    float slowDownAmount, slowDownSpeed, speedUpSpeed;

    bool slowDown, reachedMinTimeScale;

    public static event Action OnPortalCollision;

    void Update()
    {
        if (CollidingWithPortal())
        {
            HitPortal();
        }

        if (slowDown)
        {
            if (reachedMinTimeScale)
            {
                Time.timeScale = Mathf.Min(Time.timeScale + speedUpSpeed * Time.unscaledDeltaTime, 1);
                if (Time.timeScale == 1)
                {
                    Time.fixedDeltaTime = Time.timeScale * 0.02f;
                    slowDown = false;
                    reachedMinTimeScale = false;
                }
            }
            else
            {
                Time.timeScale -= slowDownSpeed * Time.unscaledDeltaTime;

                if (Time.timeScale < slowDownAmount)
                {
                    reachedMinTimeScale = true;
                }
            }

            Time.fixedDeltaTime = Time.timeScale * 0.02f;
        }
    }

    bool CollidingWithPortal()
    {
        return Physics2D.Raycast(transform.position, Vector2.right, 0.5f, portalLayer);
    }

    void HitPortal()
    {
        slowDown = true;
        OnPortalCollision?.Invoke();
    }
}
