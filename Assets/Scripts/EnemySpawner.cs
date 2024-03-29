﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _enemyPrefabs;

    private int _maxEnemies = 10;
    [SerializeField] private float _enemyRadius;
    
    private Transform _player;
    
    private void Start()
    {
        _player = GameObject.Find("Player").transform;
        
        InvokeRepeating(nameof(SpawnEnemy), 0, 1);
    }

    private void SpawnEnemy()
    {
        _maxEnemies = 10 + GameData.Score / 50;
        if (transform.childCount >= _maxEnemies) return;
        
        double angle = UnityEngine.Random.Range(0, 360);
        var x = (float) (_enemyRadius * Math.Cos(angle.ToRadians()) + _player.position.x);
        var y = (float) (_enemyRadius * Math.Sin(angle.ToRadians()) + _player.position.y);

        var enemy = Instantiate(_enemyPrefabs[UnityEngine.Random.Range(0, _enemyPrefabs.Length)], new Vector3(x, y), Quaternion.identity, transform);
    }
    
}
