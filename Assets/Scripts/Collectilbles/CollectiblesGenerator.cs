using UnityEngine;
using System.Collections;
using Game.Models;
using VContainer;
using System.Collections.Generic;

public class CoinGenerator : MonoBehaviour
{
    [Header("Collectible Types")]
    [SerializeField] private CollectibleData[] collectiblePresets;


    [Header("Spawn Settings")]
    [SerializeField] private float spawnDelay = 2f;
    [SerializeField] private int minCoinsInLine = 3;
    [SerializeField] private int maxCoinsInLine = 5;
    [SerializeField] private float coinSpacingZ = 1f;
    [SerializeField] private float minZOffset = 5f;
    [SerializeField] private float maxZOffset = 10f;

    [Header("Coin Count Weights")]
    [SerializeField]
    private List<CoinSpawnWeight> coinSpawnWeights = new List<CoinSpawnWeight>
    {
        new CoinSpawnWeight { coinCount = 1, weight = 40 },
        new CoinSpawnWeight { coinCount = 2, weight = 30 },
        new CoinSpawnWeight { coinCount = 3, weight = 20 },
        new CoinSpawnWeight { coinCount = 4, weight = 7 },
        new CoinSpawnWeight { coinCount = 5, weight = 3 }
    };


    [Header("Rare Collectible Chances (0–100%)")]
    [SerializeField, Range(0, 100)] private int gemChance = 5;
    [SerializeField, Range(0, 100)] private int starChance = 10;

    [Header("Lane X positions")]
    // Для вирівнювання позицій монеток по X
    [SerializeField] private float[] laneXPositions = new float[] { -1.9f, 0.23f, 2.4f };

    [Header("Lane selection")]
    [SerializeField] private bool useRandomLane = true;
    [SerializeField, Range(0, 2)] private int fixedLaneIndex = 1; // 0 = -1.9, 1 = 0.23, 2 = 2.4

    private float lastSpawnZ = 0f;
    private IScoreService scoreService;
    private CollectibleFeedbackManager feedbackManager;

    [Inject]
    public void Construct(IScoreService scoreService, CollectibleFeedbackManager feedbackManager)
    {
        this.scoreService = scoreService;
        this.feedbackManager = feedbackManager;
    }

    [System.Serializable]
    public class CoinSpawnWeight
    {
        public int coinCount;
        public int weight;
    }

    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            float zOffset = Random.Range(minZOffset, maxZOffset);
            float spawnZ = lastSpawnZ + zOffset;

            int roll = Random.Range(0, 100);
            if (roll < gemChance)
            {
                SpawnSingleRare(CollectibleTypes.Gem, spawnZ);
            }
            else if (roll < gemChance + starChance)
            {
                SpawnSingleRare(CollectibleTypes.Star, spawnZ);
            }
            else
            {
                int coinCount = RollCoinCount();
                int laneIndex = useRandomLane ? Random.Range(0, laneXPositions.Length) : Mathf.Clamp(fixedLaneIndex, 0, laneXPositions.Length - 1);
                SpawnCoinLine(laneIndex, coinCount, spawnZ);
            }

            lastSpawnZ = spawnZ;
            yield return new WaitForSeconds(spawnDelay);
        }
    }
    private int RollCoinCount()
    {
        int totalWeight = 0;
        foreach (var entry in coinSpawnWeights)
            totalWeight += entry.weight;

        int roll = Random.Range(0, totalWeight);
        int cumulative = 0;

        foreach (var entry in coinSpawnWeights)
        {
            cumulative += entry.weight;
            if (roll < cumulative)
                return entry.coinCount;
        }

        return 1; // fallback
    }

    private void SpawnCoinLine(int laneIndex, int count, float startZ)
    {
        float laneX = laneXPositions[Mathf.Clamp(laneIndex, 0, laneXPositions.Length - 1)];

        for (int i = 0; i < count; i++)
        {
            float z = startZ + i * coinSpacingZ;
            Vector3 pos = new Vector3(laneX, 0.45f, z);
            pos.x = SnapToNearestLaneX(pos.x); // чіткий снап навіть якщо laneX змінять
            SpawnCollectible(pos, CollectibleTypes.Coin);
        }
    }

    private void SpawnSingleRare(CollectibleTypes type, float z)
    {
        int laneIndex = useRandomLane ? Random.Range(0, laneXPositions.Length) : Mathf.Clamp(fixedLaneIndex, 0, laneXPositions.Length - 1);
        float x = laneXPositions[laneIndex];
        Vector3 pos = new Vector3(x, 0.45f, z);
        pos.x = SnapToNearestLaneX(pos.x);
        SpawnCollectible(pos, type);
    }

    private float SnapToNearestLaneX(float x)
    {
        if (laneXPositions == null || laneXPositions.Length == 0) return x;

        float nearest = laneXPositions[0];
        float bestDist = Mathf.Abs(x - nearest);
        for (int i = 1; i < laneXPositions.Length; i++)
        {
            float candidate = laneXPositions[i];
            float dist = Mathf.Abs(x - candidate);
            if (dist < bestDist)
            {
                bestDist = dist;
                nearest = candidate;
            }
        }
        return nearest;
    }
    private void SpawnCollectible(Vector3 position, CollectibleTypes type)
    {
        if (collectiblePresets == null || collectiblePresets.Length == 0) return;

        var data = System.Array.Find(collectiblePresets, c => c.Type == type);
        if (data == null)
        {
            Debug.LogWarning($"Collectible type {type} not found in presets!");
            return;
        }

        MyCollectibleScript collectible = Instantiate(data.CollectiblePrefab, position, Quaternion.identity);

        if (type == CollectibleTypes.Coin || type == CollectibleTypes.Star || type == CollectibleTypes.Gem)
        {
            if (collectible.GetComponent<CollectableRotation>() == null)
                collectible.gameObject.AddComponent<CollectableRotation>();
        }

        collectible.Init(data, scoreService, feedbackManager);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (laneXPositions == null) return;

        Gizmos.color = Color.yellow;
        float previewZ = Application.isPlaying ? lastSpawnZ : transform.position.z;
        for (int i = 0; i < laneXPositions.Length; i++)
        {
            Vector3 p1 = new Vector3(laneXPositions[i], 0.45f, previewZ - 20f);
            Vector3 p2 = new Vector3(laneXPositions[i], 0.45f, previewZ + 40f);
            Gizmos.DrawLine(p1, p2);
        }
    }
#endif
}
