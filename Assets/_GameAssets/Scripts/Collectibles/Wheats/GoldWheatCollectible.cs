using UnityEngine;

public class GoldWheatCollectible : MonoBehaviour, ICollectible
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private float _movementIncreaseSpeed;
    [SerializeField] private float _restBoostDuration;

    public void Collect()
    {
        _playerController.SetMovementSpeed(_movementIncreaseSpeed, _restBoostDuration);
        Destroy(this.gameObject); //Destroy(gameObject); direkt böyle de yazılabilir buradaki gameOnject'i yok et anlamına geliyor zaten.
    }
}
