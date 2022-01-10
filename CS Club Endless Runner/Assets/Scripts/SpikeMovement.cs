using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeMovement : MonoBehaviour
{
    [SerializeField]
    float lifetime;

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
    }

    void Update()
    {
        transform.Translate(Vector2.right * spawner.DirectionMultiplier * spawner.SpikeSpeed * Time.deltaTime);
    }

    void DeleteAfterPortalHit()
    {
        Destroy(gameObject);
    }
}
