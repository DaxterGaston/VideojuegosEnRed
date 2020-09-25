using Photon.Pun.Demo.Cockpit;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class RoomListing : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _roomText;
    public RoomInfo RoomInfo { get; private set; }

    public void SetRoomInfo(RoomInfo roomInfo)
    {
        RoomInfo = roomInfo;
        _roomText.text = roomInfo.MaxPlayers + ", " + roomInfo.Name;
    }
}