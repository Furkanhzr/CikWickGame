using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private int _maxHealth;

    private int _currentHealth;

    private void Start()
    {
        _currentHealth = _maxHealth;
    }

    public void Damage(int damage)
    {
        if (damage < 0)
        {
            _currentHealth -= damage;
            //TODO: UI Animate Damage
            if (_currentHealth <= 0)
            {
                //TODO: Player has died
            }
        }
        
    }

    public void Heal(int healAmount)
    {
        //if(_currentHealth + healAmount > _maxHealth)
        //{
        //    _currentHealth = _maxHealth;
        //}
        //else
        //{
        //    _currentHealth += healAmount;
        //}

        //veya

        _currentHealth = Mathf.Min(_currentHealth + healAmount, _maxHealth);
    }
}
