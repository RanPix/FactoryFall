using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class ArrayElementSelector : MonoBehaviour
{
    [SerializeField] public int currentElement;
    [SerializeField] public string[] elements;
    [SerializeField] private TMP_Text elementText;
    [SerializeField] private UnityEvent OnElementChange;

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
        OnElementChange.Invoke();
    }

    public void PreviousElement()
    {
        if (currentElement == 0)
            currentElement = elements.Length - 1;
        else
            currentElement--;
        UpdateElementText();
        OnElementChange.Invoke();
    }

    public void UpdateElementText()
        => elementText.text = elements[currentElement];
}
