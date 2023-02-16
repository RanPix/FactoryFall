using TMPro;
using UnityEngine;

public class TabBarItem : MonoBehaviour
{
    [SerializeField] private TMP_Text name;
    [SerializeField] private TMP_Text kills;
    [SerializeField] private TMP_Text death;
    [SerializeField] private TMP_Text ping;
    public void SetValues(string name, int kills, int deaths, int ping)
    {
        this.name.text = name;
        this.kills.text = kills.ToString();
        this.death.text = deaths.ToString();
        this.ping.text = ping.ToString();
    }
}