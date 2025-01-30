using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateManager : MonoBehaviour
{
    // Player controller
    public PlayerController m_playerController;

    // AI Controller
    public EnemyAIController m_aIController;

    // Current State
    protected BaseState m_currentState; 

    // Reference to the Rigidbody2D
    protected Rigidbody2D m_rigidbody;

    // Direction of the player
    public Vector2 m_direction;

    // Reference to the Animator
    public Animator anim;

    // ------------------------------------------------ Link States ------------------------------------------------
    public IdleState Idle = new IdleState();
    public WalkRightState WalkRight = new WalkRightState();
    public WalkLeftState WalkLeft = new WalkLeftState();
    public WalkUpState WalkUp = new WalkUpState();
    public WalkDownState WalkDown = new WalkDownState();
    public AttackDownState AttackDown = new AttackDownState();
    public AttackUpState AttackUp = new AttackUpState();
    public AttackLeftState AttackLeft = new AttackLeftState();
    public AttackRightState AttackRight = new AttackRightState();

    // ---------------------------------------------- Octarock States -----------------------------------------------
    public OctarockMoveDownState OctarockMoveDown = new OctarockMoveDownState();
    public OctarockMoveUpState OctarockMoveUp = new OctarockMoveUpState();
    public OctarockMoveRightState OctarockMoveRight = new OctarockMoveRightState();
    public OctarockMoveLeftState OctarockMoveLeft = new OctarockMoveLeftState();

    // --------------------------------------------- Moblin States --------------------------------------------------

    public MoblinDownState MoblinMoveDown = new MoblinDownState();
    public MoblinUpState MoblinMoveUp = new MoblinUpState();
    public MoblinRightState MoblinMoveRight = new MoblinRightState();
    public MoblinLeftState MoblinMoveLeft = new MoblinLeftState();

    // --------------------------------------------- Goryia States --------------------------------------------------

    public GoryiaDownState GoryiaMoveDown = new GoryiaDownState();
    public GoryiaUpState GoryiaMoveUp = new GoryiaUpState();
    public GoryiaRightState GoryiaMoveRight = new GoryiaRightState();
    public GoryiaLeftState GoryiaMoveLeft = new GoryiaLeftState();



    // Start is called before the first frame update
    void Start()
    {
        // Get the Rigidbody2D
        m_rigidbody = GetComponent<Rigidbody2D>();

        // Get the Animator
        anim = GetComponentInChildren<Animator>(); 

        m_currentState = WalkDown;
    }

    // Update is called once per frame
    void Update()
    {
        // Get the velocity of the player...
        Vector2 velocity = m_rigidbody.velocity;

        // Divide the velocity by the speed to get the direction...
        velocity = velocity / 5; // ---> m_moveSpeed;

        // Set the direction...
        m_direction = velocity;

        // Update the state...
        if (m_currentState != null)
            m_currentState.UpdateState(this);
        
    }

    // Change the state...
    public void TransitionToState(BaseState newState)
    {
        // Exit the current state...
        if (m_currentState != null)
            m_currentState.ExitState(this);

        // Change the state...
        m_currentState = newState;

        // Enter the new state...
        m_currentState.EnterState(this);
    }
}
