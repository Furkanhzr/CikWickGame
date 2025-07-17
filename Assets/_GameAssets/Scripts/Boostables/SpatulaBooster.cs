using UnityEngine;
using UnityEngine.UIElements;

public class SpatulaBooster : MonoBehaviour, IBoostable
{
    [Header("References")]
    [SerializeField] private Animator _spatulaAnimator;

    [Header("Settings")]
    [SerializeField] private float _jumoForce;

    private bool _isActived;
    public void Boost(PlayerController playerController)
    {
        if (_isActived){return;}

        PlayBoostAnimation();
        Rigidbody playerRigidbody = playerController.GetPlayerRigidbody();
    
        playerRigidbody.linearVelocity = new Vector3(playerRigidbody.linearVelocity.x, 0f, playerRigidbody.linearVelocity.z);
        playerRigidbody.AddForce(transform.forward * _jumoForce, ForceMode.Impulse);
        _isActived = true;
        Invoke(nameof(ResetActivation), 0.2f);
    }

    private void PlayBoostAnimation()
    {
        _spatulaAnimator.SetTrigger(Consts.OtherAnimations.IS_SPATULA_JUMPING);
    }

    private void ResetActivation()
    {
        _isActived = false;
    }
}
