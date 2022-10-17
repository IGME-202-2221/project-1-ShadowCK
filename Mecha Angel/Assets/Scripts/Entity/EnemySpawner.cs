using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance;
    [SerializeField]
    private float enemySpawnInterval = 10f;
    [SerializeField]
    private float meteorSpawnInterval = 3f;
    private CountdownTimer timer;
    [SerializeField]
    private List<GameObject> enemyPrefabs = new List<GameObject>();
    private List<SpawnSetting> enemySpawnSettings = new List<SpawnSetting>()
    {
        // Should be in the same order as the prefabs in enemyPrefabs! Unless we add prefabs by code
        new SpawnSetting("Scout",45),
        new SpawnSetting("Destroyer",25),
        new SpawnSetting("Castle",10),
        new SpawnSetting("Voodoo",15),
        new SpawnSetting("Venom",20)
    };
    [SerializeField]
    private List<GameObject> meteorPrefabs = new List<GameObject>();
    private List<SpawnSetting> meteorSpawnSettings = new List<SpawnSetting>
    {
        new SpawnSetting("Meteor",80),
        new SpawnSetting("HugeMeteor",20)
    };
    [SerializeField]
    private List<GameObject> enemySpawnPoints = new List<GameObject>();
    private List<GameObject> meteorSpawnPoints = new List<GameObject>();

    private bool spawnEnemy;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }
    void Start()
    {
        InitializeSpawnSettings(enemyPrefabs, enemySpawnSettings);
        InitializeSpawnSettings(meteorPrefabs, meteorSpawnSettings);

        // TODO: Adds all properly named spawn points in the scene to the list
        //       I don't really know how to get certain objects properly in Unity
        //foreach (GameObject go in SceneManager.GetActiveScene().GetRootGameObjects())
        //{
        //    if (go.name.ToLower().Contains("spawnpoint"))
        //    {
        //        enemySpawnPoints.Add(go);
        //    }
        //}

        Game.CameraSettings camera = Game.instance.mainCamera;
        float widthF = camera.width * 0.03f;
        float heightF = camera.height * 0.03f;
        Transform emptyParent = new GameObject("MeteorSpawnPoints").transform;
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                float x = 0;
                float y = 0;
                switch (Random.Range(1, 4))
                {
                    case 1:
                        x = Random.Range(camera.left, camera.right);
                        y = Random.Range(camera.top - 2 * heightF, camera.top - heightF);
                        break;
                    case 2:
                        x = Random.Range(camera.left, camera.right);
                        y = Random.Range(camera.bottom + heightF, camera.bottom + 2 * heightF);
                        break;
                    case 3:
                        x = Random.Range(camera.left + widthF, camera.left + 2 * widthF);
                        y = Random.Range(camera.top, camera.bottom);
                        break;
                    case 4:
                        x = Random.Range(camera.right - 2 * widthF, camera.right - widthF);
                        y = Random.Range(camera.top, camera.bottom);
                        break;
                }
                GameObject spawnPoint = new GameObject($"meteorSpawnPoint{i * 5 + j}");
                spawnPoint.transform.parent = emptyParent;
                spawnPoint.transform.position = new Vector3(x, y, 0);
                meteorSpawnPoints.Add(spawnPoint);
            }
        }

        timer = new CountdownTimer(System.TimeSpan.FromSeconds(enemySpawnInterval));
        timer.OnEnd += SpawnEnemies;
    }

    private void SpawnEnemies()
    {
        spawnEnemy = !spawnEnemy;
        if (spawnEnemy)
        {
            GenerateEntity(enemySpawnSettings, enemySpawnPoints);
            timer.Reset(System.TimeSpan.FromSeconds(meteorSpawnInterval));
        }
        else
        {
            GenerateEntity(meteorSpawnSettings, meteorSpawnPoints);
            timer.Reset(System.TimeSpan.FromSeconds(enemySpawnInterval));
        }
    }

    private void InitializeSpawnSettings(List<GameObject> prefabs, List<SpawnSetting> settings)
    {
        // Computing weights into chances (percentage)
        float weight = 0;
        float chance = 0;
        for (int i = 0; i < settings.Count; i++)
        {
            weight += settings[i].weight;
        }
        for (int i = 0; i < settings.Count; i++)
        {
            SpawnSetting setting = settings[i];
            chance += setting.weight / weight * 100;
            setting.chance = chance;
            setting.Prefab = (prefabs[i]);
        }
    }

    /// <summary>
    /// Spawns entities in all spawn points
    /// </summary>
    /// <param name="list"></param>
    private void GenerateEntity(List<SpawnSetting> list, List<GameObject> spawnPoints)
    {
        foreach (GameObject spawnPoint in spawnPoints)
        {
            float chance = Random.Range(minInclusive: 0, 100);
            // TODO: Algorithm can be improved.
            for (int i = 0; i < list.Count; i++)
                // Chance matches target's spawn rate
                if (chance <= list[i].chance)
                {
                    Instantiate(list[i].Prefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
                    break;
                }
                else
                {
                    continue;
                }
        }
    }

    private class SpawnSetting
    {
        private static int currentIndex = 0;

        private GameObject prefab;
        public string name;
        public int index;
        public float weight;
        public float chance;
        public SpawnSetting(string name, float weight)
        {
            index = currentIndex++;
            this.name = name;
            this.weight = weight;
        }

        public GameObject Prefab
        {
            get => prefab;
            set => prefab = value;
        }
    }

    void Update()
    {
        timer.Update(true);
    }

    private void OnDrawGizmos()
    {
        foreach (GameObject spawnPoint in enemySpawnPoints)
        {
            Gizmos.DrawWireSphere(spawnPoint.transform.position, 1);
        }
    }

    public float NextWaveCountdown()
    {
        return (float)timer.TimeLeft.TotalSeconds;
    }
}
