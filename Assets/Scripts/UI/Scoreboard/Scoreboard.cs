using Mirror;
using UnityEngine;
using TMPro;
using System;
using System.Collections;

public class Scoreboard : NetworkBehaviour
{
    private bool hasTimer;
    private bool teamsMatch;
    private bool scoreBased;

    private bool scoreNotificationEnabled;
    public Action<string, Team> OnPlayerScored;

    [SerializeField] private TMP_Text goalOrTimerText;

    [SerializeField] private TMP_Text playerScoreText;

    [SerializeField] private TMP_Text blueTeamScoreText;
    [SerializeField] private TMP_Text redTeamScoreText;

    private int goal;
    private float matchTime;

    [SerializeField] private GameObject winnersPanel;
    [SerializeField] private TMP_Text winnerText;
    [SerializeField] private TMP_Text best3FromLeaderBoard;

    [SyncVar (hook = nameof(UpdateBlueTeamScore))] private int blueTeamScore;
    [SyncVar (hook = nameof(UpdateRedTeamScore))] private int redTeamScore;

    public override void OnStartClient()
    {
        base.OnStartClient();
        
        teamsMatch = GameManager.instance.matchSettings.teamsMatch;
        hasTimer = GameManager.instance.matchSettings.hasTimer;
        scoreBased = GameManager.instance.matchSettings.gm == Gamemode.BTR ? true : false;

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
            GameManager.instance.EndGame();
            RpcBroadcastWonTeam(Team.Blue);
        }
        else if (redTeamScore >= goal)
        {
            GameManager.instance.EndGame();
            RpcBroadcastWonTeam(Team.Red);
        }
    }

    [ClientRpc]
    private void RpcBroadcastWonTeam(Team team)
    {
        winnersPanel.SetActive(true);

        switch (team)
        {
            case Team.Blue:
                winnerText.text = "Blue Team Won!";
                winnerText.color = TeamToColor.GetTeamColor(team);
                break;

            case Team.Red:
                winnerText.text = "Red Team Won!";
                winnerText.color = TeamToColor.GetTeamColor(team);
                break;

            case Team.None: 
                // has to take the best player from a list
                break;
        }

        
        
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
