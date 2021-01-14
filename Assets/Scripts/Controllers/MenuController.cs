using KnifeHit.Datas;
using KnifeHit.Services;
using UnityEngine;
using UnityEngine.UI;

namespace KnifeHit.Controllers
{
    public class MenuController
    {
        private CoreData _core;
        public MenuController(Button start, Button quit, CoreData data)
        {
            Debug.Log("Создание контроллера меню");
            quit.onClick.AddListener(Quit);
            start.onClick.AddListener(Start);
            _core = data;
        }


        private void Quit()
        {
            Application.Quit();
        }

        private void Start()
        {
            Debug.Log("Нажат Start");
            new GameController(_core);
        }

    }
}
