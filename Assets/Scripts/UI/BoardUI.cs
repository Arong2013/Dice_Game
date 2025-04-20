using UnityEngine;
using UnityEngine.UI;
public class RollDiceSignal { }

[AutoRegisterInContainer]
public class BoardUI : MonoBehaviour, IBoardUI
{
    [Inject] ISignalBus signalBus;    
    public void Show()
    {
        gameObject.SetActive(true);
        signalBus.Fire(new RollDiceSignal());    
    }
    public void Hide() => gameObject.SetActive(false);
    public void RollDice(int value)
    {
        
    }

    public void MovePlayer(int steps)
    {
      
    }
    public void UpdatePlayerPosition(int tileIndex)
    {
        
    }
}
