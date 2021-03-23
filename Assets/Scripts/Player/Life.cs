using UnityEngine;

//Class handling the player health.
public class Life : MonoBehaviour
{
    public int CurrentPlayerHealth { get; private set; }

    [SerializeField]
    private Player playerScriptReference;
    private const int InitialHealth = 3;

    private void OnEnable()
    {
        CurrentPlayerHealth = InitialHealth;
    }

    public void Damage()
    {
        CurrentPlayerHealth -= 1;

        if (CurrentPlayerHealth < 0)
            CurrentPlayerHealth = 0;

        if (CurrentPlayerHealth <= 0)
        {
            if (playerScriptReference != null)
            {
                LevelManager.Instance.TriggerPlayerDeath();
                return;
            }
        }

        LevelManager.Instance.OnPlayerDamaged.Invoke();
    }

    public void ResetPlayerLife()
    {
        CurrentPlayerHealth = InitialHealth;
    }
}