using System;
using UnityEngine;

public class OreGiveAwayArea : MonoBehaviour
{
    public static OreGiveAwayArea instance { get; private set; }
    public Action OnAreaEnter;
    private void Awake()
    {
        if(instance == null)
            instance = this;

    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "LocalPlayer")
        {
            OnAreaEnter?.Invoke();
        }
    }
}
