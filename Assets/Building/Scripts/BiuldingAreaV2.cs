using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiuldingAreaV2 : MonoBehaviour
{
    [Header("Place is clear?")]
    public bool placeIsClear = false;

    [Space(5)]

    [Header("Other")]
    [SerializeField] private Material trueMat;
    [SerializeField] private Material falseMat;
    private Renderer thisRenderer;


    private void Awake()
    {
        thisRenderer = GetComponent<Renderer>();
    }

    private void Update() //оцю хуйню поправте                    нижче
    {
        placeIsClear = Physics.CheckBox(transform.position, transform.localScale / 2, transform.rotation);
    }
}
