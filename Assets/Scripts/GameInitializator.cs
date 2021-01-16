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


            var coinText = new GameObject("CoinText").AddComponent<Text>();
            coinText.transform.parent = canvas.transform;
            coinText.font = Font.CreateDynamicFontFromOSFont("Arial", 27);
            coinText.text = $"Кол-во монет = {PlayerPrefs.GetInt("Coin")}";
            coinText.rectTransform.localPosition = new Vector3(0.0f, Screen.width / 4, 0.0f);

            var scoreText = new GameObject("ScoreText").AddComponent<Text>();
            scoreText.transform.parent = canvas.transform;
            scoreText.font = Font.CreateDynamicFontFromOSFont("Arial", 27);
            scoreText.text = $"Ваш рекорд = {PlayerPrefs.GetInt("Score")}";
            scoreText.rectTransform.localPosition = new Vector3(0.0f, Screen.width / 4 - coinText.rectTransform.rect.width, 0.0f);


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

            


            Destroy(gameObject);
        }
    }
}
