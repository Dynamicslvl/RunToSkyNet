using Dasis.DesignPattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSizeInfo : Singleton<CameraSizeInfo>
{
    private Camera mainCamera;

    private Camera MainCamera
    {
        get
        {
            if (mainCamera == null)
            {
                mainCamera = GetComponent<Camera>();
            }
            return mainCamera;
        }
    }

    public Vector2 InnerRange
    {
        get
        {
            float orthographicSize = MainCamera.orthographicSize;
            return new Vector2(MainCamera.aspect * orthographicSize, orthographicSize) + Vector2.one;
        }
    }

    public Vector2 OutterRange
    {
        get
        {
            return InnerRange * 2;
        }
    }
}
