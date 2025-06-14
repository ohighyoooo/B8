using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollision : MonoBehaviour
{
    [SerializeField]
    Collider thisCollider;

    [SerializeField]
    Collider[] colliderToIgnore;
    void Start()
    {
        foreach (Collider othercollider in colliderToIgnore)
        {
            Physics.IgnoreCollision(thisCollider, othercollider, true);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
