using System.Collections;
using System.Collections.Generic;
using FiniteMovementStateMachine;
using UnityEngine;
using Mirror;

public class Abilities : MonoBehaviour
{
    [SerializeField] private Ability redirect;
    [SerializeField] private Ability jump;
    private bool wasSubscribed;

    private void Start()
    {

    }
    private void Update()
    {
        if (NetworkClient.localPlayer && !wasSubscribed)
        {
            NetworkClient.localPlayer.GetComponent<MovementMachine>().midAir.OnRedirectsCountChange +=
                UpdateRedirectItems;
            NetworkClient.localPlayer.GetComponent<MovementMachine>().midAir.OnDoubleJumpsCountChange +=
                UpdateJumpItems;
            wasSubscribed = true;
        }

    }

    public void UpdateRedirectItems(int redirectCount)
    {
        for (int i = 0; i < redirect.items.Count; i++)
        {
            redirect.items[i].color = redirect.inactiveColor;
        }

        for (int i = 0; i < redirectCount; i++)
        {
            redirect.items[i].color = redirect.activeColor;
        }
    }

    public void UpdateJumpItems(int jumpCount)
    {
        for (int i = 0; i < jump.items.Count; i++)
        {
            if(!jump.items[i])
                return;
            jump.items[i].color = jump.inactiveColor;
            if (i < jumpCount)
            {
                jump.items[i].color = jump.activeColor;
            }
        }

    }
}
