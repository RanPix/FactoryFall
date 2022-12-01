using UnityEngine;
using System.Collections.Generic;

public class PlayerInteraction : MonoBehaviour
{
    public static PlayerInteraction instance;
    public GamePlayer player;

    [SerializeField] private float interactionDistance;

    private PlayerControls controls;
    private RaycastHit interact;

    [SerializeField] private float dmg; // test field

    private void Start()
    {
        instance = this;

        controls = new PlayerControls();
        controls.Player.Enable();
    }

    private void Update()
    {
        CheckInteraction();
    }

    private void CheckInteraction() 
    {
        if (controls.Player.Interact.WasPerformedThisFrame())
        {
            if (Physics.Raycast(transform.position, transform.forward, out interact, interactionDistance))
                interact.transform.GetComponent<IInteractable>();
        }

        
    }
}
