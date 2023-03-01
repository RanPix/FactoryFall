using System.Collections.Generic;
using System.Linq;
using Mirror;
using Player;
using UnityEngine;
using UnityEngine.UI;

public class TabBar : MonoBehaviour
{
    [SerializeField] private GameObject tabBarItem;

    [SerializeField] private GameObject bluePart;
    [SerializeField] private Vector3 firstPartPosition;
    [SerializeField] private Dictionary<string, float> bluePartUnsorted = new Dictionary<string, float>();


    [SerializeField] private GameObject redPart;
    [SerializeField] private Vector3 secondPartPosition;
    [SerializeField] private Dictionary<string, float> redPartUnsorted = new Dictionary<string, float>();

    [SerializeField] private GameObject view;


    [SerializeField] private Vector3 startItemPosition;

    [SerializeField] private Color ownBlueColor;
    [SerializeField] private Color baseBlueColor;
    [SerializeField] private Color ownRedColor;
    [SerializeField] private Color baseRedColor;

    public bool firstSort = true;
    public bool canOpen = true;
    private bool isOpen = false;



    private PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.UI.Enable();

    }

    private void Update()
    {
        KeyCodes();
    }
    private void KeyCodes()
    {
        if (controls.UI.OpenTabBar.IsPressed() && !isOpen)
        {

            if(!canOpen)
                return; 

            view.SetActive(true);

            SetValues();


            isOpen = true;
        }
        else if (controls.UI.OpenTabBar.WasReleasedThisFrame() && isOpen)
        {
            view.SetActive(false);

            isOpen = false;
        }
    }

    public void SetValues()
    {
        if(!canOpen)
            return;

        bluePartUnsorted.Clear();
        redPartUnsorted.Clear();

        for (int i = 0; i < GameManager.GetAllPlayers().Length; i++)
        {
            switch (GameManager.GetAllPlayers()[i].team)
            {
                case Team.Blue:
                    bluePartUnsorted.Add(GameManager.GetAllPlayers()[i].GetNetID(), GameManager.GetAllPlayers()[i].kills);
                    break;

                case Team.Red:
                    redPartUnsorted.Add(GameManager.GetAllPlayers()[i].GetNetID(), GameManager.GetAllPlayers()[i].kills);
                    break;
            }
        }

        SortPlayers();
          
    }


    private void SortPlayers()
    {
        for ( int i = 0; bluePartUnsorted.Keys.Count > 0; i++)
        {
            if(!NetworkClient.localPlayer)
                return;
            int maxKDIndex = bluePartUnsorted.Values.ToList().IndexOf(bluePartUnsorted.Values.Max());
            bluePart.transform.GetChild(i).gameObject.SetActive(true);

            GamePlayer _bluePlayer = GameManager.GetPlayer(bluePartUnsorted.Keys.ToArray()[maxKDIndex]);

            bluePart.transform.GetChild(i).GetComponent<TabBarItem>().SetValues(_bluePlayer.nickname, _bluePlayer.kills, _bluePlayer.deaths, _bluePlayer.score);
            
            if (_bluePlayer.gameObject == NetworkClient.localPlayer?.gameObject)
            {
                bluePart.transform.GetChild(i).GetComponent<Image>().color = ownBlueColor;

            }
            else
            {
                bluePart.transform.GetChild(i).GetComponent<Image>().color = baseBlueColor;
            }
            bluePartUnsorted.Remove(bluePartUnsorted.Keys.ToArray()[maxKDIndex], out bluePartUnsorted.Values.ToArray()[maxKDIndex]);

        }

        for (int i = 0; redPartUnsorted.Keys.Count > 0; i++)
        {
            int maxKDIndex = redPartUnsorted.Values.ToList().IndexOf(redPartUnsorted.Values.Max());
            redPart.transform.GetChild(i).gameObject.SetActive(true);

            GamePlayer _redPlayer = GameManager.GetPlayer(redPartUnsorted.Keys.ToArray()[maxKDIndex]);

            redPart.transform.GetChild(i).GetComponent<TabBarItem>().SetValues(_redPlayer.nickname, _redPlayer.kills, _redPlayer.deaths, _redPlayer.score);

            if (_redPlayer.gameObject == NetworkClient.localPlayer.gameObject)
            {
                redPart.transform.GetChild(i).GetComponent<Image>().color = ownRedColor;

            }
            else
            {
                redPart.transform.GetChild(i).GetComponent<Image>().color = baseRedColor;
            }
            redPartUnsorted.Remove(redPartUnsorted.Keys.ToArray()[maxKDIndex], out redPartUnsorted.Values.ToArray()[maxKDIndex]);

        }


        /*for (int i = 0; redPartUnsorted.Count > 0; i++)
        {
            int maxKDIndex = redPartUnsorted.Keys.ToList().IndexOf(redPartUnsorted.Keys.Max());
            redPart.transform.GetChild(i).gameObject.SetActive(true);

            GamePlayer _redPlayer = GameManager.GetPlayer(redPartUnsorted.Keys.ToArray()[maxKDIndex]);

            redPart.transform.GetChild(i).GetComponent<TabBarItem>().SetValues(_redPlayer.nickname, _redPlayer.kills, _redPlayer.deaths, 10);

            if (_redPlayer.gameObject == NetworkClient.localPlayer.gameObject)
            {
                redPart.transform.GetChild(i).GetComponent<Image>().color = ownRedColor;
            }
            else
            {
                redPart.transform.GetChild(i).GetComponent<Image>().color = baseRedColor;

            }

            redPartUnsorted.Remove(redPartUnsorted.Keys.ToArray()[maxKDIndex], out redPartUnsorted.Values.ToArray()[maxKDIndex]);

        }*/


    }

}
