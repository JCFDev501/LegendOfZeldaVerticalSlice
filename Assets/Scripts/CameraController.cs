using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Things to note:
 *  - Maybe could change to a trigger box where when enemy enter, set them to be
 *    able to update, or when they exit, set them to not updateable
*/

/* 
 * Description:
 * 
 * When the player first goes out of bound:
 *  - m_movingCamera becomes true 
 *  - PlayerController's m_controlable become false
 *  - Starts moving the camera to the desired location
 *  
 * When the player goes out of bound when camera is moving
 *  - Push the player back into bound (this is how the acutal game works)
*/

public class CameraController : MonoBehaviour
{
    [SerializeField] private float m_cameraSpeed = 5f;
    [SerializeField] private float m_curtainSpeed = 5f;
    [SerializeField] private GameObject m_player;
    [SerializeField] private GameObject m_leftCurtain;
    [SerializeField] private GameObject m_rightCurtain;

    Vector3 m_cameraGoalPos = new Vector3();

    // Use to check if the player goes out of bound when ccamera is moving
    bool m_movingCamera = false;
    bool m_openingCurtain = false;
    bool m_closingCurtain = false;
    bool m_playerDied = false;
    bool m_frozen = false;

    Rigidbody2D m_leftCurtainRB;
    Rigidbody2D m_rightCurtainRB;

    Health m_playerHealth;

    void Start()
    {
        m_leftCurtainRB = m_leftCurtain.GetComponent<Rigidbody2D>();
        m_rightCurtainRB = m_rightCurtain.GetComponent<Rigidbody2D>();
        m_playerHealth = m_player.GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_playerHealth && m_playerHealth.GetCurrentHealth() <= 0)
            m_playerDied = true;
        else
            m_playerDied = false;

        CheckIfOpenCurtain();
        CheckIfCloseCurtain();

        if (!m_player)
            return;

