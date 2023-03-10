using UnityEditor.SceneManagement;
using UnityEngine;

public class KillFeed : MonoBehaviour
{
	[SerializeField] GameObject killfeedItemPrefab;

    private void Start()
    {
        if (GameManager.instance)
        {
            SubscribeToKill();
        }
        else
        {
            GameManager.OnGameManagerSet += SubscribeToKill;
        }
            
    }

    private void SubscribeToKill()
    {
        GameManager.instance.OnPlayerKilledCallback += OnKill;
    }


    private void OnKill(string killedPlayer, Team killedTeam, string killerPlayer, Team killerTeam)
    {
        //print($"{source} killed {player}");
        GameObject _killFeedItem = Instantiate(killfeedItemPrefab, this.transform);
        _killFeedItem.GetComponent<KillfeedItem>().Setup(killedPlayer, killedTeam, killerPlayer, killerTeam);

        Destroy(_killFeedItem, 4f);
    }

    private void OnScore(string player, Team team)
    {
        print($"{player} from {team} scored!");
    }
}
