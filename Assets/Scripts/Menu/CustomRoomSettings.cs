using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CustomRoomSettings
{
    public static int RoomCode;
    public static float ClueTime = 100;
    public static float PredictivationTime = 100;
    public static int cardCount = 25;
    public static bool ShowCardCount = false;
    public static bool IsCustomRoom = false;

    public static void GenerateRandomRoomCode()
    {
        RoomCode = Random.Range(100000, 1000000);
    }
}
