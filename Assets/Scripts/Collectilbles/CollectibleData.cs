using UnityEngine;
using Game.Models;
using MoreMountains.Feedbacks;

[CreateAssetMenu(fileName = "CollectibleData", menuName = "Runner/Collectible")]
public class CollectibleData : ScriptableObject
{
    [SerializeField] private CollectibleTypes type;
    public CollectibleTypes Type => type;

    [SerializeField] private Sprite icon;
    public Sprite Icon => icon;
    [SerializeField] private GameObject Prefab;
    public MyCollectibleScript CollectiblePrefab => Prefab != null ? Prefab.GetComponent<MyCollectibleScript>() : null;

    [SerializeField] private int value = 10;
    public int Value => value;

    [SerializeField] public MMFeedbacks collectFeedback;

    [SerializeField] private GameObject collectEffect;
    public GameObject CollectEffect => collectEffect;

}
