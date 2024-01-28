using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreField : Field
{
    public override void Initialize()
    {
        GameManager.Instance.OnScoreChange += UpdateDisplay;
    }
}
