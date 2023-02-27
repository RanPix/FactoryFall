using Player;
using UnityEngine;
using TMPro;
using UnityEngine.TextCore.Text;

public class NicknameLookAtCamera : MonoBehaviour
{
    [SerializeField] private Transform localPlayer;
    private Camera cam;

    [SerializeField] private Material normalFontMaterial;
    [SerializeField] private Material ROTFontMaterial; // render on top

    private void Start()
    {
        localPlayer = GameObject.FindGameObjectWithTag("LocalPlayer")?.GetComponent<Transform>();

        if (localPlayer != null)
        {
            Team localPlayerTeam = localPlayer.GetComponent<GamePlayer>().team;
            Team thisPlayerTeam = GetComponentInParent<GamePlayer>().team;


            GetComponentInChildren<TMP_Text>().fontSharedMaterial = 
                localPlayerTeam == thisPlayerTeam ?
                ROTFontMaterial :
                normalFontMaterial;
            
        }    

        cam = Camera.main;
    }

    private void LateUpdate()
    {
        if (localPlayer)
        {
            transform.LookAt(cam.transform);
            
            float magnitudeFromCamera = (cam.transform.position - transform.position).magnitude * 0.2f;
            transform.localScale = new Vector3(magnitudeFromCamera, magnitudeFromCamera, magnitudeFromCamera);
        }
        else
        {
            cam = Camera.main;
            localPlayer = GameObject.FindGameObjectWithTag("LocalPlayer")?.GetComponent<Transform>();

            Team localPlayerTeam = localPlayer.GetComponent<GamePlayer>().team;
            Team thisPlayerTeam = GetComponentInParent<GamePlayer>().team;

            GetComponentInChildren<TMP_Text>().fontSharedMaterial =
                localPlayerTeam == thisPlayerTeam ?
                ROTFontMaterial :
                normalFontMaterial;
        }

    }
}
