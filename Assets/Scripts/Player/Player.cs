using UnityEngine;

//Class handling player movement.
public class Player : MonoBehaviour
{
    [HideInInspector]
    public Life PlayerLifeComponent;

    [Header("Movement Parameters")]
    [SerializeField]
    private float playerWalkSpeed = 4f;
    [SerializeField]
    private float playerSprintSpeed = 8f;
    [SerializeField]
    private float playerWalkAcceleration = 75f;
    [SerializeField]
    private float playerAirAcceleration = 30f;
    [SerializeField]
    private float playerGroundDeceleration = 70f;

    [SerializeField]
    private float playerJumpHeight = 2f;

    private ProjectilesPool projectilesPool;
    private BoxCollider2D playerCollider;
    private Vector2 playerVelocity;
    private bool IsPlayerGrounded;

    private Vector2 lastRecordedPlayerDirection = Vector2.right;

    private void OnEnable()
    {
        PlayerLifeComponent = GetComponent<Life>();
        playerCollider = GetComponent<BoxCollider2D>();
        projectilesPool = LevelManager.Instance.projectilesPool;
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");

        //Attack.
        if (Input.GetKeyDown(KeyCode.E))
            AttackWithProjectile();

        if (IsPlayerGrounded)
        {
            playerVelocity.y = 0;

            //Jump.
            if (Input.GetKeyDown(KeyCode.Space))
            {
                playerVelocity.y = Mathf.Sqrt(2 * playerJumpHeight * Mathf.Abs(Physics2D.gravity.y));
            }
        }

        float acceleration = IsPlayerGrounded ? playerWalkAcceleration : playerAirAcceleration;
        float deceleration = IsPlayerGrounded ? playerGroundDeceleration : 0;

        if (moveInput != 0)
        {
            if(Input.GetKey(KeyCode.LeftShift))
                playerVelocity.x = Mathf.MoveTowards(playerVelocity.x, playerSprintSpeed * moveInput, acceleration * Time.deltaTime);
            else
                playerVelocity.x = Mathf.MoveTowards(playerVelocity.x, playerWalkSpeed * moveInput, acceleration * Time.deltaTime);

            lastRecordedPlayerDirection = moveInput == 1 ? Vector2.right : -Vector2.right;
        }
        else
            playerVelocity.x = Mathf.MoveTowards(playerVelocity.x, 0, deceleration * Time.deltaTime);

        playerVelocity.y += Physics2D.gravity.y * Time.deltaTime;

        transform.Translate(playerVelocity * Time.deltaTime);

        IsPlayerGrounded = false;

        //Retrieve all colliders we have intersected after velocity has been applied.
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, playerCollider.size, 0, LayerMask.GetMask("Default"));

        foreach (Collider2D hit in hits)
        {
            if (hit == playerCollider)
                continue;

            if (hit.tag == "SystemAsset")
                continue;

            ColliderDistance2D colliderDistance = hit.Distance(playerCollider);

            if (colliderDistance.isOverlapped)
            {
                transform.Translate(colliderDistance.pointA - colliderDistance.pointB);

                //If we intersect an object beneath us, set grounded to true. 
                if (Vector2.Angle(colliderDistance.normal, Vector2.up) < 90 && playerVelocity.y < 0)
                {
                    IsPlayerGrounded = true;
                }
            }
        }
    }

    private void AttackWithProjectile()
    {
        GameObject availableProjectile = projectilesPool.GetAvailableProjectileFromPool();

        if(availableProjectile == null)
        {
            Debug.Log("Add additional functionality!");
            return;
        }

        Vector2 shootingDirection = lastRecordedPlayerDirection;

        Projectile currentProjectile = availableProjectile.GetComponent<Projectile>();
        currentProjectile.ReleaseProjectile(ProjectileInteractionType.KillEnemy, playerCollider.bounds.center, shootingDirection);
    }
}