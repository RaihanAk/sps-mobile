using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using ExitGames.Client.Photon;

enum Map
{
    Forest,
    Desert,
    Tundra,
    OtherScene
}

public class SPSLobbyMain : MonoBehaviourPunCallbacks
{
    private int roomRandomNumber;
    private int playerCount;
    private int roomSize;
    public string mapProperty;
    private string modeProperty;

    public InputField m_PlayerNameInput;
    public Text m_PlayerListText;
    public Text m_PlayerCountText;
    public Dropdown m_MapDropdown;

    public GameObject MainField;
    public GameObject LoginField;
    public GameObject SelectionField;
    public GameObject RoomField;

    #region UNITY

    public void Awake()
    {
        m_PlayerNameInput.text = "Player " + Random.Range(1, 100);
        mapProperty = "forest";
        modeProperty = "brawl";

        // Activate in sequence
        MainField.SetActive(true);
        LoginField.SetActive(false);
        SelectionField.SetActive(false);
        RoomField.SetActive(false);
    }

    #endregion

    #region PUN CALLBACKS

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        Debug.Log("OnConnectedToMaster() was called by PUN.");

        LoginField.SetActive(false);
        SelectionField.SetActive(true);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);

        CreateRoom();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);

        Debug.Log("Failed Create Room, try random nother number");
        CreateRoom();
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        Debug.Log("Currently on Room " + roomRandomNumber);

        SelectionField.SetActive(false);
        RoomField.SetActive(true);

        PlayerCountUpdate();

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            //
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        m_PlayerListText.text = newPlayer.NickName + "Joined!";
        PlayerCountUpdate();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        m_PlayerListText.text = otherPlayer.NickName + "Left..";
        PlayerCountUpdate();
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        base.OnRoomPropertiesUpdate(propertiesThatChanged);

        Debug.Log("OnRoomPropertiesUpdate() " + propertiesThatChanged.ToStringFull());

        // kenapa ga kepanggil ya
        if (propertiesThatChanged.ContainsKey(GameModeProperty.Map))
        {
            mapProperty = propertiesThatChanged[GameModeProperty.Map].ToString();
        }
    }

    #endregion

    #region UI CALLBACKS

    public void OnMatchButtonClicked()
    {
        MainField.SetActive(false);
        LoginField.SetActive(true);
    }

    public void OnLoginButtonClicked()
    {
        string playerName = m_PlayerNameInput.text;

        if (!playerName.Equals(""))
        {
            PhotonNetwork.LocalPlayer.NickName = playerName;
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            Debug.LogError("Player Name is invalid.");
        }
    }

    public void OnRandomButtonClicked()
    {
        PhotonNetwork.JoinRandomRoom();
    }


    public void OnMapSelectionClicked()
    {
        if (PhotonNetwork.CurrentRoom != null)
        {
            // We're gonna compare the map value with enum Map. So it cant be stored as string
            PhotonNetwork.CurrentRoom.CustomProperties[GameModeProperty.Map] =
                m_MapDropdown.value;

            Debug.Log("Map changed to " + PhotonNetwork.CurrentRoom.CustomProperties[GameModeProperty.Map], this);
        }

        //PhotonNetwork.CurrentRoom.IsVisible = (!PhotonNetwork.CurrentRoom.IsVisible);
    }

    public void OnPlayButtonClicked()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            m_PlayerListText.text = "Loading level for " + m_MapDropdown.options[m_MapDropdown.value].text +"...";
            Debug.Log("Starting Game on " + PhotonNetwork.CurrentRoom.CustomProperties[GameModeProperty.Map]);
            PhotonNetwork.CurrentRoom.IsOpen = false;

            switch (PhotonNetwork.CurrentRoom.CustomProperties[GameModeProperty.Map])
            {
                case (int)Map.Forest:
                    // PhotonNetwork.LoadLevel("Forest");
                    break;
                case (int)Map.Desert:
                    // PhotonNetwork.LoadLevel("Desert");
                    break;
                case (int)Map.Tundra:
                    // PhotonNetwork.LoadLevel("Tundra");
                    break;
                case (int)Map.OtherScene:
                    PhotonNetwork.LoadLevel("SecondaryScene");
                    break;
                default:
                    PhotonNetwork.LoadLevel("CoopScene");
                    Debug.Log("Loading level gone for default case");
                    break;
            }
        }
    }

    #endregion

    private void CreateRoom()
    {
        Debug.Log("Join Random Failed, Creating now..");

        roomRandomNumber = Random.Range(0, 10000);
        RoomOptions roomOptions = new RoomOptions();

        // Default prop
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = 4;
        roomOptions.IsOpen = true;

        // Custom room options
        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable();
        roomOptions.CustomRoomProperties.Add(GameModeProperty.Map, 0);
        roomOptions.CustomRoomProperties.Add(GameModeProperty.Mode, 0);
        // Debug.Log("Key Map = " + GameModeProperty.Map);

        // Properties to be listed on lobby list
        roomOptions.CustomRoomPropertiesForLobby = new string[]
        {
            GameModeProperty.Map,
            GameModeProperty.Mode
        };

        // Kureeto
        PhotonNetwork.CreateRoom("Room " + roomRandomNumber, roomOptions);
        Debug.Log("Created Room " + roomRandomNumber);
    }

    private void PlayerCountUpdate()
    {
        playerCount = PhotonNetwork.PlayerList.Length;
        roomSize = PhotonNetwork.CurrentRoom.MaxPlayers;

        m_PlayerCountText.text = playerCount + " / " + roomSize;

        if (playerCount == roomSize)
        {
            // Ready to Start
        }
        
        if (playerCount == 1)
            m_PlayerListText.text = "You are alone...";
    }
}
