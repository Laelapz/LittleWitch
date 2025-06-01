using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public int maxHealth = 3;
    public float immunityTime = 1f;
    [SerializeField] private int currentHealth;
    private bool isImmune = false;
    private float immuneTimer = 0f;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (isImmune)
        {
            immuneTimer -= Time.deltaTime;
            if (immuneTimer <= 0f)
            {
                isImmune = false;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (isImmune) return;

        currentHealth -= damage;
        Debug.Log("Player took damage. Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        isImmune = true;
        immuneTimer = immunityTime;

    }

    void Die()
    {
        Debug.Log("Player died!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
