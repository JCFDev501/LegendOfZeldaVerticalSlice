using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearText : MonoBehaviour
{
    [SerializeField] private GameObject m_oldman;
    [SerializeField] private GameObject m_teleporter;

    private Text m_textUI;

    private void Start()
    {
        m_textUI = GameObject.Find("Canvas").GetComponentInChildren<Text>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            m_textUI.ClearText();
            m_oldman.SetActive(false);
            m_teleporter.GetComponent<Teleporter>().SetText("");
        }
    }

}
