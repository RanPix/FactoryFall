using UnityEngine;

[System.Serializable]
public class MatchSettings
{
    public Gamemode gm { get; private set; }
    public bool teamsMatch { get; private set; } = true; // WILL BE REMOVED WITH DM MODE SUPPORT

    public bool hasTimer { get; private set; } = false; 
    public float matchTime { get; private set; }
    public int winningGoal { get; private set; } = 2;


    [Min(0.1f)] public float respawnTime; // If 0 is needed, check MovementMachine SpeedReset timer

    [field: SerializeField] public float dmgMultiplier { get; private set; } = 1;

    private bool firstSetup = true;

    public void Setup()
    {
        if (!firstSetup)
            return;

        firstSetup = false;
    }
}
