using UnityEngine;

[CreateAssetMenu(fileName = "HealthPreset", menuName = "ScriptableObject/HealthPreset")]
public class HealthPreset : ScriptableObject
{
    [Header("Health")]

    [Min(1)] public int MaxHealth;


    [Header("Armor")]

    [Min(0)] public int MaxArmor;


    [Space(4)]

    [Min(0)] public int ArmorAdditionForFistKill;

    [Min(0)] public int ArmorAdditionForWeaponKill;


    [Space(2)]

    [Min(0)] public int DamageForArmor; // 0 for disable

    [Min(0)] public int ArmorAdditionForDamage; // Make it less than damage for armor
}
