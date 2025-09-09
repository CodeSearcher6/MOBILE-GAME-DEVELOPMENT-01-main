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
    [SerializeField] private MyCollectibleScript prefab;
    public MyCollectibleScript CollectiblePrefab => prefab;// ось тут змінив

    [SerializeField] private int value = 10;
    public int Value => value;
    [SerializeField] public MMF_Player collectFeedback;
    public MMF_Player CollectFeedback => collectFeedback;
    
    [SerializeField] private GameObject collectEffect;
    public GameObject CollectEffect => collectEffect;

}