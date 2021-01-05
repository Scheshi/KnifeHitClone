using KnifeHit.Controllers;
using KnifeHit.Datas;
using System;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class KnifeView : MonoBehaviour
{
    public event Action<GameObject> Collision;

    private void Start()
    {
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collision?.Invoke(collision.gameObject);
    }

}
