using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlayLanguageButton : MonoBehaviour
{
    public Text howToPlayText; // HowToPlay panelindeki metin
    public Sprite[] flagSprites; // �lkelerin bayrak g�rselleri (S�ras�yla: �ngilizce, T�rk�e, Almanca, �spanyolca, Portekizce)

    private string[] howToPlayTexts = new string[]
    {
        "How to Play\nCluer gives a hint, Predictive tries to recreate the same deck. Cluer selects up to 5 images and writes a one-word hint. Predictive uses the hint to recreate the deck. When time runs out, the next player takes their turn. Predictive earns points equal to the number of cards in the deck if they fully match Cluer's deck. Have fun!",
        "Nas�l Oynan�r\nCluer ipucu verir, Predictive ayn� desteyi olu�turmaya �al���r. Cluer en fazla 5 g�rsel se�ip tek kelimelik ipucu yazar, Predictive bu ipucuyla desteyi yeniden olu�turur. S�re bitince s�radaki oyuncuya ge�ilir. Predictive, Cluer�in destesini tam do�ru olu�turdu�unda destedeki kart say�s� kadar puan kazan�r. �yi e�lenceler!",
        "Wie man spielt\nCluer gibt einen Hinweis, Predictive versucht, das gleiche Deck zu rekonstruieren. Cluer w�hlt bis zu 5 Bilder aus und schreibt einen ein-Wort-Hinweis. Predictive verwendet den Hinweis, um das Deck neu zu erstellen. Wenn die Zeit abl�uft, ist der n�chste Spieler an der Reihe. Predictive erh�lt Punkte entsprechend der Anzahl der Karten im Deck, wenn es das Deck von Cluer vollst�ndig nachbildet. Viel Spa�!",
        "C�mo jugar\nCluer da una pista, Predictive intenta recrear el mismo mazo. Cluer selecciona hasta 5 im�genes y escribe una pista de una palabra. Predictive utiliza la pista para recrear el mazo. Cuando se acaba el tiempo, el siguiente jugador toma su turno. Predictive gana puntos igual al n�mero de cartas en el mazo si coincide completamente con el mazo de Cluer. �Divi�rtete!",
        "Como Jogar\nCluer d� uma dica, Predictive tenta recriar o mesmo baralho. Cluer seleciona at� 5 imagens e escreve uma dica de uma palavra. Predictive usa a dica para recriar o baralho. Quando o tempo acabar, o pr�ximo jogador assume sua vez. Predictive ganha pontos iguais ao n�mero de cartas no baralho se corresponder completamente ao baralho de Cluer. Divirta-se!"
    };

    private int currentLanguageIndex;
    private Image flagButtonImage; // Button �zerindeki bayrak resmi

    void Start()
    {
        flagButtonImage = GetComponent<Image>();
        // Son se�ilen dili y�kle
        currentLanguageIndex = PlayerPrefs.GetInt("SelectedLanguage", 0);
        UpdateLanguage();
        GetComponent<Button>().onClick.AddListener(SwitchLanguage);
    }

    public void SwitchLanguage()
    {
        currentLanguageIndex = (currentLanguageIndex + 1) % howToPlayTexts.Length;
        PlayerPrefs.SetInt("SelectedLanguage", currentLanguageIndex);
        UpdateLanguage();
    }

    private void UpdateLanguage()
    {
        howToPlayText.text = howToPlayTexts[currentLanguageIndex];
        flagButtonImage.sprite = flagSprites[currentLanguageIndex];
    }
}
