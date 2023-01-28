using UnityEngine;
using TMPro;

public class KillfeedItem : MonoBehaviour
{
	[SerializeField] private TMP_Text text;

    public void Setup(string killedPlayer, Team killedTeam, string killerPlayer, Team killerTeam)
    {
        if (killedTeam is Team.Blue)
            text.color = new Color(0f, 0.4f, 1f);
        else if (killedTeam is Team.Red)
            text.color = Color.red;

        if (killerTeam is Team.Blue)
            text.color = new Color(0f, 0.4f, 1f);
        else if (killerTeam is Team.Red)
            text.color = Color.red;

        text.text = $"<color=>{killedPlayer}  killed  {killerPlayer}";
    }
}
