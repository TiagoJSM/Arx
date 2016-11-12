using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class SingleCharacterSpawn : MonoBehaviour
{
    private GenericComponents.Controllers.Characters.CharacterController _currentCharacter;

    [SerializeField]
    private GenericComponents.Controllers.Characters.CharacterController _characterPrefab;
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
                as GenericComponents.Controllers.Characters.CharacterController;
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