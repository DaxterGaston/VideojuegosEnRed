using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Instantiator : MonoBehaviourPunCallbacks
{
    [SerializeField] private List<Transform> _spawnPoints;
    [SerializeField] private int _index;

    private void Start()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (PhotonNetwork.PlayerList[i] == PhotonNetwork.LocalPlayer)
            {
                var o = PhotonNetwork.Instantiate("Dino", _spawnPoints[i].position, Quaternion.identity);
                o.GetComponent<Character>().SpawnPoint = _spawnPoints[i];
            }
        }
    }
}