using UnityEngine;

//Class handling camera movement and player follow.
public class CameraManager : MonoBehaviour
{
    [Header("Camera Movement Parameters")]
    [SerializeField]
    private float cameraMovementSpeed = 0.3f;
    [SerializeField]
    private float resetMovementSpeed = 0.5f;

    //Minimal distance necessary for moving.
    [SerializeField]
    private float moveForwardTrigger = 0.1f;

    private Transform cameraTarget;

    private float offsetZ = -20f;
    private Vector3 lastCameraTargetPosition;
    private Vector3 currentVelocity;
    private Vector3 moveForwardPosition;

    private void Start()
    {
        if (LevelManager.Instance.PlayerSceneReference == null)
            return;

        AssignCameraTarget();

        //Store player's last position.
        lastCameraTargetPosition = cameraTarget.position;
        transform.parent = null;
    }

    private void AssignCameraTarget()
    {
        cameraTarget = LevelManager.Instance.PlayerSceneReference.transform;
    }

    private void LateUpdate()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        //Check if the player has moved.
        float xMoveDelta = (cameraTarget.position - lastCameraTargetPosition).x;
        bool shouldUpdateMoveForwardTarget = Mathf.Abs(xMoveDelta) > moveForwardTrigger;

        if (shouldUpdateMoveForwardTarget)
        {
            moveForwardPosition = Vector3.right * Mathf.Sign(xMoveDelta);
        }
        else
        {
            moveForwardPosition = Vector3.MoveTowards(moveForwardPosition, Vector3.zero, Time.deltaTime * resetMovementSpeed);
        }

        Vector3 aheadTargetPos = cameraTarget.position + moveForwardPosition + Vector3.forward * offsetZ;

        Vector3 newCameraPosition = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref currentVelocity, cameraMovementSpeed);
        Vector3 shakeFactorPosition = Vector3.zero;

        transform.position = newCameraPosition;

        lastCameraTargetPosition = cameraTarget.position;
    }
}