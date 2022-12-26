using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Mirror;
using UnityEngine;

public class UpdateWeaponPosition : NetworkBehaviour
{
    protected Transform weaponPosition;
    [SyncVar]public GameObject localCameraHolder;
    [field: SerializeField] public bool _isServer { get; set; } = false;
    public override void OnStartServer()
    {
            weaponPosition = localCameraHolder.transform.GetChild(0).GetChild(0).GetChild(0);
    }
    private void LateUpdate()
    {
        if(!_isServer)
            return;
        transform.position = weaponPosition.position;
        transform.forward = localCameraHolder.transform.GetChild(0).forward;
    }


}
