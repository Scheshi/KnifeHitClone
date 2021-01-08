using KnifeHit.Controllers;
using KnifeHit.Datas;
using KnifeHit.Services;
using KnifeHit.Views;
using UnityEngine;


namespace KnifeHit
{
    public class GameInitializator : MonoBehaviour
    {
        [SerializeField] private CoreData _core;
        private InputManager _inputManager = new InputManager();
        private int counter = 0;
        private KnifeCreator _knifeController;
        private CoinCounterController _counterController;

        private void Start()
        {
            new GameObject("Updater")
                .AddComponent<Updater>();

            _counterController = new CoinCounterController();
            CreatingLevel();
        }

        private void CreatingLevel()
        {
            var log = Instantiate(_core.Levels[counter].LogPrefab, Vector2.up * 8, Quaternion.identity)
                .GetComponent<LogView>();
            new LogController(log, _core.Levels[counter].HitCount, _core.Levels[counter].LogSpeed)
                .Death += NextLevel;
            

            var points = log.GetComponentsInChildren<KnifePointOnLogMarker>();
            var knifeCount = Random.Range(1, 3);

            for (int i = 0; i < knifeCount; i++)
            {
                var point = points[Random.Range(0, points.Length)];
                var knife = Instantiate(
                    _core.Levels[counter].KnifeCreator.KnifePrefab,
                    point.transform.position,
                    Quaternion.identity,
                    log.transform
                    ) ;
                knife.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 160));
            }

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
            _inputManager.Throw -= _knifeController.Throwing;
            counter++;
            CreatingLevel();
            Debug.Log("Следующий уровень");
        }

    }
}
