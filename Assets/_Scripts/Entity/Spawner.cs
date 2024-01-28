using Dasis.Extensions;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;
    public Player player;
    public int minAmount, scorePerAmountInc;
    public float probability;

    private Vector2 InnerRange => CameraSizeInfo.Instance.InnerRange;
    private Vector2 OutterRange => CameraSizeInfo.Instance.OutterRange;
    private readonly List<SpawnObject> spawnObjects = new();
    private int amount;

    public int Amount
    {
        get { return amount; }
        set
        {
            amount = value;
            while (spawnObjects.Count < amount)
            {
                spawnObjects.Add(Instantiate(prefab, transform).GetComponent<SpawnObject>());
            }
        }
    }

    public void InitialSpawn()
    {
        foreach (var obj in spawnObjects)
        {
            obj.Recall();
        }
        Amount = minAmount;
        for (int i = 0; i < amount; i++)
        {
            spawnObjects[i].Spawn(GetRandomSpawnPosition());
        }
    }

    public void Update()
    {
        Amount = minAmount + GameManager.Instance.Score / scorePerAmountInc;
        UpdateSpawnObject();
    }

    public void UpdateSpawnObject()
    {
        for (int i = 0; i < amount; i++)
        {
            var obj = spawnObjects[i];
            if (!obj.IsRecalled && !obj.IsOutOfRange(OutterRange, player.transform.position))
                continue;
            obj.Spawn(GetRandomSpawnPosition());
            if (FastMath.WithChanceOf(probability)) continue;
            obj.Hide();
        }
    }

    public Vector2 GetRandomSpawnPosition()
    {
        Vector2 position = new();
        if (FastMath.WithChanceOf(0.5f))
        {
            position.x = FastMath.WithChanceOf(0.5f)
                ? Random.Range(-OutterRange.x, -InnerRange.x)
                : Random.Range(InnerRange.x, OutterRange.x);
            position.y = Random.Range(-OutterRange.y, OutterRange.y);
        } 
        else
        {
            position.y = FastMath.WithChanceOf(0.5f)
                ? Random.Range(-OutterRange.y, -InnerRange.y)
                : Random.Range(InnerRange.y, OutterRange.y);
            position.x = Random.Range(-OutterRange.x, OutterRange.x);
        }
       
        return (Vector2)player.transform.position + position;
    }
}