using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class LifePointScript : MonoBehaviourPun
{
    [SerializeField] private bool _enable;
    [SerializeField] private Animator _animator;

    private void Start()
    {
        if (!photonView.IsMine)
        {
            Destroy(this);
        }
    }

    private void Update() { _animator.SetBool("Enable", _enable); }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_enable)
            {
                int amount = 4;
                other.GetComponent<Character>().RestoreHealth(amount);
            }

            _enable = false;
            Invoke("EnableAgain", 10f);
        }
    }


    private void EnableAgain() { _enable = true; }
}