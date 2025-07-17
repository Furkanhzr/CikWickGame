using UnityEngine;

public class GoldWheatCollectible : MonoBehaviour, ICollectible
{
    [SerializeField] private WheatDesignSO _wheatDesignSO; // Bu WheatDesignSO scriptable object'i ile bağlantılı olacak.

    [SerializeField] private PlayerController _playerController;
    //[SerializeField] private float _movementIncreaseSpeed;
    //[SerializeField] private float _restBoostDuration;
    //SO'dan sonra bunları kaldırdık artık.

    public void Collect()
    {
        //_playerController.SetMovementSpeed(_movementIncreaseSpeed, _restBoostDuration);
        //SO'dan sonra bunları kaldırdık artık.
        _playerController.SetMovementSpeed(_wheatDesignSO.IncreaseDecreaseMultiplier, _wheatDesignSO.ResetBoostDuration);
        Destroy(this.gameObject); //Destroy(gameObject); direkt böyle de yazılabilir buradaki gameOnject'i yok et anlamına geliyor zaten.
    }
}
