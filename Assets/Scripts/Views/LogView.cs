using KnifeHit.Controllers;
using System;
using UnityEngine;


namespace KnifeHit.Views
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class LogView : MonoBehaviour
    {
        public event Action<GameObject> Collision;

        private void Start()
        {
            if (!TryGetComponent(out Rigidbody2D rigidbody))
                 throw new NullReferenceException("Нет Rigidbody2d на Log");
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Collision?.Invoke(collision.gameObject);   
        }
    }
}
