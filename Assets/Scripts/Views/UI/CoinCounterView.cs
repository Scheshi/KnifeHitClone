using KnifeHit.Services;
using UnityEngine;
using UnityEngine.UI;


namespace KnifeHit.Views
{
    public class CoinCounterController : Interfaces.ISaving
    {
        private readonly Text _textCount;
        private int _count = 0;
        private const string _key = "CoinCount";

        public CoinCounterController()
        {
            Repository.AddSaving(this);
            _textCount = GameObject.FindObjectOfType<Text>();
        }

        public void CreamentCount()
        {
            _count++;
            _textCount.text = _count.ToString();
        }

        public void Save()
        {
            PlayerPrefs.SetInt(_key, _count);
            PlayerPrefs.Save();
        }

        public void Load()
        {
            _count = PlayerPrefs.GetInt(_key, 0);
        }

    }
}
