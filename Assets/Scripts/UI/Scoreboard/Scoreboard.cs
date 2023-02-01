using Mirror;
using UnityEngine;
using TMPro;
using System;

public class Scoreboard : NetworkBehaviour
{
    private bool hasTimer;
    private bool teamsMatch;

    private bool scoreNotificationEnabled;
    public Action<string, Team> OnPlayerScored;

    [SerializeField] private TMP_Text goalOrTimerText;

    [SerializeField] private TMP_Text playerScoreText;

    [SerializeField] private TMP_Text blueTeamScoreText;
    [SerializeField] private TMP_Text redTeamScoreText;

    private int goal;
    private float matchTime;


    [SyncVar (hook = nameof(UpdateBlueTeamScore))] private int blueTeamScore;
    [SyncVar (hook = nameof(UpdateRedTeamScore))] private int redTeamScore;

    public override void OnStartClient()
    {
        base.OnStartClient();
        
        teamsMatch = GameManager.instance.matchSettings.teamsMatch;
        hasTimer = GameManager.instance.matchSettings.hasTimer;

        if (hasTimer)
        {
            matchTime = GameManager.instance.matchSettings.matchTime;
            
        }
        else
        {
            goal = GameManager.instance.matchSettings.winningGoal;
            goalOrTimerText.text = goal.ToString();
        }

        UpdateBlueTeamScore(-1, blueTeamScore);
        UpdateRedTeamScore(-1, redTeamScore);
    }

    private void Start()
    {
        
    }

    [Server]
    private void Update()
    {
        if (hasTimer)
        {

            
        }
    }


    [Command]
    public void CmdAddScore(Team team, int amount, string scoredPlayerName)
    {
        if (scoreNotificationEnabled)
            RpcNotificatePlayerScored(scoredPlayerName, team);

        if (!teamsMatch)
            return;

        if (team is Team.Blue)
            blueTeamScore += amount;
        else if (team is Team.Red)
            redTeamScore += amount;

        if (hasTimer)
            return;

        CheckScoreGetsGoal();
    }

    [Server]
    public void AddScore(Team team, int amount, string scoredPlayerName)
    {
        if (scoreNotificationEnabled)
            RpcNotificatePlayerScored(scoredPlayerName, team);

        if (!teamsMatch)
            return;

        if (team is Team.Blue)
            blueTeamScore += amount;
        else if (team is Team.Red)
            redTeamScore += amount;

        if (hasTimer)
            return;

        CheckScoreGetsGoal();
    }

    [ClientRpc]
    private void RpcNotificatePlayerScored(string scoredPlayerName, Team team)
    {
        OnPlayerScored.Invoke(scoredPlayerName, team);
        
    }


    [Server]
    private void CheckScoreGetsGoal()
    {
        if (blueTeamScore >= goal)
        {
            RpcBroadcastWonTeam(Team.Blue);
        }
        else if (redTeamScore >= goal)
        {
            RpcBroadcastWonTeam(Team.Red);
        }
    }

    [ClientRpc]
    private void RpcBroadcastWonTeam(Team team)
    {
        print($"{team} team Won!");
    }


    [Client]
    private void UpdateBlueTeamScore(int oldScore, int newScore)
    {
        blueTeamScoreText.text = newScore.ToString();
    }

    [Client]
    private void UpdateRedTeamScore(int oldScore, int newScore)
    {
        redTeamScoreText.text = newScore.ToString();
    }
}
