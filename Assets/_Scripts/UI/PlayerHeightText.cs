using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerHeightText : MonoBehaviour
{
    [SerializeField]
    private Player player;

    [SerializeField]
    private TextMeshProUGUI text;

    [SerializeField]
    private Image background;

    private Color startColor;

    private void Awake()
    {
        startColor = background.color;
    }

    private void Update()
    {
        float height = player.YPosition - player.YBottomLimit;
        text.text = $"{height:F0}m";
        text.color = height < 10 ? Color.red : Color.white;
        background.color = Color.Lerp(startColor, Color.black, height / 1000f);
    }
}
