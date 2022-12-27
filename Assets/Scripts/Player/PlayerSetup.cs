using UnityEngine;
using Mirror;

public class PlayerSetup : NetworkBehaviour
{
    [SerializeField] private Behaviour[] componentsToDisable;

    [HideInInspector] public bool _isLocalPlayer { get; set; } = false;

    [Client]
    private void Start()
    {
        if (_isLocalPlayer)
            return;


        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }
    }
}
