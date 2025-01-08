using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class PlayerPlacer : MonoBehaviourPunCallbacks
{
    Player currentPlayer;

    [Header("Team 1")]
    [SerializeField] Button t1_cluer_btn, t1_predictive0_btn, t1_predictive1_btn, t1_predictive2_btn;
    [SerializeField] Text t1_cluer_username, t1_predictive0_username, t1_predictive1_username, t1_predictive2_username;

    [Header("Team 2")]
    [SerializeField] Button t2_cluer_btn, t2_predictive0_btn, t2_predictive1_btn, t2_predictive2_btn;
    [SerializeField] Text t2_cluer_username, t2_predictive0_username, t2_predictive1_username, t2_predictive2_username;

    public List<Text> usernames; 

    Button setted_button = null;
    Text setted_username_tmp = null;
    private void Awake()
    {
        usernames = new List<Text> { t2_cluer_username, t2_predictive0_username, t2_predictive1_username, t2_predictive2_username, t1_cluer_username, t1_predictive0_username, t1_predictive1_username, t1_predictive2_username };

        currentPlayer = GetComponent<Player>();
        if (currentPlayer == null)
        {
            Debug.LogError("Player component not found on this GameObject!");
        }
        RenameGameObjects();
        AssignButtonListeners();
    }

    void RenameGameObjects()
    {
        // Team 1 Buttonlarý
        t1_cluer_btn.gameObject.name = "t1_cluer_btn";
        t1_predictive0_btn.gameObject.name = "t1_predictive0_btn";
        t1_predictive1_btn.gameObject.name = "t1_predictive1_btn";
        t1_predictive2_btn.gameObject.name = "t1_predictive2_btn";

        // Team 2 Buttonlarý
        t2_cluer_btn.gameObject.name = "t2_cluer_btn";
        t2_predictive0_btn.gameObject.name = "t2_predictive0_btn";
        t2_predictive1_btn.gameObject.name = "t2_predictive1_btn";
        t2_predictive2_btn.gameObject.name = "t2_predictive2_btn";

        // Team 1 TextMeshPro
        t1_cluer_username.gameObject.name = "t1_cluer_username";
        t1_predictive0_username.gameObject.name = "t1_predictive0_username";
        t1_predictive1_username.gameObject.name = "t1_predictive1_username";
        t1_predictive2_username.gameObject.name = "t1_predictive2_username";

        // Team 2 TextMeshPro
        t2_cluer_username.gameObject.name = "t2_cluer_username";
        t2_predictive0_username.gameObject.name = "t2_predictive0_username";
        t2_predictive1_username.gameObject.name = "t2_predictive1_username";
        t2_predictive2_username.gameObject.name = "t2_predictive2_username";
    }
    void AssignButtonListeners()
    {
        if (t1_cluer_btn != null) t1_cluer_btn.onClick.AddListener(() => OnButtonClicked(t1_cluer_btn, t1_cluer_username, PlayerStatu.T1_Cluer));
        if (t1_predictive0_btn != null) t1_predictive0_btn.onClick.AddListener(() => OnButtonClicked(t1_predictive0_btn, t1_predictive0_username, PlayerStatu.T1_Predictive));
        if (t1_predictive1_btn != null) t1_predictive1_btn.onClick.AddListener(() => OnButtonClicked(t1_predictive1_btn, t1_predictive1_username, PlayerStatu.T1_Predictive));
        if (t1_predictive2_btn != null) t1_predictive2_btn.onClick.AddListener(() => OnButtonClicked(t1_predictive2_btn, t1_predictive2_username, PlayerStatu.T1_Predictive));

        if (t2_cluer_btn != null) t2_cluer_btn.onClick.AddListener(() => OnButtonClicked(t2_cluer_btn, t2_cluer_username, PlayerStatu.T2_Cluer));
        if (t2_predictive0_btn != null) t2_predictive0_btn.onClick.AddListener(() => OnButtonClicked(t2_predictive0_btn, t2_predictive0_username, PlayerStatu.T2_Predictive));
        if (t2_predictive1_btn != null) t2_predictive1_btn.onClick.AddListener(() => OnButtonClicked(t2_predictive1_btn, t2_predictive1_username, PlayerStatu.T2_Predictive));
        if (t2_predictive2_btn != null) t2_predictive2_btn.onClick.AddListener(() => OnButtonClicked(t2_predictive2_btn, t2_predictive2_username, PlayerStatu.T2_Predictive));
    }

    void OnButtonClicked(Button button, Text usernameText, PlayerStatu statu)
    {
        if (currentPlayer != null && !InGameInfo.isGameStarted)
        {
            currentPlayer.statu = statu;

            if (photonView != null)
            {
                if (setted_button!=null)
                {
                    photonView.RPC(nameof(UpdateUIForAll), RpcTarget.AllBuffered, button.name, PhotonNetwork.NickName,  setted_button.name, statu);
                }
                else
                {
                    photonView.RPC(nameof(UpdateUIForAll), RpcTarget.AllBuffered, button.name, PhotonNetwork.NickName, "", statu);
                }
                setted_button = button;
                setted_username_tmp = usernameText;
            }
        }
    }
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        foreach (var username_tmp in usernames)
        {
            if (username_tmp.text==otherPlayer.NickName)
            {
                username_tmp.text = "";
                GetButtonByUsernameText(username_tmp).gameObject.SetActive(true);
            }
        }
    }
    [PunRPC]
    void UpdateUIForAll(string buttonName, string playerName,string setted_button_name, PlayerStatu statu)
    {

        Text usernameText = GetUsernameTextByButton(buttonName);
        Button _setted_button = GetButtonByName(setted_button_name);
        Text _setted_username_tmp = GetUsernameTextByButton(setted_button_name);
        if (_setted_button != null)
        {
            _setted_button.gameObject.SetActive(true);
            _setted_username_tmp.text = "";
        }
        if (usernameText != null)
        {
            usernameText.text = playerName;
        }

        Button button = GetButtonByName(buttonName);
        if (button != null)
        {
            button.gameObject.SetActive(false);
        }
    }
    Button GetButtonByUsernameText(Text usernameText)
    {
        switch (usernameText)
        {
            case var _ when usernameText == t1_cluer_username: return t1_cluer_btn;
            case var _ when usernameText == t1_predictive0_username: return t1_predictive0_btn;
            case var _ when usernameText == t1_predictive1_username: return t1_predictive1_btn;
            case var _ when usernameText == t1_predictive2_username: return t1_predictive2_btn;
            case var _ when usernameText == t2_cluer_username: return t2_cluer_btn;
            case var _ when usernameText == t2_predictive0_username: return t2_predictive0_btn;
            case var _ when usernameText == t2_predictive1_username: return t2_predictive1_btn;
            case var _ when usernameText == t2_predictive2_username: return t2_predictive2_btn;
            default: return null;
        }
    }

    Text GetUsernameTextByButton(string buttonName)
    {
        switch (buttonName)
        {
            case "t1_cluer_btn": return t1_cluer_username;
            case "t1_predictive0_btn": return t1_predictive0_username;
            case "t1_predictive1_btn": return t1_predictive1_username;
            case "t1_predictive2_btn": return t1_predictive2_username;
            case "t2_cluer_btn": return t2_cluer_username;
            case "t2_predictive0_btn": return t2_predictive0_username;
            case "t2_predictive1_btn": return t2_predictive1_username;
            case "t2_predictive2_btn": return t2_predictive2_username;
            default: return null;
        }
    }

    Button GetButtonByName(string buttonName)
    {
        switch (buttonName)
        {
            case "t1_cluer_btn": return t1_cluer_btn;
            case "t1_predictive0_btn": return t1_predictive0_btn;
            case "t1_predictive1_btn": return t1_predictive1_btn;
            case "t1_predictive2_btn": return t1_predictive2_btn;
            case "t2_cluer_btn": return t2_cluer_btn;
            case "t2_predictive0_btn": return t2_predictive0_btn;
            case "t2_predictive1_btn": return t2_predictive1_btn;
            case "t2_predictive2_btn": return t2_predictive2_btn;
            default: return null;
        }
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        // Tüm butonlarý ve statülerini kontrol etmek için bir liste oluþturun
        List<(Button button, PlayerStatu statu)> buttonList = new List<(Button, PlayerStatu)>()
    {
        (t1_cluer_btn, PlayerStatu.T1_Cluer),
        (t1_predictive0_btn, PlayerStatu.T1_Predictive),
        (t1_predictive1_btn, PlayerStatu.T1_Predictive),
        (t1_predictive2_btn, PlayerStatu.T1_Predictive),
        (t2_cluer_btn, PlayerStatu.T2_Cluer),
        (t2_predictive0_btn, PlayerStatu.T2_Predictive),
        (t2_predictive1_btn, PlayerStatu.T2_Predictive),
        (t2_predictive2_btn, PlayerStatu.T2_Predictive)
    };

        foreach (var (button, statu) in buttonList)
        {
            if (setted_button == button)
            {
                photonView.RPC(nameof(UpdateUIForAll), RpcTarget.AllBuffered, button.name, PhotonNetwork.NickName, "", statu);
            }
        }
    }

}

public enum PlayerStatu
{
    T1_Cluer,
    T2_Cluer,
    T1_Predictive,
    T2_Predictive,
    Null
}
