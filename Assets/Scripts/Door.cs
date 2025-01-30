using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private int m_roomIndex = 0;
    [SerializeField] private Sprite m_open;
    [SerializeField] private Sprite m_closed;
    [SerializeField] private GameObject m_player;

    EnemyManager m_enemyManager;
    CameraController m_cameraController;

    void Start()
    {
        m_enemyManager = GameObject.Find("Enemy Manager").GetComponent<EnemyManager>();
        m_cameraController = GameObject.Find("Main Camera").GetComponent<CameraController>();
        ClosedDoor();
    }

    void Update()
    {
        float cameraLeft = m_cameraController.GetLeft();
        float cameraRight = m_cameraController.GetRight();
        float cameraUp = m_cameraController.GetUp();
        float cameraDown = m_cameraController.GetDown();

        Vector2 currPos = transform.position;

        

        if ( currPos.x < cameraLeft || currPos.x > cameraRight || currPos.y > cameraUp || currPos.y < cameraDown)
        {
            // Outside of camera
            OpenDoor();
            return;
        }

        if (m_roomIndex < 0)
        {
            OpenDoor();
            return;
        }

        int enemyAlive = m_enemyManager.GetEnemyAliveAtRoom(m_roomIndex);
        if (enemyAlive <= 0)
            OpenDoor();
        else
        {
            if (m_cameraController.IsMoving())
            {
                if (m_player.GetComponent<PlayerController>().m_lastDir == 1)
                {
                    m_player.transform.position += transform.up / 250;
                }
                else if (m_player.GetComponent<PlayerController>().m_lastDir == 2)
                {
                    m_player.transform.position += -transform.up / 250;
                }
                else if (m_player.GetComponent<PlayerController>().m_lastDir == 3)
                {
                    m_player.transform.position += transform.right / 250;
                }
                else if (m_player.GetComponent<PlayerController>().m_lastDir == 4)
                {
                    m_player.transform.position += -transform.right / 250;
                }

            }
            else
                ClosedDoor();
        }
    }

    void OpenDoor()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = m_open;
        GetComponent<BoxCollider2D>().enabled = false;
    }

    void ClosedDoor()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = m_closed;
        GetComponent<BoxCollider2D>().enabled = true;
    }
}
