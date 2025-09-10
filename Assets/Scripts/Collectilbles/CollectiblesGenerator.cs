using UnityEngine;
using System.Collections;
using Game.Models;
using VContainer;

public class CoinGenerator : MonoBehaviour
{
    public CollectibleData[] collectibleTypes;

    [Header("Spawn Settings")]
    [SerializeField] public float spawnInterval = 2f;
    [SerializeField] public int minRow = 3;
    [SerializeField] public int maxRow = 4;
    [SerializeField] public float minXDistance = 1f;
    [SerializeField] public float maxXDistance = 2f;
    [SerializeField] public float minZDistance = 5f;
    [SerializeField] public float maxZDistance = 10f;

    [Header("Rare Collectible Chances (0..1)")]
    [SerializeField, Range(0, 100)] private int gemChance = 5;
    [SerializeField, Range(0, 100)] private int starChance = 10;

    private float lastZ = 0f;
    private IScoreService scoreService;
    private CollectibleFeedbackManager feedbackManager;

    [Inject]
    public void Construct(IScoreService scoreService, CollectibleFeedbackManager feedbackManager)
    {
        this.scoreService = scoreService;
        this.feedbackManager = feedbackManager;
    }

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private int GetCoinsInRow()
    {
        float roll = Random.value;
        if (roll < 0.30f) return 1;
        else if (roll < 0.60f) return 2;
        else if (roll < 0.85f) return 3;
        else return 4;
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            float zOffset = Random.Range(minZDistance, maxZDistance);
            float rowZ = lastZ + zOffset;

            int roll = Random.Range(0, 100);
            Debug.Log($"Spawn roll: {roll}");

            if (roll < gemChance)
            {
                SpawnSingleRare(CollectibleTypes.Gem, rowZ);
                Debug.Log("Spawned Gem!");
            }
            else if (roll < gemChance + starChance)
            {
                SpawnSingleRare(CollectibleTypes.Star, rowZ);
                Debug.Log("Spawned Star!");
            }
            else
            {
                int coinsInRow = GetCoinsInRow();
                for (int i = 0; i < coinsInRow; i++)
                {
                    float xOffset = Random.Range(minXDistance, maxXDistance);
                    int side = (i % 2 == 0 ? 1 : -1);
                    float x = side * (i / 2 + 1) * xOffset;

                    Vector3 spawnPos = new Vector3(x, 0.45f, rowZ);
                    SpawnCollectible(spawnPos, CollectibleTypes.Coin);
                }
            }

            lastZ = rowZ;
            yield return new WaitForSeconds(spawnInterval);
        }
    }


    private void SpawnSingleRare(CollectibleTypes type, float z)
    {
        Vector3 spawnPos = new Vector3(0, 0.45f, z); // по центру
        SpawnCollectible(spawnPos, type);
        
        if (type == null)
        {
            Debug.LogError($"[SpawnSystem] Missing prefab for {type}");
            return;
        }

    }

    private void SpawnCollectible(Vector3 position, CollectibleTypes forcedType = CollectibleTypes.Coin)
    {
        if (collectibleTypes.Length == 0) return;

        var data = System.Array.Find(collectibleTypes, c => c.Type == forcedType);
        if (data == null)
        {
            Debug.LogWarning($"Collectible type {forcedType} not found in collectibleTypes array!");
            return;
        }

        MyCollectibleScript obj = Instantiate(data.CollectiblePrefab, position, Quaternion.identity);

        if (data.Type == CollectibleTypes.Coin || data.Type == CollectibleTypes.Star || data.Type == CollectibleTypes.Gem)
        {
            if (obj.GetComponent<CollectableRotation>() == null)
                obj.gameObject.AddComponent<CollectableRotation>();
        }


        obj.Init(data, scoreService, feedbackManager);

    }
}
