using System;
using UnityEngine;


public class CoinView : MonoBehaviour
{
    public event Action Pickup;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<KnifeView>(out var view))
        {
            Pickup?.Invoke();
            Destroy(gameObject);
        }
    }

}
