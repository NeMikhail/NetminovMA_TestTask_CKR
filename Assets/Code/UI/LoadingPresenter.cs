using Core.Interface;
using Zenject;

public class LoadingPresenter : IInitialisation
{
    private SceneView _sceneView;

    [Inject]
    public void Construct(SceneView sceneView)
    {
        _sceneView = sceneView;
    }

    public void Initialisation()
    {
        _sceneView.LoadingScreen.SetActive(false);
    }
}
