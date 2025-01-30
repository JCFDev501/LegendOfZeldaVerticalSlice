using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairAnim : MonoBehaviour
{
    [SerializeField] private GameObject m_floor;
    [SerializeField] private GameObject m_player;
     
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Player"))
        {
            m_player = collision.gameObject;
            m_floor.GetComponent<SpriteRenderer>().sortingOrder = 3;
            collision.GetComponent<PlayerController>().WalkDownStiars();
            StartCoroutine(WalkDownStairs());
        }
    }

    private IEnumerator WalkDownStairs()
    {
        yield return new WaitForSeconds(5f);
        m_floor.GetComponent<SpriteRenderer>().sortingOrder = -1;
        m_player.GetComponent<PlayerController>().SetGliding(false);
    }
}
