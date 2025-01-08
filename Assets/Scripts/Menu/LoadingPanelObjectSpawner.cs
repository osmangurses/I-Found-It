using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class LoadingPanelObjectSpawner : MonoBehaviourPunCallbacks
{
    public List<Sprite> spriteList;       // Sprite listesini buraya atayın
    public GameObject objectPrefab;       // RandomMovingObject prefab
    public RectTransform panelRect;       // Panelin RectTransform'u
    public int objectCount = 10;          // Başlangıçta oluşturulacak obje sayısı
    private void Awake()
    {
        transform.localScale = Vector3.one;
    }
    private void Start()
    {
        for (int i = 0; i < objectCount; i++)
        {
            SpawnObject();
        }
    }
    public override void OnJoinedLobby()
    {
        Destroy(this.gameObject);
    }
    void SpawnObject()
    {
        // Panelin sınırları içinde rastgele bir pozisyon belirleyin
        Vector2 randomPosition = new Vector2(
            Random.Range(panelRect.rect.min.x, panelRect.rect.max.x),
            Random.Range(panelRect.rect.min.y, panelRect.rect.max.y)
        );

        // Prefabi oluşturun ve pozisyonunu ayarlayın
        GameObject obj = Instantiate(objectPrefab, panelRect);
        obj.GetComponent<RectTransform>().anchoredPosition = randomPosition;

        // Sprite listesinden rastgele bir sprite seçin ve atayın
        Sprite randomSprite = spriteList[Random.Range(0, spriteList.Count)];
        obj.GetComponent<Image>().sprite = randomSprite;

        // Panel sınırlarını belirlemek için RandomMovingObject scriptine panel bilgisi gönderin
        obj.GetComponent<RandomMovingObject>().panelRect = panelRect;
    }
}

