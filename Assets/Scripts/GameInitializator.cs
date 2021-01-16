using KnifeHit.Controllers;
using KnifeHit.Datas;
using System;
using UnityEngine;
using UnityEngine.UI;


namespace KnifeHit
{
    public class GameInitializator : MonoBehaviour
    {
        [SerializeField] private GameObject _startButtonPrefab;
        [SerializeField] private GameObject _quitButtonPrefab;
        [SerializeField] private CoreData _coreData;

        public void Start()
        {
            var canvas = new GameObject("Canvas").AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.gameObject.AddComponent<GraphicRaycaster>();
            var scaler = canvas.gameObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.matchWidthOrHeight = 0.5f;


            


            var startButtonPrefab = Instantiate(_startButtonPrefab, canvas.transform);
            var quitButtonPrefab = Instantiate(_quitButtonPrefab, canvas.transform);
            Button startButton;
            Button quitButton;
            if (!startButtonPrefab.TryGetComponent(out startButton) ||
                !quitButtonPrefab.TryGetComponent(out quitButton))
            {
                throw new NullReferenceException("Нет компонента Button на префабе кнопки");
            }
            else
            {
                new MenuController(startButton, quitButton, _coreData);
            }

            var coinText = new GameObject("CoinText").AddComponent<Text>();

            var scoreText = new GameObject("ScoreText").AddComponent<Text>();

            coinText.Adjust(200, 100, canvas.transform, new Vector2(startButton.transform.localPosition.x, coinText.rectTransform.rect.height));
            coinText.text = $"Кол-во монет = {PlayerPrefs.GetInt("Coin")}";

            scoreText.Adjust(200, 100, canvas.transform, new Vector2(startButton.transform.localPosition.x, coinText.rectTransform.localPosition.y + scoreText.rectTransform.rect.height));
            scoreText.text = $"Ваш рекорд = {PlayerPrefs.GetInt("Score")}";




            Destroy(gameObject);
        }
    }
}
