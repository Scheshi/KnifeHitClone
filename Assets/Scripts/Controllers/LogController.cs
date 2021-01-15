using KnifeHit.Interfaces;
using KnifeHit.Views;
using System;
using UnityEngine;


namespace KnifeHit.Controllers
{
    public class LogController : IFrameUpdatable, IDisposable
    {
        public Action Damage;
        public Action Death;
        private Transform _logTransform;
        private LogView _view;
        private float _speed = 100.0f;
        private int _health;

        public LogController(LogView logView, int health, float logSpeed)
        {
            _view = logView;
            _logTransform = logView.transform;
            logView.Collision += OnCollision;
            Updater.AddUpdatable(this);
            _health = health;
            _speed = logSpeed;
        }

        public void Dispose()
        {
            Death?.Invoke();
            Death = null;
            _view.Crash();
            Updater.RemoveUpdatable(this);
        }

        public void Update()
        {
            _logTransform.Rotate(new Vector3(0, 0, _speed * Time.deltaTime));
        }

        private void OnCollision(GameObject obj)
        {
            if (_health <= 0)
            {
                Dispose();
            }
            else if (obj.TryGetComponent(out KnifeView knife))
            {
                _health--;
                Damage?.Invoke();
            }

        }
    }
}
