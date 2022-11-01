using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingArea : MonoBehaviour
{
    public bool placeIsClear = false;
    [SerializeField] private List<GameObject> objectsOnArea;
    [SerializeField] private Material trueMat;
    [SerializeField] private Material falseMat;
    private Renderer thisRenderer;


    private void Awake()
    {
        thisRenderer = GetComponent<Renderer>();
    }


    private void Update()
    {
        if (objectsOnArea.Count == 0)
        {
            placeIsClear = true;
            thisRenderer.material = trueMat;
        }
        else
        {
            placeIsClear = false;
            thisRenderer.material = falseMat;
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        placeIsClear = false;

        bool thisObjExists = false;

        
        for (int i = 0; i < objectsOnArea.Count; i++) 
        {
            if (other.gameObject == objectsOnArea[i])
            {
                thisObjExists = true; break;
            }
        }

        if (!thisObjExists) objectsOnArea.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        placeIsClear = true;

        for (int i = 0; i < objectsOnArea.Count; i++)
        {
            if (other.gameObject == objectsOnArea[i])
            {
                objectsOnArea.RemoveAt(i); break;
            }
        }
    }
}
