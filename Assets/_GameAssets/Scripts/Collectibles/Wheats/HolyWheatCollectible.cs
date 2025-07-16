using UnityEngine;

public class HolyWheatCollectible : MonoBehaviour, ICollectible
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private float _forceIncrease;
    [SerializeField] private float _restBoostDuration;

    public void Collect()
    {
        _playerController.SetJumpForce(_forceIncrease, _restBoostDuration);
        Destroy(this.gameObject);
    }
}
