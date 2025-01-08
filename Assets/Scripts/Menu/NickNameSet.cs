using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class NickNameSet : MonoBehaviourPunCallbacks
{
    [SerializeField] InputField username_input;

    void Start()
    {
        if (PlayerPrefs.HasKey("Nick"))
        {
            username_input.text = PlayerPrefs.GetString("Nick");
            SetPlayerNickName(username_input.text);
        }
        else
        {
            username_input.text = "user_" + Random.Range(1000, 9999);
            SetPlayerNickName(username_input.text);
        }
        username_input.onEndEdit.AddListener(SetPlayerNickName);
    }

    void SetPlayerNickName(string input)
    {
        if (!string.IsNullOrWhiteSpace(input))
        {
            PhotonNetwork.NickName = input;
            PlayerPrefs.SetString("Nick", username_input.text);
            Debug.Log($"Nickname set to: {PhotonNetwork.NickName}");
        }
        else
        {
            username_input.text = "user_" + Random.Range(1000, 9999);
            SetPlayerNickName(username_input.text);
        }
        PlayerPrefs.SetString("Nick", username_input.text);
    }
}
