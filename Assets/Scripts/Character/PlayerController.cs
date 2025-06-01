using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using Utils;

[RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Collider _collider;
    private SpriteRenderer _visual;
    [SerializeField] private SpriteRenderer _hat;

    [SerializeField] private InventorySystem _inventorySystem;

    [Header("Animation")]
    private PlayerAnimationHandler _animator;

    [Header("Interaction")]
    private InteractionController _interactionController;
    private bool IsInteractionEnable = true;

    [Header("Audio")]
    
    [Header("Movement")]
    private Vector3 _inputVector;
    private FacingDirection _currentFacingDirection;
    [SerializeField] private float _currentMoveSpeed;
    
    [Header("Combat")]
    private bool _isDead;
    private Vector3 _combatDirection;
    private bool _isDamageable;
    [SerializeField] private float _knockbackStrenght;
    [SerializeField] private float _attackRadius;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Effects")]
    [SerializeField] private ParticleSystem _recoveyParticles;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CapsuleCollider>();
        _interactionController = GetComponent<InteractionController>();
        _animator = GetComponentInChildren<PlayerAnimationHandler>();
        _visual = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnEnable()
    {
        _isDead = false;
    }
    private void SetHealth()
    {
        if (PlayerPrefs.HasKey("Health"))
        {
            var currentHealth = PlayerPrefs.GetInt("Health");
        }
        //_healthController.SetupLife();
    }

    private void Start()
    {
        SetHealth();
        WhenFacingDirectionChange();
    }

    private void Update()
    {
        DetermineFacingDirection();
    }

    private void PlayAnimation()
    {
       _animator.SetWalking(_inputVector);
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        PlayAnimation();
    }

    private void OnDisable()
    {
    }
    
    #region Interaction

    public void ToggleInteraction(bool enable)
    {
        IsInteractionEnable = enable;
    }

    public void ToggleDamageable(bool enable)
    {
        _isDamageable = enable;
    }

    public void Interact(InputAction.CallbackContext ctx)
    {
        if (!IsInteractionEnable)
            return;
        if (ctx.started)
        {
            _interactionController.Interact();
        }
    }

    #endregion

    #region Combat

    public void Attack(InputAction.CallbackContext context)
    {
        if (!context.started)
            return;
        DoDamage(1, _attackRadius, .5f);
    }

    public void DoDamage(int damageAmount, float radius, float range)
    {
        Vector2 attackOrigin = (Vector2)transform.position + (Vector2)_combatDirection * range * 0.5f;

        // Use a small overlap circle to detect enemies in direction
        Collider2D hit = Physics2D.OverlapCircle(attackOrigin, radius, enemyLayer);

        if (hit != null)
        {
            Debug.Log("Hit enemy: " + hit.name);

            IDamageable damageableEnemy = hit.GetComponent<IDamageable>();
            if (damageableEnemy != null)
            {
                damageableEnemy.TakeDamage(1);
            }
        }
    }

    private void OnDrawGizmos()
    {
        var direction = _currentFacingDirection == FacingDirection.Right ? Vector3.right : Vector3.left;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + (direction * _attackRadius), _attackRadius);
    }

    public void ApplyKnockback(Vector3 origin, float intensity)
    {
        Vector3 knockbackDirection = (transform.position - origin).normalized;
        Vector3 stopMove = Vector3.up * _rb.linearVelocity.y;
        _animator.SetIdle();

        _rb.AddForce(stopMove, ForceMode2D.Impulse);
        _rb.AddForce(_knockbackStrenght * intensity * _rb.mass * knockbackDirection, ForceMode2D.Impulse);
    }

    #endregion

    #region Movement

    private void ApplyMovement()
    {   var moveDirection = _inputVector * _currentMoveSpeed;
        _rb.linearVelocity = new Vector3(moveDirection.x, moveDirection.z, 0);
    }

    public void ToggleVisualVisibility(bool enable)
    {
        _visual.enabled = enable;
    }

    #endregion

    #region Input Handler
    
    public void SetMovementInput(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            _inputVector = Vector3.zero;
            return;
        }

        var input = context.ReadValue<Vector2>();
        _inputVector = input.ToVector3().normalized;
        _combatDirection = (_inputVector == Vector3.zero) ? _combatDirection : _inputVector.normalized;

    }

    #endregion

    #region Facing Direction
    private void DetermineFacingDirection()
    {
        var newFacingDirection = _currentFacingDirection;
        if (_inputVector.x > 0)
            newFacingDirection = FacingDirection.Right;
        else if (_inputVector.x < 0)
            newFacingDirection = FacingDirection.Left;

        if (newFacingDirection != _currentFacingDirection)
        {
            _currentFacingDirection = newFacingDirection;
            WhenFacingDirectionChange();
        }
    }

    private void WhenFacingDirectionChange()
    {
        FlipSprite();
    }

    #endregion

    #region Visual

    private void FlipSprite()
    {
        _visual.flipX = _currentFacingDirection != FacingDirection.Right;
        _hat.flipX = _currentFacingDirection != FacingDirection.Right;
    }

    
    public void ToggleCollision(bool enable)
    {
        _collider.enabled = enable;
    }

    #endregion

    #region Inventory

    public void GetItem(InventoryItemData itemData)
    {
        _inventorySystem.Add(itemData);
    }
    #endregion

}

