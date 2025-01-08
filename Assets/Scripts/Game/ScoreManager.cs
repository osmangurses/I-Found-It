using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;


    public TextMeshProUGUI team1_score_tmp, team2_score_tmp;


    public int team1_score, team2_score;
    private void Awake()
    {
        instance = this;
    }
    public void ChangeScore(int teamNumber,int addToScore)
    {
        if (teamNumber==1)
        {
            team1_score += addToScore;
            team1_score_tmp.transform.DOScale(Vector3.zero, 0.2f).OnComplete(() =>
            {
                team1_score_tmp.text = team1_score.ToString();
                team1_score_tmp.transform.DOScale(Vector3.one,0.2f);
            });
        }
        else if (teamNumber == 2)
        {
            team2_score += addToScore;
            team2_score_tmp.transform.DOScale(Vector3.zero, 0.2f).OnComplete(() =>
            {
                team2_score_tmp.text = team2_score.ToString();
                team2_score_tmp.transform.DOScale(Vector3.one, 0.2f);
            });
        }
    }
}
