using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private GameObject m_player;
    [SerializeField] private Inventory m_inventory;
    [SerializeField] private GameObject m_camera;
    [SerializeField] private Vector3 m_camNewPos;
    [SerializeField] private Vector3 m_playerNewPos;
    [SerializeField] private string m_textToShow;
    [SerializeField] private bool m_clearText;
    [SerializeField] private bool m_toggleOverworld;
    [SerializeField] private bool m_pushFromBottom;

    // This should be only true when you are teleport on top
    // of a freezeCamera script and you are walking out of it
    [SerializeField] private bool m_freezeCamera;

    private bool m_isOpeningCurtain = false;
    private bool m_isClosingCurtain = false;
    private float m_waitTime = 2f;
    private float m_timer = 0f;
    private Text m_textUI;

    private void Start()
    {
        m_textUI = GameObject.Find("Canvas").GetComponentInChildren<Text>();
    }

    private void Update()
    {
        if(m_isClosingCurtain)
        {
            m_timer += Time.deltaTime;
            if(m_timer > m_waitTime)
            {
                m_timer = 0;
                m_isClosingCurtain = false;
                m_isOpeningCurtain = true;

                m_camera.GetComponent<CameraController>().OpenCurtain();
                m_player.transform.position = m_playerNewPos;
                m_camera.transform.position = m_camNewPos;
                if (m_pushFromBottom)
                    m_player.GetComponent<Rigidbody2D>().velocity = Vector3.up;
                else
                    m_player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;

                if (m_freezeCamera)
                    m_camera.GetComponent<CameraController>().SetFrozen(true);
            }
        }

        if(m_isOpeningCurtain)
        {
            m_timer += Time.deltaTime;
            if (m_timer > m_waitTime)
            {
                if(m_textToShow != "")
                    StartCoroutine(m_textUI.ShowText(m_textToShow));
                else
                    m_player.GetComponent<PlayerController>().SetControllable(true);

                m_isOpeningCurtain = false;
                m_camera.GetComponent<CameraController>().SetFrozen(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (m_pushFromBottom)
            {
                m_player.GetComponent<PlayerController>().SetGliding(true);
/*                m_camera.GetComponent<CameraController>().SetFrozen(true);*/
                StartCoroutine(Wait());
            }

            m_isClosingCurtain = true;
            m_camera.GetComponent<CameraController>().CloseCurtain();
            m_player.GetComponent<PlayerController>().SetControllable(false);
            m_timer = 0f;

            if (m_clearText)
                m_textUI.ClearText();

            m_inventory.SetInOverworld(m_toggleOverworld);
        }
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(3f);
        m_player.GetComponent<PlayerController>().SetGliding(false);
/*        yield return new WaitForSeconds(5f);
        m_camera.GetComponent<CameraController>().SetFrozen(false);*/
    }

    public void SetText(string set)
    {
        m_textToShow = set;  
    }
}
