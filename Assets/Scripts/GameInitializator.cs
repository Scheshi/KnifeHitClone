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

        private void Start()
        {
            var updater = new GameObject("Updater").AddComponent<Updater>();
            CreatingLevel();
        }

        private void CreatingLevel()
        {
            var log = Instantiate(_core.Levels[counter].LogPrefab, Vector2.up * 4, Quaternion.identity)
                .GetComponent<LogView>();
            new LogController(log, _core.Levels[counter].HitCount)
                .Death += NextLevel;
            _knifeController = new KnifeCreator(_core.Levels[counter].KnifeCreator);
            _inputManager.Throw += _knifeController.Throwing;
        }

        public void NextLevel() 
        {
            counter++;
            CreatingLevel();
            Debug.Log("Следующий уровень");
            _inputManager.Throw -= _knifeController.Throwing;
        }
    }
}
