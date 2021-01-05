using KnifeHit.Structs;
using UnityEngine;


namespace KnifeHit.Datas
{
    [CreateAssetMenu(menuName = "Datas/Knife Creator")]
    public class KnifeCreatorData : ScriptableObject
    {
        public KnifeCreatorStruct KnifeCreator;
        public GameObject KnifePrefab;
        public KnifeData KnifeData;
    }
}
