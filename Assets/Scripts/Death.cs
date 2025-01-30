using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Death : MonoBehaviour
{
    [SerializeField] private GameObject m_player;
    [SerializeField] private GameObject m_camera;
    [SerializeField] private Inventory m_inventory;
    [SerializeField] private GameObject m_respawnPoint;
    [SerializeField] private GameObject m_heartSelector;
    [SerializeField] private GameObject m_mainMenu;
    [SerializeField] private GameObject m_continue;
    [SerializeField] private GameObject m_save;
    [SerializeField] private GameObject m_retry;

    private bool m_done = false;

    // Update is called once per frame
    void Update()
    {
        m_player.GetComponent<PlayerController>().SetControllable(false);

        Vector2 selectorLocation = m_heartSelector.GetComponent<RectTransform>().anchoredPosition;

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (selectorLocation.y + 50 <= 244.25)
            {
                selectorLocation.y += 50;
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (selectorLocation.y - 50 >= 144.25)
            {
                selectorLocation.y -= 50;
            }
        }
        m_heartSelector.GetComponent<RectTransform>().anchoredPosition = selectorLocation;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (selectorLocation.y == 244.25) 
            {
                StartCoroutine(Flash(0.2f, m_continue));
                StartCoroutine(Delay(1));
            }
            else if (selectorLocation.y == 194.25)
            {
                StartCoroutine(Flash(0.2f, m_save));
                StartCoroutine(Delay(1));
                m_inventory.SaveToJson();
            }
            else if (selectorLocation.y <= 144.25)
            {
                StartCoroutine(Flash(0.2f, m_retry));
                StartCoroutine(Delay(1, true));
            }
        }
    }

    private IEnumerator Flash(float delay, GameObject gameObject)
    {
        gameObject.GetComponent<TMP_Text>().color = Color.red;
        yield return new WaitForSeconds(delay);
        gameObject.GetComponent<TMP_Text>().color = Color.white;
        yield return new WaitForSeconds(delay);
        gameObject.GetComponent<TMP_Text>().color = Color.red;
        yield return new WaitForSeconds(delay);
        gameObject.GetComponent<TMP_Text>().color = Color.white;
    }

    private IEnumerator Delay(float delay, bool mainMenu = false)
    {
        yield return new WaitForSeconds(delay);
        m_player.transform.position = m_respawnPoint.transform.position;
        m_camera.transform.position = m_respawnPoint.transform.position + new Vector3(0, 0, -10);
        m_player.GetComponent<PlayerController>().SetControllable(true);
        m_player.GetComponent<Health>().UpdateHealth(6);
        m_player.GetComponent<SpriteRenderer>().color = Color.white;
        m_player.GetComponent<Animator>().Play("Idle");
        gameObject.SetActive(false);
        if (mainMenu)
        {
            m_inventory.ResetStats();
            m_mainMenu.SetActive(true);
        }
    }

}
