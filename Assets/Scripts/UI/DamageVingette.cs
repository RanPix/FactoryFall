using Player;
using UnityEngine;
using UnityEngine.UI;

public class DamageVingette : MonoBehaviour
{
    private Color maxDamageColor;
    private Color minColor;
    [SerializeField] private RawImage vingette;
 
    private void Start()
    {
        maxDamageColor = vingette.color;
        minColor = new Color(maxDamageColor.r, maxDamageColor.g, maxDamageColor.b, 0);
        vingette.color = new Color(0, 0, 0, 0);
    }

    public void Setup(GamePlayer player)
    {
        player.OnGotHit += OnGotHit;
    }

    private void OnGotHit(string id, int amount)
    {
        vingette.color = maxDamageColor * (amount * 0.05f);
    }

    private void Update()
    {
        vingette.color = Color.Lerp(vingette.color, minColor, 0.1f);
    }
}