        if (!m_frozen)
            CheckPlayerPosition();
    }

    private void CheckPlayerPosition()
    {
        // Get the camera size and position
        Vector3 currPos = transform.position;
        Camera camera = GetComponent<Camera>();
        float cameraOrthographSize = camera.orthographicSize;
        float left = currPos.x - cameraOrthographSize;
        float right = currPos.x + cameraOrthographSize;
        float up = currPos.y + cameraOrthographSize / 2;
        float down = currPos.y - cameraOrthographSize;

        // Get the player position and size
        Transform playerTransform = m_player.GetComponent<Transform>();
        Vector3 playerPos = playerTransform.position;
        Vector3 playerScale = playerTransform.localScale / 10;
        float playerLeft = playerPos.x - playerScale.x;
        float playerRight = playerPos.x + playerScale.x;
        float playerUp = playerPos.y + playerScale.y;
        float playerDown = playerPos.y - playerScale.y;

        // Check if player is going outside of the camera
        if (left > playerLeft)
        {
            // If camera is already moving
            if (m_movingCamera)
            {
                // Push the player to be in bound to the right
                playerTransform.position = new Vector3(left + playerScale.x, playerPos.y, playerPos.z);
            }
            else
            {
                // Set the goal position (where the camera is going to be end up)
                m_cameraGoalPos = new Vector3(currPos.x - cameraOrthographSize * 2.15f, currPos.y, currPos.z);
            }
        }
        else if (right < playerRight)
        {
            // If camera is already moving
            if (m_movingCamera)
            {
                // Push the player to be in bound to the left
                playerTransform.position = new Vector3(right - playerScale.x, playerPos.y, playerPos.z);
            }
            else
            {
                // Set the goal position (where the camera is going to be end up)
                m_cameraGoalPos = new Vector3(currPos.x + cameraOrthographSize * 2.15f, currPos.y, currPos.z);
            }
        }
        else if (up < playerUp)
        {
            // If camera is already moving
            if (m_movingCamera)
            {
                // Push the player to be in bound to the down
                playerTransform.position = new Vector3(playerPos.x, up - playerScale.y, playerPos.z);
            }
            else
            {
                // Set the goal position (where the camera is going to be end up)
                m_cameraGoalPos = new Vector3(currPos.x, currPos.y + cameraOrthographSize * 1.48f, currPos.z);
            }
        }
        else if (down > playerDown)
        {
            // If camera is already moving
            if (m_movingCamera)
            {
                // Push the player to be in bound to the up
                playerTransform.position = new Vector3(playerPos.x, down + playerScale.y, playerPos.z);
            }
            else
            {
                // Set the goal position (where the camera is going to be end up)
                m_cameraGoalPos = new Vector3(currPos.x, currPos.y - cameraOrthographSize * 1.48f, currPos.z);
            }
        }

        // Get the player control to make it not controlable or controlable
        PlayerController playerController = m_player.GetComponent<PlayerController>();

        // Camera finished moving
        if (m_cameraGoalPos == transform.position)
        {
            // Reset everything back to normal
            m_movingCamera = false;
            playerController.SetControllable(true);
            m_cameraGoalPos = new Vector3();
            
            // Enable back the animator
            m_player.GetComponent<Animator>().enabled = true;
        }
        else if (m_cameraGoalPos != new Vector3())
        {
            // Moving Camera
            m_movingCamera = true;
            transform.position = Vector3.MoveTowards(transform.position, m_cameraGoalPos, m_cameraSpeed * Time.deltaTime);

            // Cannot control the player when the camera is moving
            playerController.SetControllable(false);

            // Disable the animator to stop it from playing
            m_player.GetComponent<Animator>().enabled = false;
        }
    }

    public float GetLeft()
    {
        Vector3 currPos = transform.position;
        float cameraOrthographSize = GetComponent<Camera>().orthographicSize;
        float left = currPos.x - cameraOrthographSize;
        return left;
    }

    public float GetRight()
    {
        Vector3 currPos = transform.position;
        float cameraOrthographSize = GetComponent<Camera>().orthographicSize;
        float right = currPos.x + cameraOrthographSize;
        return right;
    }

    public float GetUp()
    {
        Vector3 currPos = transform.position;
        float cameraOrthographSize = GetComponent<Camera>().orthographicSize;
        float up = currPos.y + cameraOrthographSize - 2;
        return up;
    }

    public float GetDown()
    {
        Vector3 currPos = transform.position;
        float cameraOrthographSize = GetComponent<Camera>().orthographicSize;
        float down = currPos.y - cameraOrthographSize;
        return down;
    }

    public bool IsMoving()
    {
        return m_movingCamera || m_openingCurtain || m_closingCurtain || m_playerDied;
    }

    public void CheckIfOpenCurtain()
    {
        if (!m_openingCurtain)
            return;

        Vector3 leftTarget = m_leftCurtainRB.transform.position;
        leftTarget.x = GetLeft() - m_leftCurtainRB.transform.localScale.x / 2 - 1;
        leftTarget.z = -9;
        Vector3 rightTarget = m_rightCurtainRB.transform.position;
        rightTarget.x = GetRight() + m_leftCurtainRB.transform.localScale.x / 2 + 1;
        rightTarget.z = -9;

        Vector3 leftCurtainPos = m_leftCurtain.transform.position;
        Vector3 rightCurtainPos = m_rightCurtain.transform.position;

        if (Vector3.Distance(leftCurtainPos, leftTarget) > 0.1f &&
            Vector3.Distance(rightCurtainPos, rightTarget) > 0.1f)
        {
            m_leftCurtainRB.velocity = new Vector2(-m_curtainSpeed, 0);
            m_rightCurtainRB.velocity = new Vector2(m_curtainSpeed, 0);
        }
        else
        {
            m_leftCurtainRB.velocity = new Vector2(0, 0);
            m_rightCurtainRB.velocity = new Vector2(0, 0);
            m_leftCurtainRB.transform.position = leftTarget;
            m_rightCurtainRB.transform.position = rightTarget;
            m_openingCurtain = false;
        }
    }

    public void CheckIfCloseCurtain()
    {
        if (!m_closingCurtain)
            return;

        Vector3 leftTarget = transform.position;
        leftTarget.x -= m_leftCurtainRB.transform.localScale.x / 2;
        leftTarget.z = -9;
        Vector3 rightTarget = transform.position;
        rightTarget.x += m_rightCurtainRB.transform.localScale.x / 2;
        rightTarget.z = -9;

        Vector3 leftCurtainPos = m_leftCurtainRB.transform.position;
        Vector3 rightCurtainPos = m_rightCurtainRB.transform.position;

        if (Vector3.Distance(leftCurtainPos, leftTarget) > 0.1f &&
            Vector3.Distance(rightCurtainPos, rightTarget) > 0.1f)
        {
            m_leftCurtainRB.velocity = new Vector2(m_curtainSpeed, 0);
            m_rightCurtainRB.velocity = new Vector2(-m_curtainSpeed, 0);
        }
        else
        {
            m_leftCurtainRB.velocity = new Vector2(0, 0);
            m_rightCurtainRB.velocity = new Vector2(0, 0);
            m_leftCurtainRB.transform.position = leftTarget;
            m_rightCurtainRB.transform.position = rightTarget;
            m_closingCurtain = false;
        }
    }

    public void OpenCurtain()
    {
        if (IsMoving())
            return;

        m_openingCurtain = true;

        // Left Curtain, starts at left middle of camera
        Vector3 leftCurtainPos = transform.position;
        leftCurtainPos.z = -9;
        leftCurtainPos.x -= m_leftCurtain.transform.localScale.x / 2;
        m_leftCurtain.transform.position = leftCurtainPos;

        // Right Curtain, starts at right middle of camera
        Vector3 rightCurtainPos = transform.position;
        rightCurtainPos.z = -9;
        rightCurtainPos.x += m_rightCurtain.transform.localScale.x / 2;
        m_rightCurtain.transform.position = rightCurtainPos;
    }

    public void CloseCurtain()
    {
        if (IsMoving())
            return;

        m_closingCurtain = true;

        // Left Curtain, starts at outside left middle of camera
        Vector3 leftCurtainPos = transform.position;
        leftCurtainPos.x = GetLeft() - m_leftCurtain.transform.localScale.x / 2 - 1;
        leftCurtainPos.z = m_leftCurtain.transform.position.z;
        m_leftCurtain.transform.position = leftCurtainPos;

        // Right Curtain, starts at outside right middle of camera
        Vector3 rightCurtainPos = transform.position;
        rightCurtainPos.x = GetRight() + m_rightCurtain.transform.localScale.x / 2 + 1;
        rightCurtainPos.z = m_rightCurtain.transform.position.z;
        m_rightCurtain.transform.position = rightCurtainPos;
    }
    public void SetFrozen(bool set)
    {
        m_frozen = set;
    }
}