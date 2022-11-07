using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Weaponchik : MonoBehaviour
{
    protected abstract WeaponScriptableObject ScriptableObject { get; }
    protected abstract States _states { get; }
    protected abstract _GunType _gunType { get; }


    public abstract void Shoot();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
