using Dasis.DesignPattern;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private int score;

    public int Score
    {
        get { return score; }
        set
        {
            score = value;
            OnScoreChange?.Invoke(score);
        }
    }

    public Action<int> OnScoreChange { get; set; }

    public override void OnInitialization()
    {
        Application.targetFrameRate = 120;
    }
    private void Start()
    {
        
    }
}
