using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class LobbySystem : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI _playerListText;

    [SerializeField] private TextMeshProUGUI _textButtonStart;
    [SerializeField] private Button _buttonStart;

    private void Start()
    {
        _buttonStart.interactable = false;
        UpdatePlayerList();
    }

    private void UpdatePlayerList()
    {
        var playerList = "";

        foreach (var player in PhotonNetwork.PlayerList)
        {
            playerList += player.NickName + "\n";
        }

        _playerListText.text = playerList;

        if (PhotonNetwork.PlayerList.Length > 0)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                _textButtonStart.text = "Start";
                _buttonStart.interactable = true;
            }
            else
            {
                _textButtonStart.text = "Waiting for Master Client";
                _buttonStart.interactable = false;
            }
        }
        else
        {
            _textButtonStart.text = "Waiting for Players";
            _buttonStart.interactable = false;
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) { UpdatePlayerList(); }

    public override void OnPlayerLeftRoom(Player otherPlayer) { UpdatePlayerList(); }

    public void StartGame()
    {
        photonView.RPC("LoadGameScene", RpcTarget.All, "Level01");
    }

    [PunRPC]
    public void LoadGameScene(string sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);
    }
}