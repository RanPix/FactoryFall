using UnityEngine;
using TMPro;

public class KillfeedItem : MonoBehaviour
{
	[SerializeField] private TMP_Text text;

    public void Setup(string player, string source)
    {
        text.text = "<b>" + source + "</b>" + " killed " + "<i>" + player + "</i>";
    }
}
