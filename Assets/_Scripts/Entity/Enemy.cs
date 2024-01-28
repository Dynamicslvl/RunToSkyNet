using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dasis.Common;

public class Enemy : TwoDimensionObject
{
    [SerializeField]
    private Player player;

    public float Speed => Mathf.Min(20, 5 + GameManager.Instance.Score / 17);

    public void Initialize(Vector2 position)
    {
        XYPosition = position;
    }

    private void Update()
    {
        XYPosition += Speed * Time.deltaTime * (player.XYPosition - XYPosition).normalized;
        if ((XYPosition - player.XYPosition).magnitude > 40)
        {
            XYPosition = player.XYPosition + Random.insideUnitCircle.normalized * 30;
        }
    }
}
