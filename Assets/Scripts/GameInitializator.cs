using KnifeHit.Datas;
using KnifeHit.Services;
using UnityEngine;


namespace KnifeHit
{
    public class GameInitializator : MonoBehaviour
    {
        [SerializeField] private KnifeData _knifeData;
        [SerializeField] private KnifeCreatorData _creatorData;

        private void Start()
        {
            var updater = new GameObject("Updater").AddComponent<Updater>();

            var inputManager = new InputManager();

            var knifeCreator = new KnifeCreator(_creatorData);
            inputManager.Throw += knifeCreator.Throwing;

            Destroy(gameObject);
        }
    }
}
