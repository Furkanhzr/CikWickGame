using UnityEngine;

public class HolyWheatCollectible : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private float _forceIncreased;
    [SerializeField] private float _restBoostDuration;

    public void Collect()
    {
        _playerController.SetJumpForce(_forceIncreased, _restBoostDuration);
        Destroy(this.gameObject);
    }
}
