using Player;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoTransferSubscriber : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputfilField;
    [SerializeField] private Button blueTeamButton;
    [SerializeField] private Button redTeamButton;

    void Start()
    {

        if (PlayerInfoTransfer.instance)
            inputfilField.onDeselect.AddListener(PlayerInfoTransfer.instance.SetName);
        if (PlayerInfoTransfer.instance)
            inputfilField.onValueChanged.AddListener(PlayerInfoTransfer.instance.SetName);

        if (PlayerInfoTransfer.instance)
            blueTeamButton.onClick.AddListener(()=>PlayerInfoTransfer.instance.SetTeam(1));
        if (PlayerInfoTransfer.instance)
            redTeamButton.onClick.AddListener(()=>PlayerInfoTransfer.instance.SetTeam(2));
    }


}
