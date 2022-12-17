using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Mirror;
using UnityEngine;

public class UpdateWeaponPosition : NetworkBehaviour
{
    protected Transform weaponPosition;
    public GameObject localCameraHolder;
    [HideInInspector] public bool _isLocalPlayer { get; set; } = false;

    private void Start()
    {
        weaponPosition = localCameraHolder.transform.GetChild(0).GetChild(0).GetChild(0);
    }
    private void LateUpdate()
    {
        if (_isLocalPlayer)
        {
            transform.position = weaponPosition.position;
            transform.forward = localCameraHolder.transform.GetChild(0).forward;
        }
    }


}
