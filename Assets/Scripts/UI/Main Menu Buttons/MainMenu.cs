using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MainMenuButtonGroups
{
    Main = 1,
    ConnectToSomeoneOrHost = 2,
    Host = 3,
    ConnectToSomeone = 4,
}

public class MainMenu : MonoBehaviour
{

    [SerializeField] Dictionary<MainMenuButtonGroups, GameObject> MainMenuButtonGroupsToGroupGameObject;

    void Start()
    {
        
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void PlayButtonClick()
    {

    }

    public void SetMenuButtonGroup(MainMenuButtonGroups mainMenuButtonGroup)
    {

    }
}
