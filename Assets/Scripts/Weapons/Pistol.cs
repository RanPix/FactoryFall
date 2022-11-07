using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum States
{
    Active,
    Destroied
}
public enum _GunType
{
    Auto,
    Semi
}
public class Pistol : Weaponchik
{
    [SerializeField] private WeaponScriptableObject weaponScriptableObject;
    protected override WeaponScriptableObject ScriptableObject => weaponScriptableObject;

    [SerializeField] private States state;
    protected override States _states => state;

    [SerializeField] private _GunType gunType;
    protected override _GunType _gunType => gunType;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Shoot()
    {
        
    }
}
