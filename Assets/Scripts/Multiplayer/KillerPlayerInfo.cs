using Player;
using System.Collections;
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

    private void ShowKillerInfo(string netID, string name, int hp)
    {
        this.netID.text = $"ID: {netID}";
        this.nickname.text = name;
        this.hp.text = hp.ToString();


        gameObject.SetActive(true);
    }

    private void HideKillerInfo() => gameObject.SetActive(false);
}
