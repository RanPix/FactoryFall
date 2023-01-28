using UnityEngine;

public class DestoryOnGameStart : MonoBehaviour
{
    private void Start()
    {
        GameManager.instance.OnClientStart += DestroySelf;
    }

    private void DestroySelf() => Destroy(gameObject);
}
