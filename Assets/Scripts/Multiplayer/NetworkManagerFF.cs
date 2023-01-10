using UnityEngine;
using Mirror;

public class NetworkManagerFF : NetworkManager
{
    public static Transform GetRespawnPosition() => startPositions[startPositionIndex++ % startPositions.Count];
}
