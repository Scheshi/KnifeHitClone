using KnifeHit.Interfaces;
using KnifeHit.Views;
using UnityEngine;


namespace KnifeHit.Controllers
{
    public class LogController : IFrameUpdatable
    {
        private Transform _logTransform;
        private Rigidbody2D _logRigidbody;
        private float _speed = 100.0f;

        public LogController(LogView logView)
        {
            _logTransform = logView.transform;
            _logRigidbody = logView.GetComponent<Rigidbody2D>();
            Updater.AddUpdatable(this);
        }

        public void Update()
        {
            _logTransform.Rotate(new Vector3(0, 0, _speed * Time.deltaTime));
        }
    }
}
