using System.Collections;
using System.Collections.Generic;
using Mirror;
using Player;
using UnityEngine;

public class SpawnWeapon : NetworkBehaviour
{
    public GameObject weapon;
    void Start()
    {
        if (isLocalPlayer)
        {
            weapon = GameObject.Instantiate(weapon);
            weapon.GetComponent<UpdateWeaponPosition>().localCameraHolder = gameObject.GetComponent<GamePlayer>().cameraHolder;
            weapon.GetComponent<UpdateWeaponPosition>()._isLocalPlayer = true;
            weapon.GetComponent<Weapon>()._isLocalPlayer = true;
            NetworkServer.Spawn(weapon);
        }
    }

}
