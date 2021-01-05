﻿using KnifeHit.Structs;
using UnityEngine;


namespace KnifeHit.Datas
{
    [CreateAssetMenu(menuName = "Datas/Level")]
    public class LevelData : ScriptableObject
    {
        public KnifeCreatorData KnifeCreator;
        public int HitCount;
    }
}