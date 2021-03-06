﻿using KnifeHit.Controllers;
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
        private List<IDisposable> _disposables = new List<IDisposable>();
        private Canvas _menuCanvas;


        private InputManager _inputManager = new InputManager();
        private int counter = 0;
        private KnifeCreator _knifeController;
        private CounterController _coinCounterController;
        private CounterController _scoreCounter;
        private LogController _logController;
        private CoinView _coinView;
        private Canvas _canvas;
        private LogView _logView;

        #endregion


        #region Contructors

        public GameController(CoreData coreData, Canvas menuCanvas)
        {
                _menuCanvas = menuCanvas;
            _core = coreData;
            _disposables.Add(
                new GameObject("Updater")
                    .AddComponent<Updater>()
                    );

            _canvas = new GameObject("Canvas").AddComponent<Canvas>();
            _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            _canvas.gameObject.AddComponent<GraphicRaycaster>();
            var scaler = _canvas.gameObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.matchWidthOrHeight = 0.5f;
            _canvas.transform.localPosition = Vector3.zero;

            var coinText = new GameObject("CoinCounter").AddComponent<Text>();
            coinText.Adjust(200, 100, _canvas.transform, new Vector2(
                -Screen.width / 2 + coinText.rectTransform.rect.width,
                Screen.height / 2 - coinText.rectTransform.rect.height / 2),
                new Vector2(0.0f, 1.0f),
                new Vector2(0.0f, 1.0f));
            coinText.alignment = TextAnchor.UpperLeft;


            _coinCounterController = new CounterController(
                coinText,
                "Coin"
                );
            _coinCounterController.Load();

            var scoreText = new GameObject("ScoreCounter").AddComponent<Text>();
            scoreText.Adjust(200, 100, _canvas.transform, new Vector2(
                 Screen.width / 2 - scoreText.rectTransform.rect.width,
                 Screen.height / 2 - scoreText.rectTransform.rect.height / 2),
                new Vector2(1.0f, 1.0f),
                new Vector2(1.0f, 1.0f));
            scoreText.alignment = TextAnchor.UpperRight;
            _scoreCounter = new CounterController(scoreText, "Score");

            _disposables.Add(_coinCounterController);
            _disposables.Add(_scoreCounter);
            _disposables.Add(_logController);
            _disposables.Add(_knifeController);

             CreatingLevel();
        }

        #endregion


        #region Methods

        private void CreatingLevel()
        {
            if (!Equals(_knifeController, null))
            {
                _knifeController.Dispose();
            }

            var log = GameObject.Instantiate(_core.Levels[counter].LogPrefab, Vector2.up * 8, Quaternion.identity);
            _logView = log.GetComponent<LogView>();

            var points = _logView.GetComponentsInChildren<KnifePointOnLogMarker>();

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
                        _logView.transform
                        );
                    if(knife.TryGetComponent(out Rigidbody2D rigidbody))
                    {
                        rigidbody.freezeRotation = true;
                        rigidbody.isKinematic = true;
                    }
                knife.transform.up = _logView.transform.position - knife.transform.position;
            }

            _disposables.Remove(_logController);
            _logController = new LogController(_logView, _core.Levels[counter].HitCount, _core.Levels[counter].LogSpeed);
            _logController.Death += NextLevel;
            _logController.Damage += _scoreCounter.CreamentCount;
            _disposables.Add(_logController);

            var chance = Random.Range(0.0f, 1.0f);

            if (chance <= _core.CoinSpawnChance)
            {
                var logTransform = _logView.transform;
                _coinView = GameObject.Instantiate(_core.CoinPrefab,
                    new Vector2(logTransform.position.x + 3f, logTransform.position.y),
                    Quaternion.identity, logTransform)
                    .GetComponent<CoinView>();
                    _coinView.Pickup += _coinCounterController.CreamentCount;
            }
            _disposables.Remove(_knifeController);
            _knifeController = new KnifeCreator(_core.Levels[counter].KnifeCreator);
            _knifeController.EndGame += EndGame;
            _inputManager.Throw += _knifeController.Throwing;
            _disposables.Add(_knifeController);
            
        }

        

        public void NextLevel() 
        {
            Updater.RemoveUpdatable(_knifeController);
            counter++;
            if (counter >= _core.Levels.Length)
            {
                EndGame(true);
                throw new IndexOutOfRangeException("Уровни в " + _core.name + " закончились!");
            }
            else
            {
                CreatingLevel();
                Debug.Log("Следующий уровень");
            }
        }
        
        public void EndGame(bool isWin)
        {
            Handheld.Vibrate();
            _logController.Death -= NextLevel;
            _logController.Damage -= _scoreCounter.CreamentCount;
            if(_coinView != null)
            _coinView.Pickup -= _coinCounterController.CreamentCount;
            _inputManager.Throw -= _knifeController.Throwing;
            //Time.timeScale = 0.0f;
            Repository.Save();
            for (int i = 0; i<_disposables.Count; i++)
            {   
                    if(_disposables[i] != null)
                    _disposables[i].Dispose();
            }
            _disposables.Clear();
            var button = new GameObject("Button").AddComponent<Button>();
            button.transform.parent = _canvas.transform;
            button.transform.localPosition = Vector3.zero;
            var buttonImage = button.gameObject.AddComponent<Image>();
            buttonImage.sprite = Resources.Load<Sprite>("Sprite/button");
            button.targetGraphic = buttonImage;
            button.onClick.AddListener(delegate ()
            {
                GameObject.Destroy(_canvas.gameObject);
                _menuCanvas.enabled = true;
            });

            var text = new GameObject("Text").AddComponent<Text>();
            text.Adjust(100, 200, _canvas.transform, new Vector2(
                button.transform.localPosition.x,
                button.transform.localPosition.y + text.rectTransform.rect.height
                ));
            if (isWin)
                text.text = "Вы выйграли!";
            else text.text = "Вы проиграли!";
            
        }

        #endregion
    }
}
