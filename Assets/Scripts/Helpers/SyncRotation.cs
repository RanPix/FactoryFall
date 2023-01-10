using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class SyncRotation : NetworkBehaviour
{
    public Transform reference;
    public Transform target;

    void LateUpdate()
    {
        if(isLocalPlayer)
            target.rotation = reference.rotation;
    }
}
