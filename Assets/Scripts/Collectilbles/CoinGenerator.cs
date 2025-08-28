using UnityEngine;
using System.Collections;
using Game.Models;

public class CoinGenerator : MonoBehaviour
{
    public CollectibleData[] collectibleTypes;
    [SerializeField] public float spawnInterval = 2f; // через скільки секунд спавн
    [SerializeField] public int minRow = 3; // мінімум монет у ряді
    [SerializeField] public int maxRow = 4; // максимум монет у ряді
    [SerializeField] public float minXDistance = 1f; // мін відстань між монетами по X
    [SerializeField] public float maxXDistance = 2f; // макс відстань між монетами по X
    [SerializeField] public float minZDistance = 5f; // мін відстань по Z від попереднього ряду
    [SerializeField] public float maxZDistance = 10f; // макс відстань по Z

    private float lastZ = 0f; // остання Z-позиція

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
            int coinsInRow = GetCoinsInRow();
            float zOffset = Random.Range(minZDistance, maxZDistance);
            float rowZ = lastZ + zOffset;

            for (int i = 0; i < coinsInRow; i++)
            {
                float xOffset = Random.Range(minXDistance, maxXDistance);

                int side = (i % 2 == 0 ? 1 : -1);
                float x = side * (i / 2 + 1) * xOffset;

                Vector3 spawnPos = new Vector3(x, 0.45f, rowZ);
                SpawnCollectible(spawnPos);
            }


            lastZ = rowZ;

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnCollectible(Vector3 position)
    {
        if (collectibleTypes.Length == 0) return;

        var data = collectibleTypes[Random.Range(0, collectibleTypes.Length)];
        GameObject obj = Instantiate(data.prefab, position, Quaternion.identity);

        if (data.type == CollectibleTypes.Coin)
        {
            if (obj.GetComponent<CollectableRotation>() == null)
                obj.AddComponent<CollectableRotation>();
        }
        var script = obj.GetComponent<MyCollectibleScript>();
        if (script != null)
        {
            script.Init(data);
        }
    }
}

