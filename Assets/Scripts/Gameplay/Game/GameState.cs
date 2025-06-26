using UnityEngine;
using Zenject;

public class GameState : MonoBehaviour
{
    private SceneTransistion _sceneTransition;

    [Inject]
    public void Construct(SceneTransistion sceneTransition)
    {
        _sceneTransition = sceneTransition;
    }

    public void Won()
    {
        _sceneTransition.Restart();
    }

    public void Loss()
    {
        _sceneTransition.Restart();
    }
}