using UnityEngine;

public class KillFeed : MonoBehaviour
{
	[SerializeField] GameObject killfeedItemPrefab;

    void Start()
    {
        GameManager.instance.OnPlayerKilledCallback += OnKill;
    }

    public void OnKill(string killedPlayer, Team killedTeam, string killerPlayer, Team killerTeam)
    {
        //print($"{source} killed {player}");
        GameObject _killFeedItem = Instantiate(killfeedItemPrefab, this.transform);
        _killFeedItem.GetComponent<KillfeedItem>().Setup(killedPlayer, killedTeam, killerPlayer, killerTeam);

        Destroy(_killFeedItem, 4f);
    }
}
