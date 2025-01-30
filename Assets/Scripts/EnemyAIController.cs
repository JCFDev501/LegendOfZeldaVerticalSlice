using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * TODO:
 *  - Chance to drop hearts, rupees, and bombs, and small chance 
 *    of a clock
 *  - Death animation
*/

public class EnemyAIController : MonoBehaviour
{
    [Header("Room")]
    [SerializeField] private int m_roomIndex = -1;

    [Header("Regular Drop")]
    [SerializeField] private GameObject m_heart;
    [SerializeField] private GameObject m_bomb;
    [SerializeField] private GameObject m_rupee;
    [SerializeField] private GameObject m_clock;
    [SerializeField] private int m_heartDropChance = 15;
    [SerializeField] private int m_bombDropChance = 15;
    [SerializeField] private int m_rupeeDropChance = 15;
    [SerializeField] private int m_clockDropChance = 5;

    [Header("Movement")]
    [SerializeField] private float m_velocity = 1f;
    [SerializeField] private float m_timeBetweenRandomDirection = 2f;
    [SerializeField] private GameObject m_camera;
    [SerializeField] private GameObject m_spawnInEffect;
    [SerializeField] private GameObject m_deathEffect;

    [Header("Combat")]
    [SerializeField] private int m_startingHealth = 1;
    [SerializeField] private float m_stunTimer = 3f;
    private KnockBack m_knockBackComponent;

    [Header("Range Attack")]
    [SerializeField] private bool m_haveRangeAttack = false;
    [SerializeField] private float m_attackDuration = 1f;
    [SerializeField] private float m_timeBetweenAttack = 2f;
    [SerializeField] private int m_chanceToAttack = 20;
    [SerializeField] protected GameObject m_projectile;

    [Header("Flash")]
    [SerializeField] protected MultiColorFlashEffect m_flashEffect;


    public enum Direction
    {
        Left,
        Right,
        Up,
        Down,
        None,
    }

    // Combat
    private int m_health;
    private float m_timeFromLastAttack = 0f;
    private bool m_inAttack = false;
    private bool m_isStunned = false;

    // Movement
    private Rigidbody2D m_rigidbody;
    protected Vector2 m_targetPos;
    private float m_movementTimer;
    private Vector2 m_currVel = new Vector2();
    public Direction m_direction = Direction.None;
    private Vector2 m_startPos;

    private bool m_isPaused = false;
    private bool m_isActive = false;
    private bool m_isInAnimation = false;
    private float m_spawnAnimTime = 1f;

    public void SetIsPaused(bool isPaused) { m_isPaused = isPaused; }
    public bool GetIsAlive() { return m_health > 0; }

    private Animator animator;

    protected bool m_inCamera = false;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_targetPos = m_rigidbody.position;
        m_startPos = m_rigidbody.position;
        m_movementTimer = m_timeBetweenRandomDirection;
        m_health = m_startingHealth;
        animator = GetComponent<Animator>();
        m_knockBackComponent = GetComponent<KnockBack>();

        // Get Enemy Manager and add this to the room index
        EnemyManager enemys = GameObject.Find("Enemy Manager").GetComponent<EnemyManager>();
        enemys.AddEnemy(m_roomIndex, this);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (m_knockBackComponent.isKnockbackActive)
            return;

        if (m_health <= 0) return;

        if (m_isPaused)
            animator.enabled = false;
        else
            animator.enabled = true;

        // Color for the game object
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Color newColor = spriteRenderer.color;

        // Update only inside the camera
        CameraController cameraController = m_camera.GetComponent<CameraController>();
        if(cameraController != null)
        {
            // Get camera 
            float cameraLeft = cameraController.GetLeft();
            float cameraRight = cameraController.GetRight();
            float cameraUp = cameraController.GetUp();
            float cameraDown = cameraController.GetDown();

            Vector2 currPos = m_rigidbody.position;
            if(cameraController.IsMoving() || currPos.x < cameraLeft  || currPos.x > cameraRight || currPos.y > cameraUp || currPos.y < cameraDown)
            {
                m_inCamera = false;

                // If we are outside the camera don't do anything or 
                // when the camera is moving
                StandStill();

                m_isActive = false;
                m_isInAnimation = false;

                // Change the color so the player can't see
                newColor.a = 0;
                spriteRenderer.color = newColor;

                return;
            }
        }

