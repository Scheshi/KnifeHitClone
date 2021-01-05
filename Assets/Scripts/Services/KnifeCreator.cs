﻿using KnifeHit;
using KnifeHit.Controllers;
using KnifeHit.Datas;
using KnifeHit.Interfaces;
using UnityEngine;


public class KnifeCreator : IFrameUpdatable
{
    #region Fields

    private GameObject _prefab;
    private KnifeData _knifeData;
    private float _coolDownThrow;
    private float _currentCoolDownThrow;
    private float _currentCoolDownForCreating;
    private bool _isCreating = false;
    private KnifeView _tempView;

    #endregion


    #region Contructors

    public KnifeCreator(KnifeCreatorData data)
    {
        _currentCoolDownForCreating = _coolDownThrow / 4;
        _prefab = data.KnifePrefab;
        _coolDownThrow = data.KnifeCreator.CoolDownPerThrow;
        _knifeData = data.KnifeData;
        Updater.AddUpdatable(this);
    }

    #endregion


    #region Methods

    public void Throwing()
    {
        if (_isCreating) 
        {             
            if (Time.time > _currentCoolDownThrow)
            {
                _currentCoolDownThrow = Time.time + _coolDownThrow;
                var knife = new KnifeController(_tempView, _knifeData.Knife);
                knife.Throw();
                _isCreating = false;
            }
        }
    }

    public void Update()
    {
        if (!_isCreating)
        {
            if (_currentCoolDownForCreating <= 0)
            {
                _tempView = GameObject
                    .Instantiate(_prefab, Vector2.zero, Quaternion.identity)
                    .GetComponent<KnifeView>();
                _currentCoolDownForCreating = _coolDownThrow / 4;
                _isCreating = true;
            }
            else _currentCoolDownForCreating -= Time.deltaTime;
        }
    }

    #endregion
}