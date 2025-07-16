using UnityEngine;

public class RottenWheatColectible : MonoBehaviour, ICollectible
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private float _movementDecraseSpeed;
    [SerializeField] private float _restBoostDuration;

    public void Collect()
    {
        _playerController.SetMovementSpeed(_movementDecraseSpeed, _restBoostDuration);
        Destroy(this.gameObject);
    }
}
