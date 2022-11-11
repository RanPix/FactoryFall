using UnityEngine;
using Mirror;

public class GamePlayer : NetworkBehaviour, IDamagable
{
    [SerializeField] private Health health;

    private void Start()
    {
        if (!isLocalPlayer)
            return;

        //PlayerInteraction.instance.player = this;
    }

    /*public void Interact(IInteractable interact) =>
        InteractServer(interact);

    [Command]
    private void InteractServer(IInteractable interact) =>
        interact.Interact();



    public void RemoveBlock(Block block) =>
        RemoveBlockServer(block);

    [Command]
    private void RemoveBlockServer(Block block) =>
        block.RemoveBlock();*/

    public bool Damage(float damage)
    {
        health.currentHealth -= damage;
        return false;
    }
}
