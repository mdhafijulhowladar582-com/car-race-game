using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private int damage = 1;

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("Player"))
            return;

        AudioManager.GetOrCreate().PlayCrash();

        PlayerHealth health = collision.collider.GetComponent<PlayerHealth>();

        if (health != null)
            health.TakeDamage(damage);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        AudioManager.GetOrCreate().PlayCrash();

        PlayerHealth health = other.GetComponent<PlayerHealth>();

        if (health != null)
            health.TakeDamage(damage);
    }
}
