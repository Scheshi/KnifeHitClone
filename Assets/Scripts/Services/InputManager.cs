using KnifeHit.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace KnifeHit.Services
{
    public class InputManager : IFrameUpdatable
    {
        public event Action Throw;
        private string _throwKnife = "Jump";

        public InputManager()
        {
            Updater.AddUpdatable(this);
        }

        public void Update()
        {
            if (Input.GetButtonDown(_throwKnife))
            {
                Throw?.Invoke();
            }

        }
    }
}
