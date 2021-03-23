using UnityEngine;

//Class handling player death trigger.
public class DeathTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
            LevelManager.Instance.TriggerPlayerDeath();
    }
}