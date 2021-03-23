using UnityEngine;

public enum EnemyTriggerBoxType
{
    AttackTrigger,
    VulnerableTrigger
}

//Class handling enemy triggers func.
public class EnemyTriggerBox : MonoBehaviour
{
    [SerializeField]
    private EnemyTriggerBoxType triggerBoxType;

    private Enemy enemyController;

    private void OnEnable()
    {
        enemyController = transform.parent.GetComponent<Enemy>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player")
            return;

        switch (triggerBoxType)
        {
            case EnemyTriggerBoxType.AttackTrigger:
                enemyController.TriggerDamagePlayer();
                break;
            case EnemyTriggerBoxType.VulnerableTrigger:
                enemyController.TriggerEnemyDeath();
                break;
        }
    }
}