using UnityEngine;

//Class handling object pool funcs for the projectiles.
public class ProjectilesPool : MonoBehaviour
{
    [SerializeField]
    private GameObject[] projectilesPool;

    private void Start()
    {
        LevelManager.Instance.OnPlayerDead += ResetPool;
    }

    public GameObject GetAvailableProjectileFromPool()
    {
        for(int i = 0; i <= projectilesPool.Length - 1; i++)
        {
            if (projectilesPool[i].activeSelf == false)
                return projectilesPool[i];
        }

        Debug.Log("No projectile available. Add additional functionality to handle these cases. Some feedback.");
        return null;
    }

    private void ResetPool()
    {
        for(int i = 0; i <= projectilesPool.Length - 1; i++)
        {
            projectilesPool[i].SetActive(false);
        }
    }
}