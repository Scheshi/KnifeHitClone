using KnifeHit.Controllers;
using KnifeHit.Datas;
using KnifeHit.Views;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


namespace KnifeHit.Services
{
    public class GameController
    {
        #region Fields

        [SerializeField] private CoreData _core;
        private InputManager _inputManager = new InputManager();
        private int counter = 0;
        private KnifeCreator _knifeController;
        private CounterController _coinCounterController;
        private CounterController _scoreCounter;
        private List<IDisposable> _disposables = new List<IDisposable>();

        #endregion


        #region Contructors

        public GameController(CoreData coreData)
        {
            _core = coreData;
            _disposables.Add(
                new GameObject("Updater")
                    .AddComponent<Updater>()
                    );

            var canvas = new GameObject("Canvas").AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.gameObject.AddComponent<GraphicRaycaster>();
            var scaler = canvas.gameObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.matchWidthOrHeight = 0.5f;
            canvas.transform.localPosition = Vector3.zero;

            var coinText = new GameObject("CoinCounter").AddComponent<Text>();
            coinText.transform.parent = canvas.transform;
            coinText.font = Font.CreateDynamicFontFromOSFont("Arial", 27);
            coinText.rectTransform.localPosition = new Vector2(
                coinText.rectTransform.rect.width,
                -coinText.rectTransform.rect.height);
            coinText.rectTransform.anchorMin = new Vector2(0.0f, 1.0f);
            coinText.rectTransform.anchorMax = new Vector2(0.0f, 1.0f);
            _coinCounterController = new CounterController(
                coinText,
                "Coin"
                );
            _coinCounterController.Load();

            var scoreText = new GameObject("ScoreCounter").AddComponent<Text>();
            scoreText.transform.parent = canvas.transform;
            scoreText.font = Font.CreateDynamicFontFromOSFont("Arial", 27);
            scoreText.rectTransform.localPosition = new Vector2(
                -scoreText.rectTransform.rect.width,
                -scoreText.rectTransform.rect.height);
            scoreText.rectTransform.anchorMin = new Vector2(1.0f, 1.0f);
            scoreText.rectTransform.anchorMax = new Vector2(1.0f, 1.0f);
            _scoreCounter = new CounterController(scoreText, "Score");

            _disposables.Add(_coinCounterController);
            _disposables.Add(_scoreCounter);

            CreatingLevel();
        }

        #endregion


        #region Methods

        private void CreatingLevel()
        {
            var log = GameObject.Instantiate(_core.Levels[counter].LogPrefab, Vector2.up * 8, Quaternion.identity)
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
                    var knife = GameObject.Instantiate(
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

            var logController = new LogController(log, _core.Levels[counter].HitCount, _core.Levels[counter].LogSpeed);
            logController.Death += NextLevel;
            logController.Damage += _scoreCounter.CreamentCount;

            var chance = Random.Range(0.0f, 1.0f);

            if (chance <= _core.CoinSpawnChance)
            {
                var logTransform = log.transform;
                GameObject.Instantiate(_core.CoinPrefab,
                    new Vector2(logTransform.position.x + 3f, logTransform.position.y),
                    Quaternion.identity, logTransform)
                    .GetComponent<CoinView>()
                    .Pickup += _coinCounterController.CreamentCount;
            }
            _knifeController = new KnifeCreator(_core.Levels[counter].KnifeCreator);
            _knifeController.EndGame += EndGame;
            _inputManager.Throw += _knifeController.Throwing;
            
        }

        

        public void NextLevel() 
        {
            Updater.RemoveUpdatable(_knifeController);
            _inputManager.Throw -= _knifeController.Throwing;
            counter++;
            if (counter >= _core.Levels.Length)
            {
                EndGame();
                throw new IndexOutOfRangeException("Уровни в " + _core.name + " закончились!");
            }
            else
            {
                CreatingLevel();
                Debug.Log("Следующий уровень");
            }
        }
        
        public void EndGame()
        {
            //Time.timeScale = 0.0f;
            Repository.Save();
            foreach (var item in _disposables)
            {
                item.Dispose();
            }
            _disposables.Clear();
            //var text = new GameObject("Panel").AddComponent<Text>();
            //text.text = "Конец игры!";
            //text.transform.parent = 
            //var button = new GameObject("Button").AddComponent<Button>();
            //button.onClick.AddListener(delegate ()
            //{
            //    new GameObject("GameInitializator").AddComponent<GameInitializator>();
            //});
            
        }

        #endregion
    }
}
