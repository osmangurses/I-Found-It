using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using System.Linq;
using DG.Tweening;

public class DeckManager : MonoBehaviourPunCallbacks
{
    public static DeckManager instance;

    public Button send_deck_button;
    public Button send_guess_button;
    public InputField hint_input;

    public List<IconInfo> icons_on_game = new List<IconInfo>();
    public int card_count_on_deck = 0;
    public List<CardOnDeck> card_on_deck = new List<CardOnDeck>();
    public int[] selected_card_numbers = new int[5];
    public string selected_hint;
    [SerializeField] private GameObject deck_card_prefab;
    [SerializeField] private GameObject deck_panel;
    [SerializeField] private GameObject sent_deck_panel;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        send_deck_button.onClick.AddListener(SendDeckToPredictive);
        send_guess_button.onClick.AddListener(SendGuessToControl);
    }

    private Sprite GetSpriteByNumber(int number)
    {
        int cumulativeIndex = 0;

        foreach (var category in SpriteGenerator.instance.categories)
        {
            if (number < cumulativeIndex + category.sprites.Count)
            {
                return category.sprites[number - cumulativeIndex];
            }
            cumulativeIndex += category.sprites.Count;
        }

        Debug.LogWarning($"Sprite with number {number} not found.");
        return null;
    }

    public void AddNewCardToDeck(int number)
    {
        if (card_count_on_deck < 5)
        {
            if (Player.instance.statu == PlayerStatu.T1_Predictive || Player.instance.statu == PlayerStatu.T2_Predictive)
            {
                photonView.RPC(nameof(RPC_AddNewCardToDeck), RpcTarget.AllBuffered, number);
            }
            else
            {
                AudioPlayer.instance.PlayAudio(AudioName.AddCard);
                card_count_on_deck++;
                GameObject tempCard = Instantiate(deck_card_prefab, deck_panel.transform);
                tempCard.transform.GetChild(0).GetComponent<Image>().sprite = GetSpriteByNumber(number);
                tempCard.GetComponent<CardOnDeck>().number = number;
                card_on_deck.Add(tempCard.GetComponent<CardOnDeck>());
                tempCard.transform.localScale = Vector3.zero;
                tempCard.transform.DOScale(Vector3.one, 0.2f);
                foreach (var item in icons_on_game)
                {
                    if (item.number == number)
                    {
                        item.GetComponent<Button>().interactable = false;
                    }
                }
            }
        }
    }

    [PunRPC]
    public void RPC_AddNewCardToDeck(int number)
    {
        AudioPlayer.instance.PlayAudio(AudioName.AddCard);
        if (card_count_on_deck < 5)
        {
            card_count_on_deck++;
            GameObject tempCard = Instantiate(deck_card_prefab, deck_panel.transform);
            tempCard.transform.GetChild(0).GetComponent<Image>().sprite = GetSpriteByNumber(number);
            tempCard.GetComponent<CardOnDeck>().number = number;
            card_on_deck.Add(tempCard.GetComponent<CardOnDeck>());
            tempCard.transform.localScale = Vector3.zero;
            tempCard.transform.DOScale(Vector3.one, 0.2f);
            foreach (var item in icons_on_game)
            {
                if (item.number == number)
                {
                    item.GetComponent<Button>().interactable = false;
                }
            }
        }
    }

    public void RemoveCardFromDeck(int number)
    {
        if (Player.instance.statu == PlayerStatu.T1_Predictive || Player.instance.statu == PlayerStatu.T2_Predictive)
        {
            photonView.RPC(nameof(RPC_RemoveCardFromDeck), RpcTarget.AllBuffered, number);
        }
        else
        {
            AudioPlayer.instance.PlayAudio(AudioName.RemoveCard);
            List<CardOnDeck> cardsToRemove = new List<CardOnDeck>();

            foreach (var gamecard in icons_on_game)
            {
                if (gamecard.number==number)
                {
                    gamecard.GetComponent<Button>().interactable = true;
                }
            }
            foreach (var removecard in card_on_deck)
            {
                if (removecard.number == number)
                {
                    cardsToRemove.Add(removecard);
                }
            }
            foreach (var card in cardsToRemove)
            {
                card_on_deck.Remove(card);
                card.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
                Destroy(card.gameObject)
                );
            }

            card_count_on_deck--;
        }
    }
    [PunRPC]
    public void RPC_RemoveCardFromDeck(int number)
    {
        List<CardOnDeck> cardsToRemove = new List<CardOnDeck>();
        AudioPlayer.instance.PlayAudio(AudioName.RemoveCard);

        foreach (var gamecard in icons_on_game)
        {
            if (gamecard.number == number)
            {
                gamecard.GetComponent<Button>().interactable = true;
            }
        }
        foreach (var removecard in card_on_deck)
        {
            if (removecard.number == number)
            {
                cardsToRemove.Add(removecard);
            }
        }
        foreach (var card in cardsToRemove)
        {
            card_on_deck.Remove(card);
            card.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
            Destroy(card.gameObject)
            );
        }

        card_count_on_deck--;
    }
    public void TransferDeckToSentPanel()
    {
        while (deck_panel.transform.childCount > 1)
        {
            deck_panel.transform.GetChild(1).transform.SetParent(sent_deck_panel.transform);
        }
    }
    public void SendDeckToPredictive()
    {
        if (TurnManager.instance.IsMyTurn())
        {
            int[] deck = { -1, -1, -1, -1, -1 };
            for (int i = 0; i < card_on_deck.Count; i++)
            {
                deck[i] = card_on_deck[i].number;
            }
            photonView.RPC(nameof(SendDeckToClients), RpcTarget.All, deck, hint_input.text, card_count_on_deck);
        }
        TransferDeckToSentPanel();
    }
    public void SendGuessToControl()
    {
        if (TurnManager.instance.IsMyTurn())
        {
            int[] guess_numbers = { -1, -1, -1, -1, -1 };
            for (int i = 0; i < card_on_deck.Count; i++)
            {
                guess_numbers[i] = card_on_deck[i].number;
            }
            var team = (TurnManager.instance.turn == PlayerStatu.T1_Predictive) ? "1" : "2";
            photonView.RPC(nameof(ControlGuess), RpcTarget.All, guess_numbers,team);
        }
    }

    [PunRPC]
    void SendDeckToClients(int[] card_numbers, string hint,int cardCount)
    {
        for (int i = 0; i < card_numbers.Length; i++)
        {
            selected_card_numbers[i] = card_numbers[i];
        }
        if (CustomRoomSettings.ShowCardCount)
        {
            hint =hint + $" [{cardCount}]";
        }
        HintManager.instance.ShowHint(hint);
        card_on_deck.Clear();
        TurnManager.instance.NextTurn();
        foreach (IconInfo iconInfo in icons_on_game)
        {
            iconInfo.GetComponent<Button>().interactable = true;
        }
        card_count_on_deck = 0;
    }

    [PunRPC]
    void ControlGuess(int[] card_numbers, string team)
    {
        StartCoroutine(ControlGuessCoroutine(card_numbers,TurnManager.instance.IsMyTurn(),team));
    }

    IEnumerator ControlGuessCoroutine(int[] card_numbers,bool isMyTurn,string team)
    {
        HintManager.instance.HideHint();
        bool isAllTrue = true;
        int point = card_numbers.Count(num => num != -1);

        foreach (int number in card_numbers)
        {
            if (!selected_card_numbers.Contains(number) || card_numbers.Count(num => num != -1) != selected_card_numbers.Count(num => num != -1))
            {
                isAllTrue = false;
                break;
            }
        }

        Color resultColor = (isAllTrue) ? Color.green : Color.red;
        if (isAllTrue)
        {
            AudioPlayer.instance.PlayAudio(AudioName.Correct);
            foreach (var item in card_on_deck)
            {
                item.GetComponent<LayoutElement>().ignoreLayout = true;
                item.transform.SetParent(item.transform.root);
                item.gameObject.GetComponent<Image>().color=resultColor;

                if (team=="1")
                {
                    item.transform.DOMove(ScoreManager.instance.team1_score_tmp.transform.position, 0.5f).SetDelay(0.2f);
                    item.transform.DOScale(Vector3.zero, 0.5f).SetDelay(0.2f);
                }
                else
                {
                    item.transform.DOMove(ScoreManager.instance.team2_score_tmp.transform.position, 0.5f).SetDelay(0.2f);
                    item.transform.DOScale(Vector3.zero, 0.5f).SetDelay(0.2f);
                }
            }

            // 1 saniye gecikme
            yield return new WaitForSeconds(1f);
        }
        else
        {
            AudioPlayer.instance.PlayAudio(AudioName.Incorrect);
            foreach (var item in card_on_deck)
            {
                item.gameObject.GetComponent<Image>().color = resultColor;
            }

            // 1 saniye gecikme
            yield return new WaitForSeconds(1f);
        }

        int scoremult = isAllTrue ? 1 : 0;

        if (team == "1")
        {
            ScoreManager.instance.ChangeScore(1, point * scoremult);
        }
        else
        {
            ScoreManager.instance.ChangeScore(2, point * scoremult);
        }

        if (isAllTrue)
        {
            for (int i = icons_on_game.Count - 1; i >= 0; i--)
            {
                IconInfo iconInfo = icons_on_game[i];
                if (card_numbers.Contains(iconInfo.number))
                {
                    icons_on_game.RemoveAt(i);
                    iconInfo.gameObject.SetActive(false);
                }
            }
        }

        // Deck'i temizleme iþlemleri
        for (int i = 0; i < sent_deck_panel.transform.childCount; i++)
        {
            Destroy(sent_deck_panel.transform.GetChild(i).gameObject);
        }

        foreach (IconInfo iconInfo in icons_on_game)
        {
            iconInfo.GetComponent<Button>().interactable = true;
        }

        for (int i = card_on_deck.Count - 1; i >= 0; i--)
        {
            Destroy(card_on_deck[i].gameObject);
            card_on_deck.RemoveAt(i);
        }

        List<Transform> children = new List<Transform>();
        for (int i = 0; i < sent_deck_panel.transform.childCount; i++)
        {
            children.Add(sent_deck_panel.transform.GetChild(i));
        }
        foreach (Transform child in children)
        {
            Destroy(child.gameObject);
        }

        // Tur sonu iþlemleri
        HintManager.instance.HideHint();
        if (isMyTurn)
        {
            TurnManager.instance.NextTurn();
        }
        card_count_on_deck = 0;
        card_on_deck.Clear();

        for (int i = 0; i < selected_card_numbers.Count(); i++)
        {
            selected_card_numbers[i] = -1;
        }
    }


}