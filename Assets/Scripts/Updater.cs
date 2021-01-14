using System.Collections.Generic;
using UnityEngine;
using KnifeHit.Interfaces;
using System;

namespace KnifeHit
{
    public class Updater : MonoBehaviour, IDisposable
    {

        #region Fields

        private static List<IFixedUpdatable> _fixedUpdatables = new List<IFixedUpdatable>();
        private static List<ILateUpdate> _lateUpdatables = new List<ILateUpdate>();
        private static List<IFrameUpdatable> _framesUpdatables = new List<IFrameUpdatable>();

        #endregion


        #region UnityMethods

        private void Update()
        {
            for(int i = 0; i<_framesUpdatables.Count; i++)
            {
                _framesUpdatables[i].Update();
            }
        }

        private void FixedUpdate()
        {
            for(int i = 0; i<_fixedUpdatables.Count; i++)
            {
                _fixedUpdatables[i].FixedUpdate();
            }
        }

        private void LateUpdate()
        {
            for(int i = 0; i<_lateUpdatables.Count; i++)
            {
                _lateUpdatables[i].LateUpdate();
            }
        }

        #endregion


        #region Methods

        /// <summary>
        ///  Добавление объекта в список апдейтов
        /// </summary>
        /// <param name="updatable">Объект, который нужно обновлять</param>
        public static void AddUpdatable(IUpdatable updatable)
        {
            if(updatable is IFrameUpdatable)
            {
                _framesUpdatables.Add(updatable as IFrameUpdatable);
            }
            if(updatable is IFixedUpdatable)
            {
                _fixedUpdatables.Add(updatable as IFixedUpdatable);
            }
            if(updatable is ILateUpdate)
            {
                _lateUpdatables.Add(updatable as ILateUpdate);
            }
        }

        /// <summary>
        /// Удаление объекта из списка объектов для апдейта
        /// </summary>
        /// <param name="updatable">Объект, который нужно удалить</param>
        public static void RemoveUpdatable(IUpdatable updatable)
        {
            if (updatable is IFrameUpdatable)
            {
                _framesUpdatables.Remove(updatable as IFrameUpdatable);
            }
            if (updatable is IFixedUpdatable)
            {
                _fixedUpdatables.Remove(updatable as IFixedUpdatable);
            }
            if (updatable is ILateUpdate)
            {
                _lateUpdatables.Remove(updatable as ILateUpdate);
            }
        }

        public void Dispose()
        {
            for(int i = 0; i < _fixedUpdatables.Count; i++)
            {
                RemoveUpdatable(_fixedUpdatables[i]);
            }
            for (int i = 0; i < _framesUpdatables.Count; i++)
            {
                RemoveUpdatable(_framesUpdatables[i]);
            }
            for(int i = 0; i < _lateUpdatables.Count; i++)
            {
                RemoveUpdatable(_lateUpdatables[i]);
            }
            Destroy(gameObject);
        }

        #endregion

    }
}
