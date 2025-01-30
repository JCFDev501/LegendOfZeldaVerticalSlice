using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private GameObject m_item;
    [SerializeField] private Inventory m_inventory;
    [SerializeField] private int m_price;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Player") )
        {
            if (m_inventory.GetRupies() >= m_price)
            {
                m_inventory.UpdateRupies(-m_price);
                Instantiate(m_item, transform.position, transform.rotation);
                //Destroy(gameObject);
            }
        }
    }
}
