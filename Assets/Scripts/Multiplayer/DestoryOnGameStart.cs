using Mirror;

public class DestoryOnGameStart : NetworkBehaviour
{
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        Destroy(gameObject);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        Destroy(gameObject);
    }
}
