using UnityEngine;

public class KillFeed : MonoBehaviour
{
	[SerializeField]
    GameObject killfeedItemPrefab;

    // Use this for initialization
    void Start()
    {
        GameManager.instance.OnPlayerKilledCallback += OnKill;
    }

    public void OnKill(string player, string source)
    {
        print($"{source} killed {player}");
        GameObject _killFeedItem = Instantiate(killfeedItemPrefab, this.transform);
        _killFeedItem.GetComponent<KillfeedItem>().Setup(player, source);

        Destroy(_killFeedItem, 4f);
    }
}
