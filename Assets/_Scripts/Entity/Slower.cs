using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slower : SpawnObject
{
    [SerializeField]
    private float fallSpeed;

    private void Update()
    {
        YPosition -= Time.deltaTime * fallSpeed;
    }
}
