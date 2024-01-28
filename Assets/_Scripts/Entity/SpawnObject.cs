using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dasis.Common;

public abstract class SpawnObject : TwoDimensionObject
{
    public bool IsRecalled { get; set; }

    public void Spawn(Vector2 position)
    {
        gameObject.SetActive(true);
        Transform.position = position;
        IsRecalled = false;
    }

    public void Recall()
    {
        Hide();
        IsRecalled = true;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public bool IsOutOfRange(Vector2 range, Vector2 ballPosition)
    {
        if (Transform.position.x < ballPosition.x - range.x 
            || Transform.position.x > ballPosition.x + range.x)
            return true;
        if (Transform.position.y < ballPosition.y - range.y 
            || Transform.position.y > ballPosition.y + range.y)
            return true;
        return false;
    }

    public virtual void OnSpawn() { }
    public virtual void OnRecall() { }
    public virtual void OnHide() { }

}
