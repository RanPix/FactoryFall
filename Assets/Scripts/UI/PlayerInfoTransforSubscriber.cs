using Player;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoTransforSubscriber : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputfilField;
    [SerializeField] private Button blueTeamButton;
    [SerializeField] private Button redTeamButton;
    // Start is called before the first frame update
    void Start()
    {

        if (inputfilField.onDeselect.GetPersistentEventCount() < 1 && PlayerInfoTransfer.instance)
            inputfilField.onDeselect.AddListener(PlayerInfoTransfer.instance.SetName);
        if (inputfilField.onValueChanged.GetPersistentEventCount() < 1 && PlayerInfoTransfer.instance)
            inputfilField.onValueChanged.AddListener(PlayerInfoTransfer.instance.SetName);

        if (blueTeamButton.onClick.GetPersistentEventCount() < 1 && PlayerInfoTransfer.instance)
            blueTeamButton.onClick.AddListener(()=>PlayerInfoTransfer.instance.SetTeam(1));
        if (redTeamButton.onClick.GetPersistentEventCount() < 1 && PlayerInfoTransfer.instance)
            redTeamButton.onClick.AddListener(()=>PlayerInfoTransfer.instance.SetTeam(2));
    }

    // Update is called once per frame
    void Update()
    {
        if (inputfilField.onDeselect.GetPersistentEventCount() < 1 && PlayerInfoTransfer.instance)
            inputfilField.onDeselect.AddListener(PlayerInfoTransfer.instance.SetName);
        if (inputfilField.onValueChanged.GetPersistentEventCount() < 1 && PlayerInfoTransfer.instance)
            inputfilField.onValueChanged.AddListener(PlayerInfoTransfer.instance.SetName);

        if (blueTeamButton.onClick.GetPersistentEventCount() < 1 && PlayerInfoTransfer.instance)
            blueTeamButton.onClick.AddListener(()=>PlayerInfoTransfer.instance.SetTeam(1));
        if (redTeamButton.onClick.GetPersistentEventCount() < 1 && PlayerInfoTransfer.instance)
            redTeamButton.onClick.AddListener(()=>PlayerInfoTransfer.instance.SetTeam(2));
    }

}
