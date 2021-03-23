using UnityEngine;

public enum EnemyAIState
{
    StayIdle,
    EnemyMove
}

//Class handling enemy movement.
public class Enemy : MonoBehaviour
{
    [Header("AI States Percentages")]
    [SerializeField, Range(1f, 100f)]
    private float chanceToMoveEnemy = 70f;

    [SerializeField]
    private Transform groundDetectionChecker;

    private EnemyAIState currentEnemyState;
    private float enemyWalkSpeed = 2f;
    private BoxCollider2D enemyCollider;
    private ProjectilesPool projectilesPool;
    private Player playerReference;
    private Vector2 lastRecordedEnemyDirection = -Vector2.right;

    private bool isMovingRight;

    private void Start()
    {
        playerReference = LevelManager.Instance.PlayerSceneReference;
        projectilesPool = LevelManager.Instance.projectilesPool;
        enemyCollider = GetComponent<BoxCollider2D>();

        InvokeRepeating(nameof(EnemyProjectileAttack), 1f, Random.Range(1f, 8f));
        InvokeRepeating(nameof(SetNewEnemyState), 1f, Random.Range(2f, 5f));
    }

    private void Update()
    {
        EnemyMovement();
    }

    public void TriggerEnemyDeath()
    {
        CancelInvoke();
        gameObject.SetActive(false);
    }

    public void TriggerDamagePlayer()
    {
        playerReference.PlayerLifeComponent.Damage();
    }

    private void EnemyMovement()
    {
        if (currentEnemyState == EnemyAIState.StayIdle)
            return;

        transform.Translate(Vector2.right * enemyWalkSpeed * Time.deltaTime);

        RaycastHit2D groundHit = Physics2D.Raycast(groundDetectionChecker.position, Vector2.down, 1f);

        if (!groundHit.collider)
        {
            if (isMovingRight)
            {
                lastRecordedEnemyDirection = -Vector2.right;
                transform.eulerAngles = new Vector3(0, 180, 0);
                isMovingRight = false;
            }
            else
            {
                lastRecordedEnemyDirection = Vector2.right;
                transform.eulerAngles = new Vector3(0, 0, 0);
                isMovingRight = true;
            }
        }
    }

    private void SetNewEnemyState()
    {
        float randomValue = Random.Range(0f, 100f);

        if (randomValue < chanceToMoveEnemy)
        {
            currentEnemyState = EnemyAIState.EnemyMove;
            return;
        }

        currentEnemyState = EnemyAIState.StayIdle;
    }

    private void EnemyProjectileAttack()
    {
        GameObject availableProjectile = projectilesPool.GetAvailableProjectileFromPool();

        if (availableProjectile == null)
        {
            Debug.Log("Add additional functionality!");
            return;
        }

        Vector2 shootingDirection = lastRecordedEnemyDirection;

        Projectile currentProjectile = availableProjectile.GetComponent<Projectile>();
        currentProjectile.ReleaseProjectile(ProjectileInteractionType.DamagePlayer, enemyCollider.bounds.center, shootingDirection);
    }
}