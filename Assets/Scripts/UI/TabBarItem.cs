using TMPro;
using UnityEngine;

public class TabBarItem : MonoBehaviour
{
    [SerializeField] private TMP_Text playerName;
    [SerializeField] private TMP_Text score;
    [SerializeField] private TMP_Text kills;
    [SerializeField] private TMP_Text death;

    public void SetValues(string name, int kills, int deaths, int score)
    {
        this.playerName.text = name;
        this.kills.text = kills.ToString();
        this.death.text = deaths.ToString();
        this.score.text = score.ToString();
    }
}
