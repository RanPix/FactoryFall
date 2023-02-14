using System.Xml.Serialization;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[System.Serializable]
public class MatchSettings
{
    public MatchSettingsStruct matchSettingsStruct;

    public Gamemode gm { get; private set; } = Gamemode.BTR;
    public bool teamsMatch { get; private set; }
    public bool scoreBased { get; private set; }

    public bool hasTimer { get; private set; }
    public int matchTime { get; private set; }
    public int winningGoal { get; private set; }


    [Min(0.1f)] public float respawnTime; // If 0 is needed, check MovementMachine SpeedReset timer

    [field: SerializeField] public float dmgMultiplier { get; private set; } = 1;

    public bool firstSetup { get; private set; } = true;

    public void Setup(Gamemode gm, bool timer, int matchTime, int goal)
    {
        if (!firstSetup)
            return;

        //Debug.Log("yes2");
        this.gm = gm;

        hasTimer = timer;
        this.matchTime = matchTime;
        winningGoal = goal;

        teamsMatch = gm == Gamemode.TDM || gm == Gamemode.BTR;
        scoreBased = gm == Gamemode.BTR ? true : false;

        matchSettingsStruct = new MatchSettingsStruct(gm, teamsMatch, scoreBased, hasTimer, matchTime, goal, respawnTime, dmgMultiplier);

        firstSetup = false;

        Debug.Log("INVOKE1");
        GameManager.instance.OnMatchSettingsSettedup.Invoke(teamsMatch);
    }

    public void SetMatchSettings(MatchSettingsStruct settings)
    {
        if (!firstSetup)
            return;

        //Debug.Log("yes3");
        matchSettingsStruct = settings;

        gm = settings.gm;
        teamsMatch = settings.teamsMatch;
        scoreBased = settings.scoreBased;

        hasTimer = settings.hasTimer;
        matchTime = settings.matchTime;
        winningGoal = settings.winningGoal;

        firstSetup = false;

        Debug.Log("INVOKE2");
        GameManager.instance.OnMatchSettingsSettedup.Invoke(teamsMatch);
    }
}

public struct MatchSettingsStruct
{
    public Gamemode gm;
    public bool teamsMatch;
    public bool scoreBased;

    public bool hasTimer;
    public int matchTime;
    public int winningGoal;


    [Min(0.1f)] public float respawnTime; // If 0 is needed, check MovementMachine SpeedReset timer

    [field: SerializeField] public float dmgMultiplier;

    public MatchSettingsStruct(Gamemode gm, bool teamsMatch, bool scoreBased, bool hasTimer, int matchTime, int goal, float respawnTime, float dmgMultiplier)
    {
        this.gm = gm;
        this.teamsMatch = teamsMatch;
        this.scoreBased = scoreBased;

        this.hasTimer = hasTimer;
        this.matchTime = matchTime;
        winningGoal = goal;

        this.respawnTime = respawnTime;
        this.dmgMultiplier = dmgMultiplier;
    }
}
