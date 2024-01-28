using System;
using UnityEngine;

public class TouchHandler : MonoBehaviour
{
    private Camera mainCamera;
    private bool isDrag;
    private Vector2 mousePostion;

    public Action<Vector2> OnTouch { get; set; }
    public Action<Vector2> OnDrag { get; set; }
    public Action<Vector2> OnUnTouch { get; set; }

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    public void Update()
    {
        mousePostion = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0) && !isDrag)
        {
            isDrag = true;
            OnTouch?.Invoke(mousePostion);
            return;
        }
        if (Input.GetMouseButtonUp(0) && isDrag)
        {
            isDrag = false;
            OnDrag?.Invoke(mousePostion);
            OnUnTouch?.Invoke(mousePostion);
            return;
        }
        if (isDrag)
        {
            OnDrag?.Invoke(mousePostion);
        }
    }
}
