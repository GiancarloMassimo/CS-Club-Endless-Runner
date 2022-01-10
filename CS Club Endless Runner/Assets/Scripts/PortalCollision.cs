using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PortalCollision : MonoBehaviour
{
    [SerializeField]
    LayerMask portalLayer;

    public static event Action OnPortalCollision;

    void Update()
    {
        if (CollidingWithPortal())
        {
            HitPortal();
        }
    }

    bool CollidingWithPortal()
    {
        return Physics2D.Raycast(transform.position, Vector2.right, 0.5f, portalLayer);
    }

    void HitPortal()
    {
        OnPortalCollision?.Invoke();
    }
}
