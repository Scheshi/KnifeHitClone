using KnifeHit.Interfaces;
using System.Collections.Generic;
using UnityEngine;


namespace KnifeHit.Services {
    public static class Repository
    {
        private readonly static List<ISaving> _savers = new List<ISaving>();

        public static void AddSaving(ISaving saving)
        {
            _savers.Add(saving);
        }

        public static void RemoveSaving(ISaving saving)
        {
            _savers.Remove(saving);
        }

        public static void Save()
        {
            foreach(var saver in _savers)
            {
                saver.Save();
            }
        }

        public static void Load()
        {
            foreach(var saver in _savers)
            {
                saver.Load();
            }
        }
    }
}
