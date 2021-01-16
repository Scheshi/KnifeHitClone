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
            try
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
            coinText.transform.parent = _canvas.transform;
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
            scoreText.transform.parent = _canvas.transform;
            scoreText.font = Font.CreateDynamicFontFromOSFont("Arial", 27);
            scoreText.rectTransform.localPosition = new Vector2(
                -scoreText.rectTransform.rect.width,
                -scoreText.rectTransform.rect.height);
            scoreText.rectTransform.anchorMin = new Vector2(1.0f, 1.0f);
            scoreText.rectTransform.anchorMax = new Vector2(1.0f, 1.0f);
            _scoreCounter = new CounterController(scoreText, "Score");

            _disposables.Add(_coinCounterController);
            _disposables.Add(_scoreCounter);
            _disposables.Add(_logController);
            _disposables.Add(_knifeController);


                CreatingLevel();
            }
            catch(Exception e)
            {
                var errorText = new GameObject("Error").AddComponent<Text>();
                errorText.font = Font.CreateDynamicFontFromOSFont("Arial", 27);
                errorText.rectTransform.anchorMin = new Vector2(1.0f, 1.0f);
                errorText.rectTransform.anchorMax = new Vector2(1.0f, 1.0f);
                errorText.transform.parent = _menuCanvas.transform;
                errorText.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width);
                errorText.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height);
                errorText.text = e.ToString();
                errorText.transform.localPosition = new Vector2(0.0f, -Screen.height / 4);
                _menuCanvas.enabled = true;
            }
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
            _logController.Death -= NextLevel;
            //_logController.Death();
            _logController.Damage -= _scoreCounter.CreamentCount;
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
            
        }

        #endregion
    }
}
