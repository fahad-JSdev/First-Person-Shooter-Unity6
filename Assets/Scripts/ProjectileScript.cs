using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
     //public GameObject enemyComponent;
     //public EnemyScript enemyScript;
    void Start()
    {
         //enemyScript = enemyComponent.GetComponent<EnemyScript>();
        Destroy(gameObject, 3f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //enemyScript.TakeDamage(10f);
            // Destroy the projectile when it collides with the player
            Destroy(gameObject);
        }
    }
}
