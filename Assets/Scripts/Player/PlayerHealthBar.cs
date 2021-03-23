using UnityEngine;
using UnityEngine.UI;

//Class handling the player health bar.
public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField]
    private Image[] heartImages;

    private Life playerLifeComponent;
    private Color heartActiveColor = Color.red;
    private Color heartUnactiveColor = Color.grey;

    private void Start()
    {
        LevelManager.Instance.OnPlayerDamaged += UpdateHealthBar;
        LevelManager.Instance.OnPlayerDead += ResetPlayerHealth;
        playerLifeComponent = LevelManager.Instance.PlayerSceneReference.GetComponent<Life>();
        ResetPlayerHealth();
    }

    private void UpdateHealthBar()
    {
        if (playerLifeComponent.CurrentPlayerHealth <= 0)
        {
            ResetPlayerHealth();
            return;
        }

        GetNextAvailableHeart().color = heartUnactiveColor;
    }

    private Image GetNextAvailableHeart()
    {
        for (int i = heartImages.Length - 1; i >= 0; i--)
        {
            if (heartImages[i].color == heartActiveColor)
            {
                return heartImages[i];
            }
        }

        return heartImages[heartImages.Length - 1];
    }

    private void ResetPlayerHealth()
    {
        for(int i = 0; i <= heartImages.Length - 1; i++)
        {
            heartImages[i].color = heartActiveColor;
        }
    }
}