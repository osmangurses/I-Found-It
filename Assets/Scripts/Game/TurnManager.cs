using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using DG.Tweening;

public class TurnManager : MonoBehaviourPunCallbacks
{
    public static TurnManager instance;
    public PlayerStatu turn;
    public GameObject start_game_button;
    public GameObject selected_deck_panel;
    public Slider time_slider;
    public Image turn_info_frame_image;
    public TextMeshProUGUI turn_info_tmp;

    bool isTimeOver = false;
    private void Awake()
    {
        instance = this;
        if (PhotonNetwork.IsMasterClient)
        {
            start_game_button.SetActive(true);
        }
        else
        {
            start_game_button.SetActive(false);
        }
        start_game_button.GetComponent<Button>().onClick.AddListener(StartGame);
        InGameInfo.isGameStarted = false;
        AudioPlayer.instance.PlayAudio(AudioName.Music);
    }

    [PunRPC]
    void SetTurn(string turnStatu)
    {
        if (ScoreManager.instance.team1_score>(SpriteGenerator.instance.spriteCountOnPanel/2))
        {
            turn_info_frame_image.DOFade(1, 0.1f).OnComplete(() =>
            turn_info_tmp.DOText("TEAM 1 WON", 0.2f));
            turn_info_frame_image.raycastTarget = true;
            DeckManager.instance.hint_input.gameObject.SetActive(false);
            DeckManager.instance.send_guess_button.gameObject.SetActive(false);
            AudioPlayer.instance.PlayAudio(AudioName.Winner);
            InGameInfo.isGameStarted = false;
            return;
        }
        else if (ScoreManager.instance.team2_score > (SpriteGenerator.instance.spriteCountOnPanel / 2))
        {
            turn_info_frame_image.DOFade(1, 0.1f).OnComplete(() =>
            turn_info_tmp.DOText("TEAM 2 WON", 0.2f));
            turn_info_frame_image.raycastTarget = true;
            DeckManager.instance.hint_input.gameObject.SetActive(false);
            DeckManager.instance.send_guess_button.gameObject.SetActive(false);
            AudioPlayer.instance.PlayAudio(AudioName.Winner);
            InGameInfo.isGameStarted = false;
            return;
        }
        else if ((ScoreManager.instance.team2_score - ScoreManager.instance.team1_score)==0 && ScoreManager.instance.team2_score>1 && DeckManager.instance.icons_on_game.Count==0)
        {
            turn_info_frame_image.DOFade(1, 0.1f).OnComplete(() =>
            turn_info_tmp.DOText("DRAW", 0.2f));
            turn_info_frame_image.raycastTarget = true;
            DeckManager.instance.hint_input.gameObject.SetActive(false);
            DeckManager.instance.send_guess_button.gameObject.SetActive(false);
            AudioPlayer.instance.PlayAudio(AudioName.Winner);
            return;
        }
        isTimeOver = false;
        time_slider.value = time_slider.maxValue;
        turn = (PlayerStatu)System.Enum.Parse(typeof(PlayerStatu), turnStatu, true);
        CheckUI();
        float maxTime = (turn == PlayerStatu.T1_Cluer || turn == PlayerStatu.T2_Cluer) ? CustomRoomSettings.ClueTime : CustomRoomSettings.PredictivationTime;
        time_slider.maxValue = maxTime;
        time_slider.value = maxTime;

        string turn_info = "";

        switch (turn)
        {
            case PlayerStatu.T1_Cluer:
                turn_info = "TEAM 1 CLUER";
                break;
            case PlayerStatu.T2_Cluer:
                turn_info = "TEAM 2 CLUER";
                break;
            case PlayerStatu.T1_Predictive:
                turn_info = "TEAM 1 PREDICTIVE";
                break;
            case PlayerStatu.T2_Predictive:
                turn_info = "TEAM 2 PREDICTIVE";
                break;
            default:
                turn_info = "UNKNOWN";
                break;
        }

        AudioPlayer.instance.PlayAudio(AudioName.NextTurn);
        turn_info_frame_image.DOFade(1, 0.1f).OnComplete(() =>
        turn_info_tmp.DOText(turn_info,0.2f));
        turn_info_tmp.DOText("", 0.1f).SetDelay(1.5f).OnComplete(() =>
        turn_info_frame_image.DOFade(0, 0.1f)); ;
    }

