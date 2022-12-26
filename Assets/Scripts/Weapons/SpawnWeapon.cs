using System.Collections;
using System.Collections.Generic;
using Mirror;
using Player;
using UnityEngine;

public class SpawnWeapon : NetworkBehaviour
{
    public GameObject weapon;
    public override void OnStartServer()
    {
        if(!isServer)
            return;
        weapon = NetworkManager.Instantiate(weapon);
        GetComponent<WeaponKeyCodes>().enabled = true;
        GetComponent<WeaponKeyCodes>().weapon = weapon.GetComponent<Weapon>();
        weapon.GetComponent<Weapon>().player = gameObject;
        weapon.GetComponent<UpdateWeaponPosition>().localCameraHolder = gameObject.GetComponent<GamePlayer>().cameraHolder;
        NetworkServer.Spawn(weapon);
        weapon.GetComponent<Weapon>()._isLocalPlayer = true;
        weapon.GetComponent<UpdateWeaponPosition>()._isServer = true;

    }

}
