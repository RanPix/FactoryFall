using UnityEngine;

public class MatchSettingsTransfer : MonoBehaviour
{
    public static MatchSettingsTransfer instance;

    public Gamemode gm = Gamemode.DM;

    public bool hasTimer;
    [Min(30)] public int matchTime = 300;
    public int winningGoal = 50;


    [Min(0.1f)] public float respawnTime; // If 0 is needed, check MovementMachine SpeedReset timer

    public float dmgMultiplier { get; private set; } = 1;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void SetSettings(Gamemode gm, bool timer, int matchTime, int goal)
    {
        this.gm = gm;

        hasTimer = timer;

        this.matchTime = matchTime;
        winningGoal = goal;
    }

    public void ResetSettings()
    {
        gm = Gamemode.DM;

        hasTimer = false;

        matchTime = 300;
        winningGoal = 50;
    }

    public void SetGameMode(int gm)
    {
        this.gm = (Gamemode)gm;
    }

    public void SetTimerMode()
    {
        hasTimer = !hasTimer;
    }

    public void SetMatchTime(string time)
    {
        matchTime = int.Parse(time);
    }
    

    public void SetWinningGoal(string goal)
    {
        winningGoal = int.Parse(goal);
    }
}
