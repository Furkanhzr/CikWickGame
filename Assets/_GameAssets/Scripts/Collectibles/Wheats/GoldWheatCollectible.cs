using UnityEngine;
using UnityEngine.UI;

public class GoldWheatCollectible : MonoBehaviour, ICollectible
{
    [SerializeField] private WheatDesignSO _wheatDesignSO; // Bu WheatDesignSO scriptable object'i ile bağlantılı olacak.

    [SerializeField] private PlayerController _playerController;
    //[SerializeField] private float _movementIncreaseSpeed;
    //[SerializeField] private float _restBoostDuration;
    //SO'dan sonra bunları kaldırdık artık.
    [SerializeField] private PlayerStateUI _playerStateUI; // PlayerStateUI referansı ekledik, UI güncellemeleri için.

    private RectTransform _playerBoosterSpeedTransform;
    private Image _playerBoosterImage;

    private void Awake()
    {
        _playerBoosterSpeedTransform = _playerStateUI.GetBoosterSpeedTransform();
        _playerBoosterImage = _playerBoosterSpeedTransform.GetComponent<Image>();
    }
    public void Collect()
    {
        //_playerController.SetMovementSpeed(_movementIncreaseSpeed, _restBoostDuration);
        //SO'dan sonra bunları kaldırdık artık.
        _playerController.SetMovementSpeed(_wheatDesignSO.IncreaseDecreaseMultiplier, _wheatDesignSO.ResetBoostDuration);
        _playerStateUI.PlayerBoosterUInimation(_playerBoosterSpeedTransform, _playerBoosterImage, _playerStateUI.GetGoldBoosterWheatImage(), _wheatDesignSO.ActiveSprite, _wheatDesignSO.PassiveSprite, _wheatDesignSO.ActiveWheatSprite, _wheatDesignSO.PassiveWheatSprite, _wheatDesignSO.ResetBoostDuration);

        Destroy(this.gameObject); //Destroy(gameObject); direkt böyle de yazılabilir buradaki gameOnject'i yok et anlamına geliyor zaten.
    }
}
