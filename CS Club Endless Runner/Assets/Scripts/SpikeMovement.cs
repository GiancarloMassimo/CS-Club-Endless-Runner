using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeMovement : MonoBehaviour
{
    [SerializeField]
    float lifetime;

    Rigidbody2D rb;
 
    SpikeSpawner spawner;

    void OnEnable()
    {
        PortalCollision.OnPortalCollision += DeleteAfterPortalHit;
    }

    private void OnDisable()
    {
        PortalCollision.OnPortalCollision -= DeleteAfterPortalHit;
    }

    void Start()
    {
        spawner = SpikeSpawner.Instance;
        Destroy(gameObject, lifetime);
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.velocity = Vector2.right * spawner.DirectionMultiplier * spawner.SpikeSpeed;
    }

    void DeleteAfterPortalHit()
    {
        Destroy(gameObject);
    }
}
