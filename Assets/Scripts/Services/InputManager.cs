using KnifeHit.Interfaces;
using System;
using UnityEngine;


namespace KnifeHit.Services
{
    public class InputManager : IFrameUpdatable
    {
        public event Action Throw;
#if (!UNITY_ANDROID && !UNITY_IOS) || UNITY_EDITOR
        private string _throwKnife = "Jump";
#endif

        public InputManager()
        {
            Updater.AddUpdatable(this);
        }

        public void Update()
        {
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            if (Input.touchCount > 0)
            {
                Throw?.Invoke();
            }
#else
            if (Input.GetButtonDown(_throwKnife))
            {
                Throw?.Invoke();
            }
#endif

        }
    }
}
