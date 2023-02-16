using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneCamera : MonoBehaviour
{
    [SerializeField] private Transform[] positions;
    // Start is called before the first frame update
    void Awake()
    {
        int random = Random.Range(0, positions.Length);
        gameObject.transform.SetPositionAndRotation(positions[random].position, positions[random].rotation);
        print(positions[random].name);
    }

}
