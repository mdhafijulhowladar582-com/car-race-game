using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;

    public int CurrentHealth { get; private set; }

    private void Awake()
    {
        CurrentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (damage <= 0 || CurrentHealth <= 0)
            return;

        CurrentHealth = Mathf.Max(0, CurrentHealth - damage);

        AudioManager.GetOrCreate().PlayDamage();

        if (GameManager.Instance != null)
            GameManager.Instance.UpdateHealth(CurrentHealth);

        if (CurrentHealth <= 0)
            GameManager.Instance?.GameOver();
    }
}
