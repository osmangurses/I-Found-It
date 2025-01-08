using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlayLanguageButton : MonoBehaviour
{
    public Text howToPlayText; // HowToPlay panelindeki metin
    public Sprite[] flagSprites; // Ülkelerin bayrak görselleri (Sýrasýyla: Ýngilizce, Türkçe, Almanca, Ýspanyolca, Portekizce)

    private string[] howToPlayTexts = new string[]
    {
        "How to Play\nCluer gives a hint, Predictive tries to recreate the same deck. Cluer selects up to 5 images and writes a one-word hint. Predictive uses the hint to recreate the deck. When time runs out, the next player takes their turn. Predictive earns points equal to the number of cards in the deck if they fully match Cluer's deck. Have fun!",
        "Nasýl Oynanýr\nCluer ipucu verir, Predictive ayný desteyi oluþturmaya çalýþýr. Cluer en fazla 5 görsel seçip tek kelimelik ipucu yazar, Predictive bu ipucuyla desteyi yeniden oluþturur. Süre bitince sýradaki oyuncuya geçilir. Predictive, Cluer’in destesini tam doðru oluþturduðunda destedeki kart sayýsý kadar puan kazanýr. Ýyi eðlenceler!",
        "Wie man spielt\nCluer gibt einen Hinweis, Predictive versucht, das gleiche Deck zu rekonstruieren. Cluer wählt bis zu 5 Bilder aus und schreibt einen ein-Wort-Hinweis. Predictive verwendet den Hinweis, um das Deck neu zu erstellen. Wenn die Zeit abläuft, ist der nächste Spieler an der Reihe. Predictive erhält Punkte entsprechend der Anzahl der Karten im Deck, wenn es das Deck von Cluer vollständig nachbildet. Viel Spaß!",
        "Cómo jugar\nCluer da una pista, Predictive intenta recrear el mismo mazo. Cluer selecciona hasta 5 imágenes y escribe una pista de una palabra. Predictive utiliza la pista para recrear el mazo. Cuando se acaba el tiempo, el siguiente jugador toma su turno. Predictive gana puntos igual al número de cartas en el mazo si coincide completamente con el mazo de Cluer. ¡Diviértete!",
        "Como Jogar\nCluer dá uma dica, Predictive tenta recriar o mesmo baralho. Cluer seleciona até 5 imagens e escreve uma dica de uma palavra. Predictive usa a dica para recriar o baralho. Quando o tempo acabar, o próximo jogador assume sua vez. Predictive ganha pontos iguais ao número de cartas no baralho se corresponder completamente ao baralho de Cluer. Divirta-se!"
    };

    private int currentLanguageIndex;
    private Image flagButtonImage; // Button üzerindeki bayrak resmi

    void Start()
    {
        flagButtonImage = GetComponent<Image>();
        // Son seçilen dili yükle
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
