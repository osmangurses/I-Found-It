using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class SpriteGenerator : MonoBehaviourPun
{
    public static SpriteGenerator instance;

    [Header("References")]
    public GameObject parentObject; // Grid Layouta sahip parent obje
    public GameObject prefab;       // Prefab objesi
    public List<SpriteCategory> categories; // Kategorilere ayrýlmýþ sprite listesi

    public int spriteCountOnPanel = 40;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        spriteCountOnPanel = CustomRoomSettings.cardCount;
        if (spriteCountOnPanel > 35) { parentObject.GetComponent<GridLayoutGroup>().cellSize = new Vector2(100, 100); }
        else if (spriteCountOnPanel > 25) { parentObject.GetComponent<GridLayoutGroup>().cellSize = new Vector2(119.25f, 100); }
        else { parentObject.GetComponent<GridLayoutGroup>().cellSize = new Vector2(180, 100); }
    }

    public void SpawnSprites()
    {
        if (PhotonNetwork.IsMasterClient) // Rastgele sprite'larý yalnýzca MasterClient belirler
        {
            List<int> spriteIndices = GenerateRandomSpriteIndices();
            photonView.RPC("SyncSpawnSprites", RpcTarget.All, spriteIndices.ToArray());
        }
    }

    private List<int> GenerateRandomSpriteIndices()
    {
        List<int> randomIndices = new List<int>();
        List<Sprite> selectedSprites = new List<Sprite>();

        // Toplam seçilen sprite sayýsýný takip eden sayaç
        int totalSelectedCount = 0;

        // Seçimlerin kategorilere göre dengeli olmasý için bir kategoriler listesi karýþtýrýlýr
        List<SpriteCategory> shuffledCategories = new List<SpriteCategory>(categories);
        Shuffle(shuffledCategories);

        while (totalSelectedCount < spriteCountOnPanel)
        {
            foreach (var category in shuffledCategories)
            {
                if (totalSelectedCount >= spriteCountOnPanel)
                    break;

                List<Sprite> shuffledCategorySprites = new List<Sprite>(category.sprites);
                Shuffle(shuffledCategorySprites); // Kategorideki sprite'larý karýþtýr

                // Her kategoriden 0 ile 5 arasýnda rastgele bir sayý kadar sprite seç
                int maxSelectable = Mathf.Min(5, spriteCountOnPanel - totalSelectedCount, shuffledCategorySprites.Count);
                int spritesToSelect = Random.Range(0, maxSelectable + 1);

                for (int i = 0; i < spritesToSelect; i++)
                {
                    Sprite selectedSprite = shuffledCategorySprites[0];
                    shuffledCategorySprites.RemoveAt(0);

                    // Seçilen sprite'ýn daha önce eklenip eklenmediðini kontrol et
                    if (!selectedSprites.Contains(selectedSprite))
                    {
                        selectedSprites.Add(selectedSprite);
                        totalSelectedCount++;
                    }
                }
            }
        }

        Shuffle(selectedSprites); // Tüm seçilen sprite'larý karýþtýr

        // Seçilen sprite'larý indekslerine dönüþtür
        for (int i = 0; i < selectedSprites.Count; i++)
        {
            randomIndices.Add(GetSpriteIndex(selectedSprites[i]));
        }

        return randomIndices;
    }



    private int GetSpriteIndex(Sprite sprite)
    {
        for (int i = 0; i < categories.Count; i++)
        {
            int index = categories[i].sprites.IndexOf(sprite);
            if (index >= 0)
            {
                return index + GetCategoryOffset(i);
            }
        }
        return -1;
    }

    private int GetCategoryOffset(int categoryIndex)
    {
        int offset = 0;
        for (int i = 0; i < categoryIndex; i++)
        {
            offset += categories[i].sprites.Count;
        }
        return offset;
    }

    private void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    [PunRPC]
    void SyncSpawnSprites(int[] spriteIndices)
    {
        spriteCountOnPanel = CustomRoomSettings.cardCount;
        if (spriteCountOnPanel > 35) { parentObject.GetComponent<GridLayoutGroup>().cellSize = new Vector2(100, 100); }
        else if (spriteCountOnPanel > 25) { parentObject.GetComponent<GridLayoutGroup>().cellSize = new Vector2(119.25f, 100); }
        else { parentObject.GetComponent<GridLayoutGroup>().cellSize = new Vector2(180, 100); }
        StartCoroutine(SpawnIcons(spriteIndices));
    }

    IEnumerator SpawnIcons(int[] spriteIndices)
    {
        for (int i = 0; i < spriteIndices.Length; i++)
        {
            if (spriteIndices.Length == 0)
            {
                Debug.LogWarning("Yeterli sprite yok!");
                yield break;
            }

            GameObject iconObject = Instantiate(prefab, parentObject.transform);
            iconObject.GetComponent<Image>().DOFade(0, 0f);
            iconObject.GetComponent<Image>().DOFade(1, 0.2f);
            IconInfo iconInfo = iconObject.GetComponent<IconInfo>();

            Sprite selectedSprite = FindSpriteByIndex(spriteIndices[i]);
            iconInfo.icon.sprite = selectedSprite;
            iconInfo.icon.DOFade(0, 0f);
            iconInfo.icon.DOFade(1, 0.2f);
            iconInfo.number = spriteIndices[i];
            DeckManager.instance.icons_on_game.Add(iconInfo);
            yield return new WaitForSeconds(0.2f);
        }

        foreach (var item in DeckManager.instance.icons_on_game)
        {
            item.GetComponent<LayoutElement>().ignoreLayout = true;
        }
    }

    private Sprite FindSpriteByIndex(int index)
    {
        foreach (var category in categories)
        {
            if (index < category.sprites.Count)
            {
                return category.sprites[index];
            }
            index -= category.sprites.Count;
        }
        return null;
    }
}

[System.Serializable]
public class SpriteCategory
{
    public List<Sprite> sprites;
    public string categoryName;
}
