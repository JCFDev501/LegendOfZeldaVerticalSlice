using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    [SerializeField] private Sprite m_open;
    [SerializeField] private Inventory m_inventory;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Player") && gameObject.GetComponent<SpriteRenderer>().sprite != m_open)
        {
            if (m_inventory.GetKeys() > 0)
            {
                m_inventory.UpdateKeys(-1);
                gameObject.GetComponent<SpriteRenderer>().sprite = m_open;
                GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }
}
