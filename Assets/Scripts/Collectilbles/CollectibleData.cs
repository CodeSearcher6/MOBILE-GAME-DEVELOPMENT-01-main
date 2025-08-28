using UnityEngine;
using Game.Models;
using MoreMountains.Feedbacks;


[CreateAssetMenu(fileName = "CollectibleData", menuName = "Runner/Collectible")]
public class CollectibleData : ScriptableObject
{
    public CollectibleTypes type;
    public Sprite icon;
    public int value;
    public GameObject prefab;
    public MMF_Player feedbacks;
    public GameObject collectEffect;
}
