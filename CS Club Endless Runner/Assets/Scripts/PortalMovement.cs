using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalMovement : SpikeMovement
{
    [SerializeField]
    GameObject destroyEffect;

    private void OnDestroy()
    {
        Instantiate(destroyEffect, transform.position, Quaternion.Euler(0, 0, 90));
    }
}
