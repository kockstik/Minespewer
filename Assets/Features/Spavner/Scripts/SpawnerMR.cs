using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

public class SpawnerMR : MonoBehaviour
{
    [Range(0, 100)]
    [SerializeField] private int botCount = 10;
    [SerializeField] private Player playerPrefab;
    [SerializeField] private Bot botPrefab;

    private Player polledPlayer;
    private List<Bot> polledBots = new List<Bot>();
    private List<SpawnPoint> points = new List<SpawnPoint>();
    private EntityManager entityManager;

    private bool spawnPlayerFlag = false;

    void Start()
    {
        entityManager = FindAnyObjectByType<EntityManager>();
        points = GetComponentsInChildren<SpawnPoint>().ToList();
        for (int i = 0; i < botCount; i++)
        {
            Bot bot = Instantiate(botPrefab, new Vector3(0, -100, 0), Quaternion.identity).GetComponent<Bot>();
            bot.gameObject.SetActive(false);
            bot.entityManager = entityManager;
            polledBots.Add(bot);
        }

        //polledPlayer = Instantiate(playerPrefab, transform.position, Quaternion.identity).GetComponent<Player>();
        //polledPlayer.gameObject.SetActive(false);
        //SpawnPlayer();
    }


    void Update()
    {
        SpawnBot();

        if (!spawnPlayerFlag)
            return;

        if (polledPlayer == null)
            return;

        var point = points[Random.Range(0, points.Count)];
        if (point != null && point.canSpawn)
        {
            point.AddSpawn(polledPlayer);
            spawnPlayerFlag = false;
        }
    }

    private void SpawnPlayer()
    {
        spawnPlayerFlag = true;
    }

    private void SpawnBot()
    {
        if (polledBots.Count > 0)
        {
            var bot = polledBots[0];
            polledBots.RemoveAt(0);
            var point = points[Random.Range(0, points.Count)];
            point.AddSpawn(bot);
        }
    }

    public void AddPooledBot(Bot bot)
    {
        polledBots.Add(bot);
    }
}
