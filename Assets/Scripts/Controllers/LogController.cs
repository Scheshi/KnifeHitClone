﻿using KnifeHit.Interfaces;
using KnifeHit.Views;
using System;
using UnityEngine;


namespace KnifeHit.Controllers
{
    public class LogController : IFrameUpdatable
    {
        public Action Death;
        private Transform _logTransform;
        private Rigidbody2D _logRigidbody;
        private float _speed = 100.0f;
        private int _health;

        public LogController(LogView logView, int health)
        {
            _logTransform = logView.transform;
            _logRigidbody = logView.GetComponent<Rigidbody2D>();
            logView.Collision += OnCollision;
            Updater.AddUpdatable(this);
            _health = health;
        }

        public void Update()
        {
            _logTransform.Rotate(new Vector3(0, 0, _speed * Time.deltaTime));
        }

        private void OnCollision(GameObject obj)
        {
            if(obj.TryGetComponent(out KnifeView knife))
            {
                _health--;
            }
            if (_health <= 0)
            {
                Death?.Invoke();
                Death = null;
                Updater.RemoveUpdatable(this);
                GameObject.Destroy(_logRigidbody.gameObject);
            }
        }
    }
}
