using UnityEngine;

public class DiceRollState : IGameState
{
    [Inject] ISignalBus signalBus;  
    [Inject] IBoardUI boardUI;  
    private GameStateMachine stateMachine;
    public void Enter(GameStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        signalBus.Subscribe<RollDiceSignal>(OnRollDice);    
        boardUI.Show();
    }
    public void Update()
    {
        
    }
    public void Exit()
    {
        UnityEngine.Debug.Log("DiceRollState: Exit");
    }
    public void OnRollDice(RollDiceSignal signal)
    {
        Debug.Log("주사위 돌릴게");
    }   
}