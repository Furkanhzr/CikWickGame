using UnityEngine;
using UnityEngine.UI;

public class RottenWheatColectible : MonoBehaviour, ICollectible
{
    [SerializeField] private WheatDesignSO _wheatDesignSO; // Bu WheatDesignSO scriptable object'i ile bağlantılı olacak.

    [SerializeField] private PlayerController _playerController;

    [SerializeField] private PlayerStateUI _playerStateUI;

    private RectTransform _playerBoosterSpeedTransform;
    private Image _playerBoosterImage;

    private void Awake()
    {
        _playerBoosterSpeedTransform = _playerStateUI.GetBoosterSlowTransform();
        _playerBoosterImage = _playerBoosterSpeedTransform.GetComponent<Image>();
    }
    public void Collect()
    {
        _playerController.SetMovementSpeed(_wheatDesignSO.IncreaseDecreaseMultiplier, _wheatDesignSO.ResetBoostDuration);
        _playerStateUI.PlayerBoosterUInimation(_playerBoosterSpeedTransform, _playerBoosterImage, _playerStateUI.GetRottenBoosterWheatImage(), _wheatDesignSO.ActiveSprite, _wheatDesignSO.PassiveSprite, _wheatDesignSO.ActiveWheatSprite, _wheatDesignSO.PassiveWheatSprite, _wheatDesignSO.ResetBoostDuration);
        Destroy(this.gameObject);
    }
}
