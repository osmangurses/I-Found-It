using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviourPunCallbacks
{
    public static Player instance;

    public PlayerStatu statu = PlayerStatu.Null;
    private void Awake()
    {
        instance = this;
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient && CustomRoomSettings.IsCustomRoom)
        {
            photonView.RPC(nameof(SyncCustomRoomSettings), newPlayer, CustomRoomSettings.ClueTime, CustomRoomSettings.PredictivationTime, CustomRoomSettings.ShowCardCount,CustomRoomSettings.cardCount);
        }
    }

    [PunRPC]
    void SyncCustomRoomSettings(float clueTime,float predictionTime,bool showCardCount,int cardCount)
    {
        CustomRoomSettings.ClueTime = clueTime;
        CustomRoomSettings.PredictivationTime = predictionTime;
        CustomRoomSettings.PredictivationTime = predictionTime;
        CustomRoomSettings.ShowCardCount = showCardCount;
        CustomRoomSettings.cardCount = cardCount;
    }
    public void GoHome()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        SceneManager.LoadScene("Menu");
    }

}
