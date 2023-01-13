using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum ButtonGroup
{
    Main = 0,
    ConnectToSomeoneOrHost = 1,
    Host = 2,
    ConnectToSomeone = 3,
}

public class MainMenu : MonoBehaviour
{

    ButtonGroup currentButtonGroup = ButtonGroup.Main;
    [SerializeField] GameObject[] MainMenuButtonGroupsToGroupGameObject;
    [SerializeField] float buttonGroupMoveTime;
    [SerializeField] Vector3 closedGroupPosition;
    [SerializeField] Vector3 openedGroupPosition;


    void Start()
    {
        SetMainButtonGroup();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void SetMainButtonGroup()
        => SetMenuButtonGroup(ButtonGroup.Main);
    public void SetConnectToSomeoneOrHostButtonGroup()
        => SetMenuButtonGroup(ButtonGroup.ConnectToSomeoneOrHost);
    public void SetHostButtonGroup()
        => SetMenuButtonGroup(ButtonGroup.Host);
    public void SetConnectToSomeoneButtonGroup()
        => SetMenuButtonGroup(ButtonGroup.ConnectToSomeone);

    public void SetMenuButtonGroup(ButtonGroup buttonGroup)
    {
        currentButtonGroup = buttonGroup;
        UpdateButtonGroupsPosition();
    }

    public void UpdateButtonGroupsPosition()
    {

        for (int i = 0; i < MainMenuButtonGroupsToGroupGameObject.Length; i++)
            if (i != (int)currentButtonGroup)
                MainMenuButtonGroupsToGroupGameObject[i].transform.DOMove(closedGroupPosition, buttonGroupMoveTime);

        MainMenuButtonGroupsToGroupGameObject[(int)currentButtonGroup].transform.DOMove(openedGroupPosition, buttonGroupMoveTime);
    }
}
