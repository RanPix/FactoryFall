using UnityEngine;

[System.Serializable]
public class MatchSettings
{
    [Min(0.1f)] public float respawnTime; // If 0 is needed, check MovementMachine SpeedReset timer

    public float dmgMultiplier = 1;
}
