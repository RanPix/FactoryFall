using Player;
using TMPro;
using UnityEngine;

public class KillerPlayerInfo : MonoBehaviour
{
    [SerializeField] TMP_Text netID;
    [SerializeField] TMP_Text nickname;
    [SerializeField] TMP_Text hp;

    public void Setup(GamePlayer player)
    {
        gameObject.SetActive(false);

        player.OnDeath += ShowKillerInfo;
        player.OnRespawn += HideKillerInfo;
    }

    private void ShowKillerInfo(string netID, Team team, string name, int hp)
    {
        this.netID.text = $"ID: {netID}";

        nickname.color = TeamToColor.GetTeamColor(team);
        nickname.text = name;

        this.hp.text = hp.ToString();


        gameObject.SetActive(true);
    }

    private void HideKillerInfo() => gameObject.SetActive(false);
}
