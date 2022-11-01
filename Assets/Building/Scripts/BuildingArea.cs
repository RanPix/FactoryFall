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

    private void OnTriggerEnter(Collider other)
    {
        bool thisObjExists = false;

        for (int i = 0; i < objectsOnArea.Count; i++) 
        {
            if (other.gameObject == objectsOnArea[i])
            {
                thisObjExists = true; break;
            }
        }

        if (!thisObjExists) objectsOnArea.Add(other.gameObject); 
        foreach(GameObject g in objectsOnArea)
            print(g);
    }

    private void OnTriggerExit(Collider other)
    {
        for (int i = 0; i < objectsOnArea.Count; i++)
        {
            if (other.gameObject == objectsOnArea[i])
            {
                objectsOnArea.RemoveAt(i); break;
            }
        }
    }
}
