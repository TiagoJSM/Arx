using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class SingleCharacterSpawn : MonoBehaviour
{
    private Assets.Standard_Assets._2D.Scripts.Controllers.BasePlatformerController _currentCharacter;

    [SerializeField]
    private Assets.Standard_Assets._2D.Scripts.Controllers.BasePlatformerController _characterPrefab;
    [SerializeField]
    [Range(1, 60*3)]
    private int _spawnInSecondsDelay = 5;

    private void Start()
    {
        StartCoroutine(Spawn());
    }

    private void InstantiateCharacter()
    {
        _currentCharacter = Instantiate(_characterPrefab, this.transform.position, Quaternion.identity)
                as Assets.Standard_Assets._2D.Scripts.Controllers.BasePlatformerController;
    }

    private IEnumerator Spawn()
    {
        InstantiateCharacter();
        while (true)
        {
            yield return new WaitUntil(() => _currentCharacter != null && _currentCharacter.LifePoints <= 0);
            yield return new WaitForSeconds(_spawnInSecondsDelay);
            InstantiateCharacter();
        }
    }
}