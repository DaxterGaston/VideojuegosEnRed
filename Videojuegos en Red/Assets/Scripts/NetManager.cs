using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class NetManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI _roomName;
    [SerializeField] private TextMeshProUGUI _roomNamePlaceholder;
    [Space] [SerializeField] private TextMeshProUGUI _nickname;
    [SerializeField] private TextMeshProUGUI _nicknamePlaceholder;
    [Space] [SerializeField] private Button _connectButton;
    
    

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        _connectButton.interactable = false;
    }

    private void Update()
    {
        if (_nickname.text.Length >= 12)
        {
            _nickname.color = Color.red;
        }
        else
        {
            _nickname.color = Color.black;
        }
    }

    public void ConnectToRoom()
    {
        var error = false;
        
        if (_nickname.text.Length <= 1)
        {
            _nicknamePlaceholder.text = "Enter a nickname";
            _nicknamePlaceholder.color = Color.red;
            error = true;
        }

        if (_nickname.text.Length >= 12)
        {
            error = true;
        }

        if (_roomName.text.Length <= 1)
        {
            _roomNamePlaceholder.text = "Enter server name";
            _roomNamePlaceholder.color = Color.red;
            error = true;
        }


        if (error) return;
        
        var options = new RoomOptions
        {
            MaxPlayers = 4
        };
        PhotonNetwork.LocalPlayer.NickName = _nickname.text;
        PhotonNetwork.JoinOrCreateRoom(_roomName.text, options, TypedLobby.Default);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to server");
        _connectButton.interactable = true;
    }

    public override void OnCreatedRoom() { Debug.Log("Room created: " + PhotonNetwork.CurrentRoom.Name); }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room: " + PhotonNetwork.CurrentRoom.Name);
        PhotonNetwork.LoadLevel("Lobby");
    }
}