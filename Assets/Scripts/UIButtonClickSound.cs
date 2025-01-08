using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonClickSound : MonoBehaviour
{
    private Button btn;

    private void Start()
    {
        if (TryGetComponent(out btn))
        {
            btn.onClick.AddListener(() =>
                AudioPlayer.instance.PlayAudio(AudioName.UIButtonClick)
            );
        }

    }
}
