using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCameraPositioner : MonoBehaviour
{
    //[SerializeField] public int numm;
    [SerializeField] public Vector3[] positions;
    [SerializeField] public Vector3[] rotations;
    void Start()
    {
        int num = Random.Range(0, positions.Length);
        transform.position = positions[num];
        transform.eulerAngles = rotations[num];
    }
}
