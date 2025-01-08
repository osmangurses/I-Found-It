using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class HintManager : MonoBehaviour
{
    public static HintManager instance;
    public string hint_string;
    public Text hint_tmp;
    private void Awake()
    {
        instance = this;
    }
    public void ShowHint(string _hint_string)
    {
        hint_string = _hint_string;
        hint_tmp.DOText(hint_string,0.5f);
    }
    public void HideHint()
    {
        hint_tmp.DOText("",0.2f);
    }
}
