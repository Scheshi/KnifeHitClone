using UnityEngine;
using UnityEngine.UI;

public class CoinCounterController
{
    private Text _textCount;
    private float _count = 0;

    public CoinCounterController()
    {
        _textCount = GameObject.FindObjectOfType<Text>();
    }

    public void CreamentCount()
    {
        _count++;
        _textCount.text = _count.ToString();
    }


}
