using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private PlayerController m_player;
    [SerializeField] private CameraController m_camera;
    [SerializeField] private GameObject m_start;
    [SerializeField] private GameObject m_wattfall;
    [SerializeField] private GameObject m_story;
    [SerializeField] private Vector2 m_scrollPosZero;
    [SerializeField] private Vector2 m_scrollPosOne;
    [SerializeField] private Vector2 m_scrollPosTwo;
    [SerializeField] private float m_delayOne = 10;
    [SerializeField] private float m_delayTwo = 1;
    [SerializeField] private float m_scrollSpeed = 240f;

    private bool m_waiting = false;
    private bool m_fade = false;
    private bool m_scroll = false;
    private bool m_scrollAway = false;

    private void Start()
    {
        StartCoroutine(Delay(m_delayOne));
        m_player.SetControllable(false);
        m_camera.OpenCurtain();
    }

    private void Update()
    {
        if (m_fade && !m_scroll && !m_waiting)
        {
            StartCoroutine(Delay(m_delayTwo));
            m_start.GetComponent<Image>().color = new Color(0, 0, 0, 255);
            m_wattfall.SetActive(false);
        }

        if (m_scroll && m_story.GetComponent<RectTransform>().anchoredPosition != m_scrollPosOne && !m_scrollAway)
        {
            m_story.GetComponent<RectTransform>().anchoredPosition = Vector2.MoveTowards(m_story.GetComponent<RectTransform>().anchoredPosition, m_scrollPosOne, m_scrollSpeed * Time.deltaTime);
        }

        if (m_story.GetComponent<RectTransform>().anchoredPosition == m_scrollPosOne && !m_waiting)
        {
            StartCoroutine(Delay(5f));
        }

        if (m_scrollAway && m_story.GetComponent<RectTransform>().anchoredPosition != m_scrollPosTwo)
        {
            m_story.GetComponent<RectTransform>().anchoredPosition = Vector2.MoveTowards(m_story.GetComponent<RectTransform>().anchoredPosition, m_scrollPosTwo, m_scrollSpeed * Time.deltaTime);
        }

        if (m_story.GetComponent<RectTransform>().anchoredPosition == m_scrollPosTwo && !m_waiting)
        {
            m_start.GetComponent<Image>().color = new Color(255, 255, 255, 255);
            m_wattfall.SetActive(true);
            m_story.GetComponent<RectTransform>().anchoredPosition = m_scrollPosZero;
            StartCoroutine(Delay(m_delayOne));

        }

        //
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_player.SetControllable(true);
            gameObject.SetActive(false);
            m_camera.OpenCurtain();
        }
    }
    private IEnumerator Delay(float delay)
    {
        m_waiting = true;
        yield return new WaitForSeconds(delay);
        m_waiting = false;

        if (!m_fade) 
        {
            m_fade = true;
        }
        else if(m_fade && !m_scroll)
        {
            m_scroll = true;
        }
        else if (!m_scrollAway && m_fade && m_scroll)
        {
            m_scrollAway = true;
        }
        else if (m_scrollAway && m_fade && m_scroll)
        {
            m_scrollAway = false;
            m_scroll = false;
            m_fade = false;
        }
    }
}
