using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerList : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _playersListText;
    [SerializeField] private TextMeshProUGUI _pingText;
    
    private void Update()
    {
        string playersNames = "";
        var players = PhotonNetwork.PlayerList;
        foreach (var player in players)
        {
            // playersNames += player.NickName + " " + (int)player.CustomProperties["DeathCounter"] + "\n";
            playersNames += player.NickName + "\n";
        }
        
        _playersListText.text = playersNames;
        
        _pingText.text = PhotonNetwork.GetPing().ToString();
    }
}
