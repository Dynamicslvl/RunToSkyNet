using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dasis.Common;

[RequireComponent(typeof(Camera))]
public class CameraFollower : TwoDimensionObject
{
    [SerializeField]
    private float yBottomLimit;

    [SerializeField]
    private Transform target;

    [SerializeField]
    private Vector3 offset = new(0, 0, -10);

    [SerializeField]
    private float smoothTime = 0.25f;

    private Vector3 velocity;

    public bool IsEnable { get; set; }
    public float YBottomLimit => yBottomLimit;

    private void Awake()
    {
        IsEnable = true;
    }

    public void Update()
    {
        if (!IsEnable) return;
        Vector3 targetPos = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
        if (YPosition < yBottomLimit)
        {
            YPosition = yBottomLimit;
        }
    }
}