    void CheckUI()
    {
        if (IsMyTurn())
        {
            DeckManager.instance.hint_input.text="";
            if (Player.instance.statu==PlayerStatu.T1_Cluer || Player.instance.statu == PlayerStatu.T2_Cluer)
            {
                DeckManager.instance.hint_input.gameObject.SetActive(true);
            }
            else if (Player.instance.statu==PlayerStatu.T1_Predictive || Player.instance.statu == PlayerStatu.T2_Predictive)
            {
                DeckManager.instance.send_guess_button.gameObject.SetActive(true);
            }
            else
            {
                DeckManager.instance.hint_input.gameObject.SetActive(false);
                DeckManager.instance.send_guess_button.gameObject.SetActive(false);
            }
        }
        else
        {
            DeckManager.instance.hint_input.gameObject.SetActive(false);
            DeckManager.instance.send_guess_button.gameObject.SetActive(false);
        }
        if ((turn== PlayerStatu.T1_Predictive && Player.instance.statu== PlayerStatu.T1_Cluer) || (turn == PlayerStatu.T2_Predictive && Player.instance.statu == PlayerStatu.T2_Cluer))
        {
            selected_deck_panel.SetActive(true);
        }
        else
        {
            selected_deck_panel.SetActive(false);
        }
    }
    public void NextTurn()
    {
        if (turn==PlayerStatu.T1_Cluer)
        {
            photonView.RPC(nameof(SetTurn), RpcTarget.All, PlayerStatu.T1_Predictive.ToString());
        }
        else if (turn==PlayerStatu.T1_Predictive)
        {
            photonView.RPC(nameof(SetTurn), RpcTarget.All, PlayerStatu.T2_Cluer.ToString());
        }
        else if (turn==PlayerStatu.T2_Cluer)
        {
            photonView.RPC(nameof(SetTurn), RpcTarget.All, PlayerStatu.T2_Predictive.ToString());
        }
        else if (turn==PlayerStatu.T2_Predictive)
        {
            photonView.RPC(nameof(SetTurn), RpcTarget.All, PlayerStatu.T1_Cluer.ToString());
        }
    }
    public bool IsMyTurn()
    {
        return (Player.instance.statu == turn) ? true : false;
    }



    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            turn = ((int)Random.Range(0, 2) > 0) ? PlayerStatu.T1_Cluer : PlayerStatu.T2_Cluer;
            photonView.RPC(nameof(SetTurn), RpcTarget.AllBuffered, turn.ToString());
        }
        start_game_button.SetActive(false);
        SpriteGenerator.instance.SpawnSprites();
        photonView.RPC(nameof(SetStartGame),RpcTarget.AllBuffered);
        PhotonNetwork.CurrentRoom.IsVisible = false;
    }
    [PunRPC]
    void SetStartGame()
    {

        InGameInfo.isGameStarted = true;
        AudioPlayer.instance.PlayAudio(AudioName.StartGame);
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
        {
            { "statu", Player.instance.statu }
        });

    }
    private int GetLowestActorNumberInTurn()
    {
        int lowestActorNumber = int.MaxValue;

        foreach (var player in PhotonNetwork.PlayerList)
        {
            // Ayný turda olan oyuncularý filtreleyin
            if (player.CustomProperties.ContainsKey("statu") &&
                (PlayerStatu)player.CustomProperties["statu"] == turn)
            {
                if (player.ActorNumber < lowestActorNumber)
                {
                    lowestActorNumber = player.ActorNumber;
                }
            }
        }

        return lowestActorNumber;
    }

    private void Update()
    {
        if (InGameInfo.isGameStarted)
        {
            time_slider.value -= Time.deltaTime;
        }

        if (time_slider.value <= 0.1f && !isTimeOver)
        {
            if (IsMyTurn())
            {
                if (Player.instance.statu == PlayerStatu.T1_Cluer || Player.instance.statu == PlayerStatu.T2_Cluer)
                {
                    DeckManager.instance.SendDeckToPredictive();
                }
                else if (PhotonNetwork.LocalPlayer.ActorNumber == GetLowestActorNumberInTurn())
                {
                    DeckManager.instance.SendGuessToControl();
                }

                isTimeOver = true;
            }
        }
    }


}
