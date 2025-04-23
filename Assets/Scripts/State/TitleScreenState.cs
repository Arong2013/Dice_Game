public class TitleScreenState : IGameState
{
    [Inject] private ITitleUI _titleUI;
    [Inject] private ISceneLoader _sceneLoader;

    public void Enter(GameStateMachine stateMachine)
    {
        _titleUI.OnContinueClicked += ContinueGame;
        _titleUI.Show();

    }
    public void Update()
    {

    }
    public void Exit()
    {
        _titleUI.OnContinueClicked -= ContinueGame;
        _titleUI.Hide();
    }
    private void ContinueGame()
    {
        _sceneLoader.Load(SceneName.Lobby);
    }
}
