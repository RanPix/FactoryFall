using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class KillfeedItem : MonoBehaviour
{
	[SerializeField] private TMP_Text text;
    private Color killedPlayersColor;
    private Color killerPlayersColor;
    public void Setup(string killedPlayer, Team killedTeam, string killerPlayer, Team killerTeam)
    {
        killedPlayersColor.GetTeamColor(killedTeam);
        killerPlayersColor.GetTeamColor(killerTeam);

        text.text = $"<color=#{killedPlayersColor.ToHexString()}>{killedPlayer}</color>  killed  <color=#{killerPlayersColor.ToHexString()}>{killerPlayer}</color>";
    }
}
