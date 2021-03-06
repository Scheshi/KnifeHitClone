﻿using KnifeHit.Controllers;
using System;
using UnityEngine;


namespace KnifeHit.Views
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class LogView : MonoBehaviour
    {
        [SerializeField] private GameObject _piecesPrefab;
        [SerializeField] private AudioClip _hitSound;

        public event Action<GameObject> Collision;

        private void Start()
        {
            if (!TryGetComponent(out Rigidbody2D rigidbody))
                 throw new NullReferenceException("Нет Rigidbody2d на Log");
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            AudioSource.PlayClipAtPoint(_hitSound, transform.position);
            Collision?.Invoke(collision.gameObject);   
        }

        public void Crash()
        {
            var count = UnityEngine.Random.Range(5, 10);
                for (int i = 0; i < count; i++)
                {
                    Instantiate(_piecesPrefab, transform.position, Quaternion.identity)
                        .GetComponent<Rigidbody2D>()
                        .AddForce(new Vector2(UnityEngine.Random.Range(-10, 10),
                                                UnityEngine.Random.Range(-10, 10)),
                                                ForceMode2D.Impulse
                                              );

                }
            Destroy(gameObject);
        }
    }
}
