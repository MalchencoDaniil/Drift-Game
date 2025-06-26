using Zenject;

public class GameplayInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<SceneTransistion>().FromComponentInHierarchy().AsSingle();
        Container.Bind<InputManager>().FromComponentInHierarchy().AsSingle();
    }
}