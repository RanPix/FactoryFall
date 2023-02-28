using System.Collections;
using System.Collections.Generic;
using Player;
using TMPro;
using UnityEngine;

public class NicknameInputField : MonoBehaviour
{
    private TMP_InputField inputfilField;
    // Start is called before the first frame update
    void Start()
    {
        inputfilField = GetComponent<TMP_InputField>();

        if(inputfilField.onDeselect.GetPersistentEventCount() < 1 && PlayerInfoTransfer.instance)
            inputfilField.onDeselect.AddListener(PlayerInfoTransfer.instance.SetName);
        if(inputfilField.onValueChanged.GetPersistentEventCount() < 1 && PlayerInfoTransfer.instance)
            inputfilField.onValueChanged.AddListener(PlayerInfoTransfer.instance.SetName);
    }

    // Update is called once per frame
    void Update()
    {
        if(inputfilField.onDeselect.GetPersistentEventCount() < 1 && PlayerInfoTransfer.instance)
            inputfilField.onDeselect.AddListener(PlayerInfoTransfer.instance.SetName);
        if(inputfilField.onValueChanged.GetPersistentEventCount() < 1 && PlayerInfoTransfer.instance)
            inputfilField.onValueChanged.AddListener(PlayerInfoTransfer.instance.SetName);
    }
}
