using UnityEngine;

public enum ProjectileInteractionType
{
    DamagePlayer,
    KillEnemy
}

//Class handling individual projectile functions.
public class Projectile : MonoBehaviour
{
    private ProjectileInteractionType projectileInteractionType;
    private float timeUntilProjectileReset = 5f;
    private Vector3 projectileDirection;
    private float projectileSpeed = 5f;
    private bool isProjectileReleased;

    private void Update()
    {
        if(isProjectileReleased)
            transform.position += projectileDirection * projectileSpeed * Time.deltaTime;
    }

    public void ReleaseProjectile(ProjectileInteractionType interactionType, Vector3 initialPosition, Vector3 targetDirection)
    {
        projectileInteractionType = interactionType;
        isProjectileReleased = true;
        transform.position = initialPosition;
        gameObject.SetActive(true);
        projectileDirection = targetDirection;
        Invoke(nameof(ResetProjectile), timeUntilProjectileReset);
    }

    private void ResetProjectile()
    {
        isProjectileReleased = false;
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Projectile")
            return;

        switch (projectileInteractionType)
        {
            case ProjectileInteractionType.KillEnemy:
                if(collision.tag == "Enemy")
                {
                    collision.GetComponent<Enemy>().TriggerEnemyDeath(); //ERROR OCCURED HERE. CHECK!!!!!!!!!!!!!!!
                    ResetProjectile();
                }
                break;
            case ProjectileInteractionType.DamagePlayer:
                if(collision.tag == "Player")
                {
                    collision.GetComponent<Life>().Damage();
                    ResetProjectile();
                }
                break;
        }
    }
}