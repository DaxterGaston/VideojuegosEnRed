using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ServersList : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform _content;
    [SerializeField] private RoomListing _roomListing;
    [SerializeField] private List<RoomListing> _listings;

    private void Start()
    {
        for (int i = 0; i < 50; i++)
        {
            Instantiate(_roomListing, _content);
        }
    }

    public void CreateAsdd()
    {
        Instantiate(_roomListing, _content);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (var roomInfo in roomList)
        {
            if (roomInfo.RemovedFromList)
            {
                // Eliminados de la lista


                var index = _listings.FindIndex(x => x.RoomInfo.Name == roomInfo.Name);
                if (index != -1)
                {
                    Destroy(_listings[index].gameObject);
                    _listings.RemoveAt(index);
                }
            }
            else
            {
                // Añadidos a la list
                var listing = Instantiate(_roomListing, _content);
                if (listing != null)
                {
                    listing.SetRoomInfo(roomInfo);
                    _listings.Add(listing);
                }
            }
        }
    }
}