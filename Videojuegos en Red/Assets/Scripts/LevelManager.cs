using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum LevelCondition
{
    PerTime,
    PerKills,
    PerDeaths
}

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private readonly LevelCondition actual;

    // Start is called before the first frame update
    void Start()
    {
        //TODO: Esto funciona unicamente si se espera que los jugadores se unan antes de comenzar la partida.
        //En caso de poder unirse a una partida en curso deberia usarse un flag y un metodo para actualizar la lista.
        var players = PhotonNetwork.PlayerList;
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: El win per time/deths se tendria que verificar en cada frame.
        //Para el win per kills se podria verificar cada vez que se elimina a un jugador para no llamar al metodo todo el tiempo.

        switch (actual)
        {
            case LevelCondition.PerTime:
                WinPerTime();
                break;
            case LevelCondition.PerKills:
                WinPerKills();
                break;
            case LevelCondition.PerDeaths:
                WinPerDeaths();
                break;
            default:
                break;
        }
    }

    void WinPerTime()
    {
        throw new NotImplementedException("Corroborar la clase que va a ser utilizada para obtener los datos de cada jugador en la sala.");
    }

    void WinPerKills()
    {
        throw new NotImplementedException("Corroborar la clase que va a ser utilizada para obtener los datos de cada jugador en la sala.");
    }

    void WinPerDeaths()
    {
        throw new NotImplementedException("Corroborar la clase que va a ser utilizada para obtener los datos de cada jugador en la sala.");
    }
}
