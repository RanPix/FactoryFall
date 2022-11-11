using System.Collections.Generic;
using UnityEngine;

public class WeaponsLink : MonoBehaviour
{
    public static WeaponsLink instance { get; private set; }
    public List<Weapon> weapons = new List<Weapon>();
    private void Awake()
    {
        instance = this;
    }

}
