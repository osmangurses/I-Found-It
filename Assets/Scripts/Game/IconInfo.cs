using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconInfo : MonoBehaviour
{
    public Image icon;
    public int number = 0;
    private Button btn;

    private void Awake()
    {
        btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(AddIconToDeck);
        }
    }

    void AddIconToDeck()
    {
        if (TurnManager.instance.IsMyTurn())
        {
            if (DeckManager.instance != null && DeckManager.instance.card_count_on_deck < 5)
            {
                DeckManager.instance.AddNewCardToDeck(number);
            }
        }
    }
    public void EnterSound()
    {
        AudioPlayer.instance.PlayAudio(AudioName.EnterCard);
    }
}
