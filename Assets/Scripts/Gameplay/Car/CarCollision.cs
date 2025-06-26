using UnityEngine;
using Zenject;

public class CarCollision : MonoBehaviour
{
    [Inject]
    private GameState _gameState;

    private void OnCollisionEnter(Collision _collision)
    {
        if (_collision.gameObject.tag == TagDictionary.obstacle)
        {
            _gameState.Loss();
        }
    }
}