        // See if it is already active, if not, play spawn in animation
        if (m_isActive == false && m_spawnInEffect)
        {
            if(m_isInAnimation == false)
            {
                Instantiate(m_spawnInEffect, transform.position, Quaternion.identity);
                m_isInAnimation = true;
            }

            m_spawnAnimTime -= Time.deltaTime;
            if (m_spawnAnimTime < 0f)
            {
                m_isInAnimation = false;
                m_isActive = true;
                m_spawnAnimTime = 1f;
                TurnOnAllCollision();
            }
            else
            {
                TurnOffAllCollision();
                return;
            }
        }

        // Change the color so the player can see
        m_inCamera = true;
        newColor.a = 1;
        spriteRenderer.color = newColor;

        if (m_isStunned)
        {
            m_isPaused = true;

            m_stunTimer -= Time.deltaTime;
            if (m_stunTimer < 0)
            {
                m_isStunned = false;
                m_isPaused = false;
                m_stunTimer = 3f;
            }
            else
            {
                return;
            }
        }

        // Don't do anything if the game object is paused
        if (m_isPaused)
        {
            StandStill();
            return;
        }


        // Stop when attacking 
        if (m_inAttack)
        {
            m_timeFromLastAttack += Time.deltaTime;
            if (m_timeFromLastAttack > m_attackDuration)
            {
                m_inAttack = false;
                m_timeFromLastAttack -= m_attackDuration;
            }
            else
            {
                StandStill();
                return;
            }
        }


        // Check if it is time to make a decision
        m_movementTimer += Time.deltaTime;
        if (m_movementTimer > m_timeBetweenRandomDirection)
        {
            ChooseRandomDirection();

            // Reset timer
            m_movementTimer -= m_timeBetweenRandomDirection;
        }

        if (IsAtTargetPosition() || IsWalkingOffCamera())
        {   
            StandStill();
        }
        else
        {
            MoveTowardDirection();
        }

        // Long Range Attack
        if (!m_haveRangeAttack)
            return;

