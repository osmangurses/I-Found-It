using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardOnDeck : MonoBehaviour
{
    public int number;
    private Button btn;

    private void Awake()
    {
        btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(RemoveCardFromDeck);
        }
    }

    void RemoveCardFromDeck()
    {
        if (TurnManager.instance.IsMyTurn())
        {
            DeckManager.instance?.RemoveCardFromDeck(number);
        }
    }
    public void EnterSound()
    {
        AudioPlayer.instance.PlayAudio(AudioName.EnterCard);
    }
}