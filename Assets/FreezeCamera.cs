using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeCamera : MonoBehaviour
{
    [SerializeField] private GameObject m_camera;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.transform.position.y > transform.position.y)
        {
            m_camera.GetComponent<CameraController>().SetFrozen(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.transform.position.y > transform.position.y)
        {
            m_camera.GetComponent<CameraController>().SetFrozen(false);
        }
    }
}
