using KnifeHit.Services;
using UnityEngine;
using UnityEngine.UI;


namespace KnifeHit.Views
{
    public class CounterController : Interfaces.ISaving
    {
        private readonly Text _textCount;
        private int _count;
        private readonly string _key;
        private string _text;

        public CounterController(Text text, string saveKey)
        {
            Repository.AddSaving(this);
            _textCount = text;
            _key = saveKey;
            _count = 0;
            _text = saveKey + ": ";
            _textCount.text = _text + _count.ToString();
        }

        public void CreamentCount()
        {
            _count++;
            _textCount.text = _text + _count.ToString();
        }

        public void Save()
        {
            PlayerPrefs.SetInt(_key, _count);
            PlayerPrefs.Save();
        }

        public void Load()
        {
            _count = PlayerPrefs.GetInt(_key, 0);
            _textCount.text = _text + _count.ToString();
        }

    }
}
