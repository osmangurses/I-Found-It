using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Photon.Pun;
using Photon.Realtime;
using System.Runtime.InteropServices;

public class FriendPlayManager : MonoBehaviourPunCallbacks
{
    [SerializeField] Slider clue_time_slider, predictivation_time_slider, card_count_slider;
    [SerializeField] TextMeshProUGUI room_code_tmp, clue_time_tmp, predictivation_time_tmp, copied_tmp,card_count_tmp;
    [SerializeField] Toggle show_card_count_toggle;
    [SerializeField] Button show_room_code_button, copy_room_code_button, create_room_button,join_room_button;
    [SerializeField] InputField room_code_input;

    private bool isRoomCodeVisible = false; // Room code visibility toggle


    [DllImport("__Internal")]
    private static extern void CopyToClipboard(string text);

    private void Start()
    {
        // Rastgele oda kodu oluþtur ve UI'ye yaz
        CustomRoomSettings.GenerateRandomRoomCode();
        room_code_tmp.text = new string('•', 6); // Pin olarak baþlat

        // Olay dinleyicilerini ekle
        create_room_button.onClick.AddListener(CreateCustomRoom);
        join_room_button.onClick.AddListener(JoinCustomRoom);
        copy_room_code_button.onClick.AddListener(CopyRoomCodeToClipboard);
        show_room_code_button.onClick.AddListener(ToggleRoomCodeVisibility);
        clue_time_slider.onValueChanged.AddListener(OnClueTimeChanged);
        card_count_slider.onValueChanged.AddListener(OnCardCountChanged);
        predictivation_time_slider.onValueChanged.AddListener(OnPredictivationTimeChanged);
        show_card_count_toggle.onValueChanged.AddListener(OnShowCardCountChanged);

        // Baþlangýç deðerlerini kaydet
        SaveRoomSettings();
    }
    void JoinCustomRoom()
    {
        PhotonNetwork.JoinRoom(room_code_input.text);
    }
    void CreateCustomRoom()
    {
        SaveRoomSettings();
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 8;
        roomOptions.IsVisible = false;
        PhotonNetwork.CreateRoom(CustomRoomSettings.RoomCode.ToString(),roomOptions);

    }
    private void SaveRoomSettings()
    {
        CustomRoomSettings.ClueTime = clue_time_slider.value;
        CustomRoomSettings.PredictivationTime = predictivation_time_slider.value;
        CustomRoomSettings.cardCount = (int)card_count_slider.value;
        CustomRoomSettings.ShowCardCount = show_card_count_toggle.isOn;
        CustomRoomSettings.IsCustomRoom = true;

        Debug.Log("Room settings updated:");
        Debug.Log($"Room Code: {CustomRoomSettings.RoomCode}");
        Debug.Log($"Clue Time: {CustomRoomSettings.ClueTime}");
        Debug.Log($"Predictivation Time: {CustomRoomSettings.PredictivationTime}");
        Debug.Log($"Show Card Count: {CustomRoomSettings.ShowCardCount}");
    }

    private void CopyRoomCodeToClipboard()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
            CopyToClipboard(CustomRoomSettings.RoomCode.ToString());
#else
        GUIUtility.systemCopyBuffer = CustomRoomSettings.RoomCode.ToString();
#endif

        copied_tmp.gameObject.SetActive(true);
        copied_tmp.DOFade(1, 0.5f).OnComplete(() =>
        {
            copied_tmp.DOFade(0, 0.5f).SetDelay(1).OnComplete(() => copied_tmp.gameObject.SetActive(false));
        });
    }

    private void ToggleRoomCodeVisibility()
    {
        isRoomCodeVisible = !isRoomCodeVisible;
        room_code_tmp.text = isRoomCodeVisible ? CustomRoomSettings.RoomCode.ToString() : new string('•', 6);
    }

    private void OnClueTimeChanged(float value)
    {
        clue_time_tmp.text = $"{value}s";
        SaveRoomSettings();
    }
    private void OnCardCountChanged(float value)
    {
        card_count_tmp.text = $"{value}";
        SaveRoomSettings();
    }

    private void OnPredictivationTimeChanged(float value)
    {
        predictivation_time_tmp.text = $"{value}s";
        SaveRoomSettings();
    }

    private void OnShowCardCountChanged(bool isOn)
    {
        SaveRoomSettings();
    }
}
