using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingArea : MonoBehaviour
{
    public bool placeIsClear = false;
    [SerializeField] private Material normalMat;
    [SerializeField] private Material falseMat;


    private void Update()
    {
        if(placeIsClear == true)
            GetComponent<Renderer>().material = normalMat;
        else
            GetComponent<Renderer>().material = falseMat;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform != GetComponentInParent<Transform>())
        {
            placeIsClear = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        placeIsClear = true;
    }
}
