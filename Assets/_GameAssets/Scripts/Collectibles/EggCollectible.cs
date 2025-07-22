using UnityEngine;

public class EggColectible : MonoBehaviour, ICollectible
{
   public void Collect()
    {
        GameManager.Instance.OnEggCollected();
        Destroy(gameObject);
    }
}
