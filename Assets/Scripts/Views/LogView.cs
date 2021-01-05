using KnifeHit.Controllers;
using System;
using UnityEngine;


namespace KnifeHit.Views
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class LogView : MonoBehaviour
    {
        private void Start()
        {
            if (TryGetComponent(out Rigidbody2D rigidbody))
            {
                new LogController(this);
            }
            else throw new NullReferenceException("Нет Rigidbody2d на Log");
        }
    }
}
