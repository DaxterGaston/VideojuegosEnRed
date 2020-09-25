using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class IntantiateLifeRestore : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        PhotonNetwork.Instantiate("LifePoint", transform.position, Quaternion.identity); 
        // photonView.RPC("Disable", RpcTarget.AllBuffered);   
    }

    [PunRPC]
    public void Disable()
    {
        this.enabled = false;
    }
}