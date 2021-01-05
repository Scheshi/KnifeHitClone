using KnifeHit.Interfaces;
using KnifeHit.Views;
using UnityEngine;


namespace KnifeHit.Controllers
{
    public class KnifeController : IFixedUpdatable
    {
        private KnifeView _knifeView;
        private Rigidbody2D _knifeRigidbody;

        public KnifeController(KnifeView knifeView)
        {
            _knifeView = knifeView;
            _knifeRigidbody = knifeView.GetComponent<Rigidbody2D>();
            knifeView.Collision += OnCollision;
            Updater.AddUpdatable(this);
        }

        public void FixedUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Выстрел");
                _knifeRigidbody.AddForce(new Vector2(0, 10.0f), ForceMode2D.Impulse);
            }
        }

        private void OnCollision(GameObject obj)
        {
            Updater.RemoveUpdatable(this);
            if (obj.TryGetComponent(out KnifeView _))
            {
                Debug.Log("Проигрыш");
            }
            else if(obj.TryGetComponent(out LogView view))
            {
                _knifeView.transform.parent = view.transform;
                _knifeRigidbody.freezeRotation = true;
                _knifeRigidbody.isKinematic = true;
            }
        }
    }
}