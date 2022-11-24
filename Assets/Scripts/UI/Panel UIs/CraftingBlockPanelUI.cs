using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingBlockPanelUI : MonoBehaviour
{
    [SerializeField] private GameObject inputSlots;
    [SerializeField] private GameObject outputSlots;
    public GameObject InputSlots => inputSlots;
    public GameObject OutputSlots => outputSlots;
}
