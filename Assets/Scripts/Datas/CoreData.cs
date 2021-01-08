using KnifeHit.Structs;
using UnityEngine;


namespace KnifeHit.Datas
{
    [CreateAssetMenu(menuName = "Datas/Core")]
    public class CoreData : ScriptableObject
    {
        public LevelData[] Levels;
        [Range(0.0f, 1.0f)]public float CoinSpawnChance;
        public GameObject CoinPrefab;
    }
}
