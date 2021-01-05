using KnifeHit.Interfaces;
using KnifeHit.Structs;
using KnifeHit.Views;
using UnityEngine;


namespace KnifeHit.Controllers
{
    public class KnifeController
    {
        private KnifeView _knifeView;
        private Rigidbody2D _knifeRigidbody;
        private KnifeStruct _struct;

        public KnifeController(KnifeView knifeView, KnifeStruct str)
        {
            _knifeView = knifeView;
            _knifeRigidbody = knifeView.GetComponent<Rigidbody2D>();
            _struct = str;
            knifeView.Collision += OnCollision;
        }

        public void Throw()
        {
                Debug.Log("Выстрел");
                _knifeRigidbody.AddForce(new Vector2(0, _struct.Speed), ForceMode2D.Impulse);
        }

        private void OnCollision(GameObject obj)
        {
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