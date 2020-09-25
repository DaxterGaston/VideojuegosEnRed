using System;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class ProyectileController : MonoBehaviourPun
{
    private readonly float _speed = 7f;
    private Vector3 _dir;

    private GameObject _creator;

    public GameObject Creator { get => _creator; set => _creator = value; }
    public Vector3 Dir { get => _dir; set => _dir = value; }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * (_speed * Time.deltaTime); 
        
    }
    

    private void OnTriggerEnter(Collider other)
    {
        //Verifico si colisiono con un jugador.
        if (other.CompareTag("Player") && _creator != other.gameObject)
        {
            if (photonView.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }

            return;
        }

        if (other.CompareTag("Obstacle"))
        {
            //Al colisionar con cualquier cosa destruyo este gameObject.
            if (photonView.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}