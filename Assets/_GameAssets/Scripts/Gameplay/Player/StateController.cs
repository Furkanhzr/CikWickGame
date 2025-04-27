using UnityEngine;

public class StateController : MonoBehaviour
{
    private PlayerState _currentPlayerState = PlayerState.Idle;

    private void Start()
    {
        ChangeState(PlayerState.Idle);
    }
    public void ChangeState(PlayerState newPlayerState)
    {
        if (newPlayerState == _currentPlayerState) {return;}
        _currentPlayerState = newPlayerState;
        //Eğer newPlayerState ile _currentPlayerState eşitse _currentPlayerState değiştirme.
        //Zaten aynı ise gereksiz bir işlem yapılmış oluyor.
    }

    public PlayerState GetCurrentState() //Get metodu yazdık.
    {
        return _currentPlayerState;
    }
    //Encapsulation(kapsülleme) sağlıyor:
    //_currentPlayerState değişkenini doğrudan public yapmadan, kontrollü bir şekilde erişim veriyoruz.
}
