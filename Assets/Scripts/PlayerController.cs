using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum PlayerStates
{
    idel, movingNorth, movingSouth, movingEast, movingWest, attackNorth, attackSouth, attackEast, attackWest, pickingUpItem, idleNorth, idleSouth, idleEast, idleWest
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Health m_healthComp;
    [SerializeField] private float m_moveSpeed = 5f;
    [SerializeField] private GameObject m_UI;
    [SerializeField] private GameObject m_death;
    [SerializeField] private GameObject m_hurtBox;
    [SerializeField] private GameObject m_beam;
    [SerializeField] public GameObject m_itemOverHeadRef;
    [SerializeField] private GameObject m_arrowPrefab;
    [SerializeField] private GameObject m_boomerangPrefab;
    [SerializeField] private GameObject m_bombPrefab;

    private bool isMoving = false;
    private Vector3 targetPosition;

    // Temporary
    public MultiColorFlashEffect m_flashEffect;

    private bool m_activeBoomerang;

    public PlayerStates m_state;

    [SerializeField] public Inventory m_inventory;

    private GameObject m_attack;
    private Rigidbody2D m_rigidbody;
    private Animator anim;
    private KnockBack m_knockBackComponent;
    public int m_lastDir = 1; //0: None, 1: North, 2: South, 3: East, 4: West

    private bool m_canMoveNorth = true;
    private bool m_canMoveSouth = true;
    private bool m_canMoveEast = true;
    private bool m_canMoveWest = true;

    // If it is controlable
    public bool m_controlable = true;
    public bool m_gliding = false;

    private void Start()
    {
        m_attack = new GameObject();
        m_rigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        m_knockBackComponent = GetComponent<KnockBack>();
    }

    void Update()
    {
        if (m_healthComp.GetCurrentHealth() <= 0)
        {

            Die();
        }

        if (m_gliding)
        {
            return;
        }

        if (!m_controlable)
        {
            m_rigidbody.velocity = Vector2.zero;
            return;
        }

        Move();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (m_UI.GetComponent<Inventory>().ContainsItem("Sword"))
                Attack();
        }
        else if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            UseItemInBSlot(m_inventory.UseBSlot());
        }

    }

    private void Die()
    {
        m_controlable = false;
        anim.Play("Death");
    }

    private void Move()
    {
        if (m_knockBackComponent.isKnockbackActive)
            return; // Skip movement input if currently experiencing knockback

        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = 0;

        if (m_canMoveNorth == false && vertical > 0)
            vertical = 0;
        if (m_canMoveSouth == false && vertical < 0)
            vertical = 0;

        if (vertical == 0)
            horizontal = Input.GetAxisRaw("Horizontal");

        Vector3 m_input = new Vector3(horizontal, vertical, 0);

        m_rigidbody.velocity = m_input * m_moveSpeed;


        if (vertical > 0)
        {
            m_lastDir = 1;
            m_state = PlayerStates.movingNorth;
        }
    
        else if (vertical < 0)
        {
            m_lastDir = 2;
            m_state = PlayerStates.movingSouth;
        }
        else if (horizontal > 0)
        {
            m_lastDir = 3;
            m_state = PlayerStates.movingEast;
        }
        else if (horizontal < 0)
        {
            m_lastDir = 4;
            m_state = PlayerStates.movingWest;

        }
        else
        {
            m_state = PlayerStates.idel;
        }



    }

    private void Attack()
    {
        m_rigidbody.velocity = Vector2.zero;
        float attackTime;

        if (m_healthComp.GetCurrentHealth() == m_healthComp.GetMaxHealth())
        {
            if (m_lastDir == 1)
            {
                m_state = PlayerStates.attackNorth;
            }
            if (m_lastDir == 2)
            {
                m_state = PlayerStates.attackSouth;
            }
            if (m_lastDir == 3)
            {
                m_state = PlayerStates.attackEast;
            }
            if (m_lastDir == 4)
            {
                m_state = PlayerStates.attackWest;
            }
            attackTime = m_beam.GetComponent<Projectile>().GetAttackTime();        
        }
        else
        {
            if (m_lastDir == 1)
            {
                m_state = PlayerStates.attackNorth;
            }
            if (m_lastDir == 2)
            {
                m_state = PlayerStates.attackSouth;
            }
            if (m_lastDir == 3)
            {
                m_state = PlayerStates.attackEast;
            }
            if (m_lastDir == 4)
            {
                m_state = PlayerStates.attackWest;
            }
            attackTime = m_hurtBox.GetComponent<HurtBox>().GetAttackTime();
        }

        m_controlable = false;
        StartCoroutine(AttackTime(attackTime));
    }

    public void AttackNorth()
    {
        float attackTime = 0;

        if (m_healthComp.GetCurrentHealth() == m_healthComp.GetMaxHealth())
        {
            
            Vector3 spawnPos = transform.position + new Vector3(0.1f, 1f, 0);
            Quaternion spawnRos = transform.rotation * Quaternion.Euler(0, 0, 0);
            m_attack = Instantiate(m_beam, spawnPos, spawnRos);
            
            attackTime = m_attack.GetComponent<Projectile>().GetAttackTime();
        }
        else
        {
           
            Vector3 spawnPos = transform.position + new Vector3(0.1f, 1f, 0);
            Quaternion spawnRos = transform.rotation;
            m_attack = Instantiate(m_hurtBox, spawnPos, spawnRos);
           
            attackTime = m_attack.GetComponent<HurtBox>().GetAttackTime();
        }

        m_controlable = false;
        StartCoroutine(AttackTime(attackTime));
    }

    public void AttackSouth()
    {
        float attackTime = 0;

        if (m_healthComp.GetCurrentHealth() == m_healthComp.GetMaxHealth())
        {
            Vector3 spawnPos = transform.position + new Vector3(-.1f, -1f, 0);
            Quaternion spawnRos = transform.rotation * Quaternion.Euler(0, 0, 180);
            m_attack = Instantiate(m_beam, spawnPos, spawnRos);

            attackTime = m_attack.GetComponent<Projectile>().GetAttackTime();
        }
        else
        {
            Vector3 spawnPos = transform.position + new Vector3(-.1f, -1f, 0);
            Quaternion spawnRos = transform.rotation;
            m_attack = Instantiate(m_hurtBox, spawnPos, spawnRos);

            attackTime = m_attack.GetComponent<HurtBox>().GetAttackTime();
        }

        m_controlable = false;
        StartCoroutine(AttackTime(attackTime));
    }

    public void AttackEast()
    {
        float attackTime = 0;

        if (m_healthComp.GetCurrentHealth() == m_healthComp.GetMaxHealth())
        {
            Vector3 spawnPos = transform.position + new Vector3(1f, -.2f, 0);
            Quaternion spawnRos = transform.rotation * Quaternion.Euler(0, 0, 270);
            m_attack = Instantiate(m_beam, spawnPos, spawnRos);

            attackTime = m_attack.GetComponent<Projectile>().GetAttackTime();
        }
        else
        {
            Vector3 spawnPos = transform.position + new Vector3(1f, -.2f, 0);
            Quaternion spawnRos = transform.rotation;
            m_attack = Instantiate(m_hurtBox, spawnPos, spawnRos);

            attackTime = m_attack.GetComponent<HurtBox>().GetAttackTime();
        }

        m_controlable = false;
        StartCoroutine(AttackTime(attackTime));
    }

    public void AttackWest()
    {
        float attackTime = 0;

        if (m_healthComp.GetCurrentHealth() == m_healthComp.GetMaxHealth())
        {
            Vector3 spawnPos = transform.position + new Vector3(-1f, .1f, 0);
            Quaternion spawnRos = transform.rotation * Quaternion.Euler(0, 0, 90);
            m_attack = Instantiate(m_beam, spawnPos, spawnRos);

            attackTime = m_attack.GetComponent<Projectile>().GetAttackTime();
        }
        else
        {
            Vector3 spawnPos = transform.position + new Vector3(-1f, .1f, 0);
            Quaternion spawnRos = transform.rotation;
            m_attack = Instantiate(m_hurtBox, spawnPos, spawnRos);

            attackTime = m_attack.GetComponent<HurtBox>().GetAttackTime();
        }

        m_controlable = false;
        StartCoroutine(AttackTime(attackTime));
    }
    private IEnumerator AttackTime(float attackTime)
    {
        yield return new WaitForSeconds(attackTime);
        m_controlable = true;
    }
    public void SetControllable(bool controllable)
    {
        m_controlable = controllable;
    }

    public void SetGliding(bool set)
    {
        m_gliding = set;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.transform.position.y - collision.transform.localScale.y / 2 > transform.position.y + transform.localScale.y / 2)
                m_canMoveNorth = false;
            else if (collision.transform.position.y + collision.transform.localScale.y / 2 < transform.position.y - transform.localScale.y / 2)
                m_canMoveSouth = false;
            else if (collision.transform.position.x - collision.transform.localScale.x / 2 > transform.position.x + transform.localScale.x / 2)
                m_canMoveEast = false;
            else if (collision.transform.position.x + collision.transform.localScale.x / 2 < transform.position.x - transform.localScale.x / 2)
                m_canMoveWest = false;

            if (collision.gameObject.tag == "Collectable")
            {
                collision.gameObject.GetComponent<Collectable>().UseItem(m_UI);
            }

            if (collision.gameObject.tag == "Enemy")
            {

                // Calculate the direction from the enemy to the player
                Vector2 knockbackDirection = ((Vector2)transform.position - (Vector2)collision.transform.position).normalized;

                m_flashEffect.FlashTheColors(0.5f);
                m_knockBackComponent.ApplyKnockback(knockbackDirection);

                m_healthComp.UpdateHealth(-1); // TODO: Temporary
            }

            if (collision.gameObject.tag == "Item")
            {
                PickUpItem();

                collision.gameObject.transform.position = m_itemOverHeadRef.transform.position;

                collision.gameObject.GetComponent<Item>().GetItem(m_UI, 1f);
            }

            if (collision.gameObject.tag == "Projectile" || collision.gameObject.tag == "NonReflectiveProj")
            {
                if(collision.gameObject.tag == "Projectile")
                {
                    if (m_lastDir == 1 && m_state == PlayerStates.idel)
                    {
                        m_state = PlayerStates.idleNorth;
                    }
                    if (m_lastDir == 2 && m_state == PlayerStates.idel)
                    {
                        m_state = PlayerStates.idleSouth;
                    }
                    if (m_lastDir == 3 && m_state == PlayerStates.idel)
                    {
                        m_state = PlayerStates.idleEast;
                    }
                    if (m_lastDir == 4 && m_state == PlayerStates.idel)
                    {
                        m_state = PlayerStates.idleWest;
                    }


                    // north
                    if (m_state == PlayerStates.movingNorth || m_state == PlayerStates.idleNorth)
                    {
                        if (collision.transform.position.y > transform.position.y && collision.gameObject.GetComponent<Rigidbody2D>().velocity.y != 0)
                        {
                            collision.GetComponent<Projectile>().SetVelocity(new Vector2(collision.GetComponent<Rigidbody2D>().velocity.y, -collision.GetComponent<Rigidbody2D>().velocity.y));
                            StartCoroutine(ProjectileDisapear(collision.gameObject));
                            return;
                        }
                    }

                    // south
                    if (m_state == PlayerStates.movingSouth || m_state == PlayerStates.idleSouth)
                    {
                        if (collision.transform.position.y < transform.position.y && collision.gameObject.GetComponent<Rigidbody2D>().velocity.y != 0)
                        {
                            collision.GetComponent<Projectile>().SetVelocity(new Vector2(collision.GetComponent<Rigidbody2D>().velocity.y, -collision.GetComponent<Rigidbody2D>().velocity.y));
                            StartCoroutine(ProjectileDisapear(collision.gameObject));
                            return;
                        }
                    }

                    // east
                    if (m_state == PlayerStates.movingEast || m_state == PlayerStates.idleEast)
                    {
                        if (collision.transform.position.x > transform.position.x && collision.gameObject.GetComponent<Rigidbody2D>().velocity.x != 0)
                        {
                            collision.GetComponent<Projectile>().SetVelocity(new Vector2(-collision.GetComponent<Rigidbody2D>().velocity.x, collision.GetComponent<Rigidbody2D>().velocity.x));
                            StartCoroutine(ProjectileDisapear(collision.gameObject));
                            return;
                        }
                    }

                    // west
                    if (m_state == PlayerStates.movingWest || m_state == PlayerStates.idleWest)
                    {
                        if (collision.transform.position.x < transform.position.x && collision.gameObject.GetComponent<Rigidbody2D>().velocity.x != 0)
                        {
                            collision.GetComponent<Projectile>().SetVelocity(new Vector2(-collision.GetComponent<Rigidbody2D>().velocity.x, collision.GetComponent<Rigidbody2D>().velocity.x));
                            StartCoroutine(ProjectileDisapear(collision.gameObject));
                            return;
                        }
                    }
                }

                Vector2 knockbackDirection = ((Vector2)transform.position - (Vector2)collision.transform.position).normalized;

                m_flashEffect.FlashTheColors(0.5f);
                m_knockBackComponent.ApplyKnockback(knockbackDirection);

                m_healthComp.UpdateHealth(-1); // TODO: Temporary
                Destroy(collision.gameObject);
            }
        }
    }

    private IEnumerator ProjectileDisapear(GameObject projectile)
    {
        projectile.GetComponent<SpriteRenderer>().color = new Color (1, 1, 1, projectile.GetComponent<SpriteRenderer>().color.a - 0.2f);
        yield return new WaitForSeconds(0.1f);
        projectile.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, projectile.GetComponent<SpriteRenderer>().color.a - 0.2f);
        yield return new WaitForSeconds(0.1f);
        projectile.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, projectile.GetComponent<SpriteRenderer>().color.a - 0.2f);
        yield return new WaitForSeconds(0.1f);
        projectile.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, projectile.GetComponent<SpriteRenderer>().color.a - 0.2f);
        yield return new WaitForSeconds(0.1f);
        projectile.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, projectile.GetComponent<SpriteRenderer>().color.a - 0.2f);
        yield return new WaitForSeconds(0.1f);
        Destroy(projectile);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null)
        {
            m_canMoveNorth = true;
            m_canMoveSouth = true;
            m_canMoveEast = true;
            m_canMoveWest = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Trap")
        {

            Vector2 knockbackDirection = ((Vector2)transform.position - (Vector2)collision.transform.position).normalized;

            m_flashEffect.FlashTheColors(0.5f);
            m_knockBackComponent.ApplyKnockback(knockbackDirection);

            m_healthComp.UpdateHealth(-1); // TODO: Temporary
        }
    }

    public void PickUpItem()
    {
        m_controlable = false;
        anim.Play("PickUp");
    }

    public void SetControlableToTrue()
    {
        m_controlable = true;
    }

    private void UseItemInBSlot(BSlot Item)
    {
        if (Item == BSlot.Bomb)
            UseBomb();
        else if (Item == BSlot.Boomarang)
            ThrowBoomerang();
        else if (Item == BSlot.Bow)
            ShootBow();
        else
            return;
    }

    void ShootBow()
    {
        // Check for the arrow prefab
        if (m_arrowPrefab == null)
        {
            Debug.LogError("Arrow prefab is not assigned!");
            return;
        }

        // Determine the spawn position and direction
        Vector3 spawnPos = transform.position;
        Quaternion spawnRos = transform.rotation * Quaternion.Euler(0, 0, 0);
        float attackTime = 0;

        switch (m_lastDir)
        {
            case 1: // North
                spawnPos = transform.position + new Vector3(0, .67f, 0);
                spawnRos = transform.rotation * Quaternion.Euler(0, 0, 0);
                break;

            case 2: // South
                spawnPos = transform.position + new Vector3(0, -.67f, 0);
                spawnRos = transform.rotation * Quaternion.Euler(0, 0, 180);
                break;
            case 3: // East:
                spawnPos = transform.position + new Vector3(.67f, -.335f, 0);
                spawnRos = transform.rotation * Quaternion.Euler(0, 0, 270);
                break;
            case 4: // West
                spawnPos = transform.position + new Vector3(-.67f, -.335f, 0);
                spawnRos = transform.rotation * Quaternion.Euler(0, 0, 90);
                break;
            default:
                Debug.LogWarning("Invalid direction for shooting an arrow!");
                return;
        }

        // Instantiate the arrow
        GameObject arrow = Instantiate(m_arrowPrefab, spawnPos, spawnRos);

        attackTime = arrow.GetComponent<Projectile>().GetAttackTime();

        m_controlable = false;
        StartCoroutine(AttackTime(attackTime));
    }

    void ThrowBoomerang()
    {
        if (m_activeBoomerang)
            return;

        // Check for the boomerang prefab
        if (m_boomerangPrefab == null)
        {
            Debug.LogError("Boomerang prefab is not assigned!");
            return;
        }

        // Determine the spawn position and direction
        Vector3 spawnPos = transform.position;
        Quaternion spawnRot = transform.rotation * Quaternion.Euler(0, 0, 0);

        switch (m_lastDir)
        {
            case 1: // North
                spawnPos = transform.position + new Vector3(0, .67f, 0);
                spawnRot = transform.rotation * Quaternion.Euler(0, 0, 0);
                break;

            case 2: // South
                spawnPos = transform.position + new Vector3(0, -.67f, 0);
                spawnRot = transform.rotation * Quaternion.Euler(0, 0, 180);
                break;
            case 3: // East:
                spawnPos = transform.position + new Vector3(.67f, -.335f, 0);
                spawnRot = transform.rotation * Quaternion.Euler(0, 0, 270);
                break;
            case 4: // West
                spawnPos = transform.position + new Vector3(-.67f, -.335f, 0);
                spawnRot = transform.rotation * Quaternion.Euler(0, 0, 90);
                break;
            default:
                Debug.LogWarning("Invalid direction for throwing a boomerang!");
                return;
        }

        // Instantiate the boomerang
        GameObject boomerang = Instantiate(m_boomerangPrefab, spawnPos, spawnRot);
        Boomerang boomerangScript = boomerang.GetComponent<Boomerang>();
       

        if (boomerangScript != null)
        {
            boomerangScript.Initialize(m_lastDir, transform);
            m_activeBoomerang = true;
        }
        else
        {
            Debug.LogError("Boomerang prefab does not have a Boomerang script attached!");
        }
    }

    public void OnBoomerangDestroyed()
    {
        m_activeBoomerang = false; // Reset the flag when the boomerang is destroyed or returns
    }

    void UseBomb()
    {
        // Check if bomb prefab is assigned
        if (m_bombPrefab == null)
        {
            Debug.LogError("Bomb prefab is not assigned!");
            return;
        }

        // Determine the spawn position and direction
        Vector3 spawnPos = transform.position;
        Quaternion spawnRot = transform.rotation * Quaternion.Euler(0, 0, 0);

        switch (m_lastDir)
        {
            case 1: // North
                spawnPos = transform.position + new Vector3(0, .67f, 0);
               // spawnRot = transform.rotation * Quaternion.Euler(0, 0, 0);
                break;
            case 2: // South
                spawnPos = transform.position + new Vector3(0, -.67f, 0);
                //spawnRot = transform.rotation * Quaternion.Euler(0, 0, 180);
                break;
            case 3: // East
                spawnPos = transform.position + new Vector3(.67f, -.335f, 0);
                //spawnRot = transform.rotation * Quaternion.Euler(0, 0, 270);
                break;
            case 4: // West
                spawnPos = transform.position + new Vector3(-.67f, -.335f, 0);
                //spawnRot = transform.rotation * Quaternion.Euler(0, 0, 90);
                break;
            default:
                Debug.LogWarning("Invalid direction for using Bomb!");
                return;
        }

        // Instantiate the bomb
        GameObject bomb = Instantiate(m_bombPrefab, spawnPos, spawnRot);

        BombExplosion bombExplosionScript = bomb.GetComponent<BombExplosion>();
    }

    public void WalkDownStiars()
    {
        m_gliding = true;
        m_rigidbody.velocity = Vector3.down / 2;
        anim.Play("WalkUp");
    }

    public void SetIsDead(bool isDead)
    {
        m_death.SetActive(isDead);
    }

    public float GetMovmentSpeed()
    {
        return m_moveSpeed;
    }

}
