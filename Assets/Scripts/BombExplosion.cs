using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplosion : MonoBehaviour
{
    public float fuseDuration = 3f; // Duration in seconds before the bomb explodes
    public float explosionRadius = 1.5f; // Radius of the bomb's explosion
    public string destroyTag = "Destroyable"; // Tag of the GameObjects to destroy
    public string enemyTag = "Enemy"; // Tag of the GameObjects to destroy

    public GameObject explosionPrefab; // Prefab for the explosion effect

    private bool exploded = false;

    private void Start()
    {
        StartCoroutine(ExplodeAfterDelay(fuseDuration));
    }

    private IEnumerator ExplodeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        Explode();
    }

    private void Explode()
    {
        if (exploded)
            return;

        exploded = true;

        // Instantiate the explosion effect
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        // Detect objects in the explosion radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        // Apply damage or other effects to the affected objects
        foreach (Collider2D collider in colliders)
        {
            // Check if the collider has the specified tag
            if (collider.CompareTag(destroyTag))
            {
                // Destroy the GameObject
                Destroy(collider.gameObject);
            }
            else if (collider.CompareTag(enemyTag))
            {
                collider.GetComponent<EnemyAIController>().UpdateHealth(-2);
            }
        }

        // Destroy the bomb object
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the bomb collides with anything, immediately explode
        if (!exploded)
        {
            //Explode();
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a gizmo to visualize the explosion radius in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
