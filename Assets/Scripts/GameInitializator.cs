using KnifeHit.Controllers;
using KnifeHit.Datas;
using KnifeHit.Services;
using KnifeHit.Views;
using System;
using UnityEngine;
using Random = UnityEngine.Random;


namespace KnifeHit
{
    public class GameInitializator : MonoBehaviour
    {
        #region Methods

        [SerializeField] private CoreData _core;
        private InputManager _inputManager = new InputManager();
        private int counter = 0;
        private KnifeCreator _knifeController;
        private CoinCounterController _counterController;

        #endregion


        #region UnityMethods

        private void Start()
        {
            new GameObject("Updater")
                .AddComponent<Updater>();

            _counterController = new CoinCounterController();
            CreatingLevel();
        }

        #endregion


        #region Methods

        private void CreatingLevel()
        {
            var log = Instantiate(_core.Levels[counter].LogPrefab, Vector2.up * 8, Quaternion.identity)
                .GetComponent<LogView>();

            var points = log.GetComponentsInChildren<KnifePointOnLogMarker>();

            if(points.Length == 0)
            {
                throw new NullReferenceException("Нет точек на бревне для спавна кинжалов");
            }

            var knifeCount = Random.Range(1, 4);

                for (int i = 0; i < knifeCount; i++)
                {
                if (i > points.Length) break;
                    var point = points[i];
                    var knife = Instantiate(
                        _core.Levels[counter].KnifeCreator.KnifePrefab,
                        point.transform.position,
                        Quaternion.identity,
                        log.transform
                        );
                    if(knife.TryGetComponent(out Rigidbody2D rigidbody))
                    {
                        rigidbody.freezeRotation = true;
                        rigidbody.isKinematic = true;
                    }
                knife.transform.up = log.transform.position - knife.transform.position;
            }

            new LogController(log, _core.Levels[counter].HitCount, _core.Levels[counter].LogSpeed)
                .Death += NextLevel;

            var chance = Random.Range(0.0f, 1.0f);

            if (chance <= _core.CoinSpawnChance)
            {
                var logTransform = log.transform;
                Instantiate(_core.CoinPrefab,
                    new Vector2(logTransform.position.x + 1.5f, logTransform.position.y),
                    Quaternion.identity, logTransform)
                    .GetComponent<CoinView>()
                    .Pickup += _counterController.CreamentCount;
            }
            _knifeController = new KnifeCreator(_core.Levels[counter].KnifeCreator);
            _inputManager.Throw += _knifeController.Throwing;
        }

        

        public void NextLevel() 
        {
            Updater.RemoveUpdatable(_knifeController);
            _inputManager.Throw -= _knifeController.Throwing;
            counter++;
            if (counter >= _core.Levels.Length)
            {
                throw new IndexOutOfRangeException("Уровни в " + _core.name + " закончились!");

            }
            else
            {
                CreatingLevel();
                Debug.Log("Следующий уровень");
            }
        }

        #endregion
    }
}
