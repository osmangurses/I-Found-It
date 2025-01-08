using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class PhotonBase : MonoBehaviourPunCallbacks
{
    private void Awake()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.Disconnect();
        }
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Connecting to Photon Network...");
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        Debug.Log("Connected to Master");
        Debug.Log("Joining Lobby...");
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
    }

    public void JoinOrCreateRandomRoom()
    {
        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = (byte)8, // int deðeri byte türüne dönüþtürüldü
            IsVisible = true
        };

        PhotonNetwork.JoinRandomOrCreateRoom(null, (byte)roomOptions.MaxPlayers, MatchmakingMode.FillRoom, null, null, null, roomOptions);
    }
    public override void OnJoinedRoom()
    {
        string result = PhotonNetwork.IsMasterClient ? "Room created as Master" : "Joined existing room";
        Debug.Log(result);
        SceneManager.LoadScene("Game");
    }
}
