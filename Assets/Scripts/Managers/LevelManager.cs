using UnityEngine;
using UnityEngine.Events;

//Class handling player spawn and respawn.
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public UnityAction OnPlayerDamaged;
    public UnityAction OnPlayerDead;

    [HideInInspector]
    public Player PlayerSceneReference;

    [Header("Scene References")]
    public ProjectilesPool projectilesPool;
    [SerializeField]
    private Player playerPrefab;
    [SerializeField]
    private Transform playerSpawnPoint;

    private void Awake()
    {
        Instance = this;
        InstantiatePlayer();
    }

    public void TriggerPlayerDeath()
    {
        OnPlayerDead.Invoke();
        RespawnPlayer();
    }

    private void InstantiatePlayer()
    {
        if (playerPrefab == null)
            return;

        Player newPlayer = Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity);
        newPlayer.name = playerPrefab.name;
        PlayerSceneReference = newPlayer;
    }

    private void RespawnPlayer()
    {
        PlayerSceneReference.transform.position = playerSpawnPoint.position;
        PlayerSceneReference.PlayerLifeComponent.ResetPlayerLife();
    }
}