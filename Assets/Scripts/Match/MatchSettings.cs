using UnityEngine;

[System.Serializable]
public class MatchSettings
{
    public Gamemode gm { get; private set; } = Gamemode.BTR;
    public bool teamsMatch { get; private set; } = true; // WILL BE REMOVED WITH DM MODE SUPPORT
    public bool scoreBased { get; private set; }

    public bool hasTimer { get; private set; } = false;
    public int matchTime { get; private set; } = 60;
    public int winningGoal { get; private set; } = 30;


    [Min(0.1f)] public float respawnTime; // If 0 is needed, check MovementMachine SpeedReset timer

    [field: SerializeField] public float dmgMultiplier { get; private set; } = 1;

    private bool firstSetup = true;

    public void Setup()
    {
        if (!firstSetup)
            return;

        scoreBased = gm == Gamemode.BTR ? true : false;

        firstSetup = false;
    }
}