        m_timeFromLastAttack += Time.deltaTime;
        if(m_timeFromLastAttack > m_timeBetweenAttack)
        {
            m_inAttack = ChanceToShootProjectile();
            m_timeFromLastAttack += -m_timeBetweenAttack;
        }
    }

    protected virtual void ChooseRandomDirection()
    {
        // Get Current Position
        Vector3 currPos = transform.position;

        // Get Random Direction
        int direction = Random.Range(0, 4);
        switch (direction)
        {
            case 0:
                m_direction = Direction.Up;
                m_targetPos = new Vector3(currPos.x, currPos.y + 1f);
                break;

            case 1:     
                m_direction = Direction.Down;
                m_targetPos = new Vector3(currPos.x, currPos.y - 1f);
                break;

            case 2:  
                m_direction = Direction.Left;
                m_targetPos = new Vector3(currPos.x - 1f, currPos.y);
                break;

            case 3:    
                m_direction = Direction.Right;
                m_targetPos = new Vector3(currPos.x + 1f, currPos.y);
                break;

            default:
                m_direction = Direction.None;
                break;
        }
    }
    bool IsAtTargetPosition()
    {
        // Get Current Position
        Vector2 currPos = m_rigidbody.position;

        // Check if at target position
        return currPos == m_targetPos;
    }

    void MoveTowardDirection()
    {
        // Reset velocity
        m_currVel = new Vector2();

        switch (m_direction)
        {
            case Direction.Up:     
                m_currVel.y = m_velocity;
                break;

            case Direction.Down:    
                m_currVel.y = -m_velocity;
                break;

            case Direction.Left:     
                m_currVel.x = -m_velocity;
                break;

            case Direction.Right:   
                m_currVel.x = m_velocity;
                break;

            default:
                break;
        }

        m_rigidbody.velocity = m_currVel;
    }

    void StandStill()
    {
        m_rigidbody.velocity = Vector2.zero;
    }

    //-------------------------------------------------------
    // Return whether or not it shoot projectile
    //-------------------------------------------------------
    bool ChanceToShootProjectile()
    {
        // Random number from 1 to 100
        int rand = Random.Range(1, 101);

        if(rand < m_chanceToAttack)
        {
            ShootProjectile();
            return true;
        }

        return false;
    }

    //-------------------------------------------------------
    // Shoot Projectile base on our direction
    //-------------------------------------------------------
    protected virtual void ShootProjectile()
    {
        Vector3 spawnPos = new Vector3();
        Quaternion spawnRos = new Quaternion();
        switch (m_direction)
        {
            case Direction.Up:
                spawnPos = transform.position + new Vector3(0, .335f, 0);
                spawnRos = transform.rotation * Quaternion.Euler(0, 0, 0);
                break;

            case Direction.Down:
                spawnPos = transform.position + new Vector3(0, -.67f, 0);
                spawnRos = transform.rotation * Quaternion.Euler(0, 0, 180);
                break;

            case Direction.Left:
                spawnPos = transform.position + new Vector3(-.67f, -.335f, 0);
                spawnRos = transform.rotation * Quaternion.Euler(0, 0, 90);
                break;

            case Direction.Right:
                spawnPos = transform.position + new Vector3(.67f, -.335f, 0);
                spawnRos = transform.rotation * Quaternion.Euler(0, 0, 270);
                break;

            default:
                return;
        }

        Instantiate(m_projectile, spawnPos, spawnRos);
    }

    protected virtual void OnDeath() 
    {
        // Play On Death Animation
        if(m_deathEffect)
            Instantiate(m_deathEffect, transform.position, Quaternion.identity);

        // Random percent from 0 to 100
        int rand = Random.Range(0, 101);

        // Heart
        int currPercent = m_heartDropChance;
        if (currPercent > rand)
        {
            Instantiate(m_heart, transform.position, new Quaternion());
            return;
        }

        // Rupee
        currPercent += m_rupeeDropChance;
        if (currPercent > rand)
        {
            Instantiate(m_rupee, transform.position, new Quaternion());
            return;
        }

        // Bomb
        currPercent += m_bombDropChance;
        if(currPercent > rand)
        {
            Instantiate(m_bomb, transform.position, new Quaternion());
            return;
        }

        // Clock
        currPercent += m_clockDropChance;
        if (currPercent > rand)
        {
            Instantiate(m_clock, transform.position, new Quaternion());
            return;
        }
    }

    public virtual void ResetToDefault()
    {
        m_health = m_startingHealth;
        m_rigidbody.position = m_startPos;
        m_targetPos = m_startPos;
        m_timeFromLastAttack = 0f;
        m_inAttack = false;
        m_currVel = new Vector2();
        m_direction = Direction.None;
}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Should not do anything collision check anymore 
        // since it is already dead
        if (m_health <= 0)
            return;

        if (collision != null)
        {
            if (collision.tag != "LinkAttack" && collision.tag != "Boomarang")
                return;

            if (collision.tag == "Boomarang")
            {
                m_isStunned = true;
                m_flashEffect.FlashTheColors(m_stunTimer);
                return;
            }
            Vector2 knockbackDirection = ((Vector2)transform.position - (Vector2)collision.transform.position).normalized;

            m_knockBackComponent.ApplyKnockback(knockbackDirection);

            m_health -= 1;
            m_flashEffect.FlashTheColors(0.5f);
            

            if(m_health <= 0)
            {
                OnDeath();
                m_rigidbody.position = new Vector2(-1000f, -1000f);
            }
        }
    }

    public void UpdateHealth(int change)
    {
        m_health = change;

        if (m_health <= 0)
        {
            OnDeath();
            m_rigidbody.position = new Vector2(-1000f, -1000f);
        }
    }

    bool IsWalkingOffCamera()
    {
        CameraController cameraController = m_camera.GetComponent<CameraController>();
        if (cameraController == null)
            return false;

        // Get camera positions
        float cameraLeft = cameraController.GetLeft();
        float cameraRight = cameraController.GetRight();
        float cameraUp = cameraController.GetUp();
        float cameraDown = cameraController.GetDown();

        // Get current position
        float currentPosX = transform.position.x;
        float currentPosY = transform.position.y;

        // Left
        if (cameraLeft > currentPosX - .5f && m_direction == Direction.Left)
            return true;

        // Right
        if (cameraRight < currentPosX + .5f && m_direction == Direction.Right)
            return true;

        // Up
        if (cameraUp < currentPosY + 1f && m_direction == Direction.Up)
            return true;

        // Down
        if (cameraDown > currentPosY - 1f && m_direction == Direction.Down)
            return true;

        return false;
    }

    void TurnOnAllCollision()
    {
        foreach(Collider2D collider in GetComponents<Collider2D>())
        {
            collider.enabled = true;
        }
    }

    void TurnOffAllCollision()
    {
        foreach (Collider2D collider in GetComponents<Collider2D>())
        {
            collider.enabled = false;
        }
    }
}