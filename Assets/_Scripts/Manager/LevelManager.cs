using Dasis.DesignPattern;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField]
    private Player player;

    [SerializeField]
    private Enemy enemy;

    [SerializeField]
    private Mattress mattress;

    [SerializeField]
    private GameObject spawnerObject;

    private List<Spawner> spawners;

    public override void OnInitialization()
    {
        spawners = spawnerObject.GetComponents<Spawner>().ToList();
    }

    public void LoadLevel()
    {
        SetLevelActive(true);
        GameManager.Instance.Score = 0;
        ComboManager.Instance.ResetCombo();
        player.Initialize();
        enemy.Initialize(player.XYPosition + Vector2.down * 30);
        mattress.Initialize();
        foreach (var spawner in spawners)
        {
            spawner.InitialSpawn();
        }
    }

    public void ClearLevel()
    {
        SetLevelActive(false);
    }

    public void SetLevelActive(bool active)
    {
        player.gameObject.SetActive(active);
        enemy.gameObject.SetActive(active);
        mattress.gameObject.SetActive(active);
        spawnerObject.SetActive(active);
    }
}
