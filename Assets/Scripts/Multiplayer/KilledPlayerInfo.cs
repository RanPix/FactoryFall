using Player;

using TMPro;
using UnityEngine;

public class KilledPlayerInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text killedText;
    [SerializeField] private Color killedTextColor;
    private Color minkilledTextColor;

    [Space]

    [SerializeField] private GameObject textPrefab;
    [SerializeField] private Transform list;

    public void Setup(GamePlayer player)
    {
        killedTextColor = killedText.color;
        minkilledTextColor = new Color(killedTextColor.r, killedTextColor.g, killedTextColor.b, 0);
        killedText.color = new Color(0, 0, 0, 0);

        player.OnKill += ShowKilledPlayer;
    }

    private void ShowKilledPlayer(string netID, string name)
    {
        TMP_Text textInstance = Instantiate(textPrefab, list).GetComponent<TMP_Text>();

        textInstance.color = TeamToColor.GetTeamColor(GameManager.GetPlayer(netID).team);
        textInstance.text = name;

        killedText.color = killedTextColor;

        Destroy(textInstance.gameObject, 1.5f);
    }

    private void Update()
    {
        killedText.color = Color.Lerp(killedText.color, minkilledTextColor, 0.01f);
    }
}
