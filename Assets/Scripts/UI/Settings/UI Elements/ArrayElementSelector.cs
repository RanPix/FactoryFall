using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ArrayElementSelector : MonoBehaviour
{
    [SerializeField] public int currentElement;
    [SerializeField] public string[] elements;
    [SerializeField] private TMP_Text elementText;

    void Start()
    {
        UpdateElementText();
    }

    public void NextElement()
    {
        if (currentElement == elements.Length - 1)
            currentElement = 0;
        else
            currentElement++;
        UpdateElementText();
    }

    public void PreviousElement()
    {
        if (currentElement == 0)
            currentElement = elements.Length - 1;
        else
            currentElement--;
        UpdateElementText();
    }

    void UpdateElementText()
        => elementText.text = elements[currentElement];
}
