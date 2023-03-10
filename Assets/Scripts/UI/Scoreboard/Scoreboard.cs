using Mirror;
using UnityEngine;
using TMPro;
using System;

public class Scoreboard : NetworkBehaviour
{
    private bool hasTimer;
    private bool teamsMatch;
    private bool scoreBased;

    private bool scoreNotificationEnabled;
    public Action<string, Team> OnPlayerScored;

    [SyncVar (hook = nameof(UpdateGoalOrTimerText))] private string goalOrTimerString;
    [SerializeField] private TMP_Text goalOrTimerText;

    [SerializeField] private TMP_Text playerScoreText;

    [SerializeField] private TMP_Text blueTeamScoreText;
    [SerializeField] private TMP_Text redTeamScoreText;

    private int localScore;

    private int goal;

    private int matchTime;
    [SyncVar] private float matchTimer;
    private bool countDown = true;

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
            matchTimer = matchTime;
        }
        else
        {
            goal = GameManager.instance.matchSettings.winningGoal;
            goalOrTimerString = goal.ToString();
        }

        UpdateBlueTeamScore(-1, blueTeamScore);
        UpdateRedTeamScore(-1, redTeamScore);
    }

    [Server]
    private void Update()
    {
        if (!NetworkManager.singleton.isNetworkActive)
            return;

        if (hasTimer)
        {
            Timer();
        }
    }
    

    [Command]
    public void CmdAddScore(Team team, int amount, string scoredPlayerName)
    {
        if (GameManager.instance.gameEnded)
            return;

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
        if (GameManager.instance.gameEnded) 
            return;

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

    [Client]
    public void ChangeLocalPlayerScore(int amount)
    {
        playerScoreText.text = amount.ToString();
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
            EndGame(Team.Blue);
        }
        else if (redTeamScore >= goal)
        {
            EndGame(Team.Red);
        }
    }

    [Server]
    private void CheckTeamsScore()
    {
        if (blueTeamScore == redTeamScore)
        {
            EndGame(Team.Null);
        }
        else if(blueTeamScore > redTeamScore)
        {
            EndGame(Team.Blue);
        }
        else if (blueTeamScore < redTeamScore)
        {
            EndGame(Team.Red);
        }
    }

    [ClientRpc]
    private void RpcBroadcastWonTeam(Team team)
    {
        winnersPanel.SetActive(true);

        switch (team)
        {
            case Team.Null:
                winnerText.text = "Draw";
                break;

            case Team.Blue:
                winnerText.text = "Blue Team Won";
                winnerText.color = TeamToColor.GetTeamColor(team);
                break;

            case Team.Red:
                winnerText.text = "Red Team Won";
                winnerText.color = TeamToColor.GetTeamColor(team);
                break;

            case Team.None: 
                // has to take the best player from a list
                break;
        }

        
        
    }

    [Server]
    private void EndGame(Team wonTeam)
    {
        GameManager.instance.EndGame();
        RpcBroadcastWonTeam(wonTeam);
    }



    [Client]
    private void UpdateGoalOrTimerText(string oldText, string newText)
    {
        goalOrTimerText.text = newText;
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


#region Timer

    [Server]
    private void Timer()
    {
        if (!countDown)
            return;

        matchTimer -= Time.deltaTime;
        UpdateTimer();

        if (matchTimer < 0)
        {
            countDown = false;
            matchTimer = 0;
            CheckTeamsScore();
        }
    }

    [Server]
    private void UpdateTimer()
    {
        float minutes = Mathf.FloorToInt(matchTimer / 60);
        float seconds = Mathf.FloorToInt(matchTimer % 60);

        goalOrTimerString = $"{minutes:00}:{seconds:00}";
    }

#endregion
}
