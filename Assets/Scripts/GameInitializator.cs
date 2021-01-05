using KnifeHit.Datas;
using KnifeHit.Services;
using UnityEngine;


namespace KnifeHit
{
    public class GameInitializator : MonoBehaviour
    {
        [SerializeField] private KnifeData _knifeData;

        private void Start()
        {
            var updater = new GameObject("Updater").AddComponent<Updater>();

            var inputManager = new InputManager();

            var knifeCreator = new KnifeCreator(_knifeData);
            inputManager.Throw += knifeCreator.Creating;

            Destroy(gameObject);
        }
    }
}
