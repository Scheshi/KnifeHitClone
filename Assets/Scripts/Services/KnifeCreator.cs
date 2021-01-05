using KnifeHit.Datas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeCreator
{
    private GameObject _prefab;
    private float _coolDown;

    public KnifeCreator(KnifeData data)
    {
        _prefab = data.KnifePrefab;
    }


    public void Creating()
    {
        if (Time.time > _coolDown)
        {
            _coolDown = Time.time + 1.0f;
            GameObject.Instantiate(_prefab, Vector2.zero, Quaternion.identity);
        }
    }
}
