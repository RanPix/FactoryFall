using UnityEngine;

public class SpawnPoints : MonoBehaviour
{
    public static SpawnPoints instance;
    public Transform[] spawnPoints;
    private int currentSpawnPoint = 0;

    private void Awake()
    {
        if(instance == null)
            instance = this;
    }

    public Transform GetSpawnPoint() => spawnPoints[currentSpawnPoint++ % spawnPoints.Length];
}
