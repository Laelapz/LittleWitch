using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

[RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour, IDamageable
{
    private Rigidbody2D _rb;
    private Collider _collider;
    private SpriteRenderer _visual;

    [SerializeField] private InventorySystem _inventorySystem;

    private Vector3 _inputVector;
    public static GameObject Player;
    public static event Action OnPlayerDies;

    [Header("Animation")]
    private PlayerAnimationHandler _animator;

    [Header("Interaction")]
    private InteractionController _interactionController;
    private bool IsInteractionEnable = true;

    [Header("Combat")]
    private IHealthController _healthController;
    
    public int CurrentHealth => _healthController.Health;


    [Header("Audio")]
    
    [Header("Movement")]
    private bool _isGrounded;
    private bool _isGravityEnable = true;
    private bool _isMovementEnable = true;
    private FacingDirection _currentFacingDirection;
    [SerializeField] private float _currentMoveSpeed;
    
    [Header("Combat")]
    private bool _isDead;
    private bool _isCombatible = true;
    private Vector3 _combatDirection;
    private bool _isDamageable;
    [SerializeField] private float _knockbackStrenght;
    [SerializeField] private int _groundLayers;
    [SerializeField] private float _distanceToGroundCheck;
    [SerializeField] private float _attackRadius;
    //Hit Combo
    private bool _attackPressed;
    private int _attackIndex;
    private bool _isAttacking;

    [Header("Effects")]
    [SerializeField] private ParticleSystem _recoveyParticles;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CapsuleCollider>();
        _interactionController = GetComponent<InteractionController>();
        _animator = GetComponentInChildren<PlayerAnimationHandler>();
        _visual = GetComponentInChildren<SpriteRenderer>();
        Player = gameObject;
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
        if (!_isMovementEnable) return;
        DetermineFacingDirection();
    }

    private void PlayAnimation()
    {
        if (_isAttacking) return;

        _animator.SetWalking(_inputVector);
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        PlayAnimation();
    }

    private void OnDisable()
    {

        if (_healthController.Health > 0)
        {
            PlayerPrefs.SetInt("Health", _healthController.Health);
        }
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

    public void ToggleCombat(bool enable)
    {
        _isCombatible = enable;
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (!context.started || !_isCombatible)
            return;

        //attack
    }

    //private float GetCurrentAnimationLength(string stateName)
    //{
    //    var clip = _animator.runtimeAnimatorController.animationClips.FirstOrDefault(x => x.name == stateName);
    //    return clip != null ? clip.length : 0f;
    //}

    public void DoDamage(int damageAmount, float radius, float range, bool recoveryMana = false)
    {
        var direction = _currentFacingDirection == FacingDirection.Right ? Vector3.right : Vector3.left;
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, radius, direction, range);
        foreach (RaycastHit hit in hits)
        {
            if (!hit.collider.CompareTag("Player") && hit.collider.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(damageAmount);
            }
        }
    }

    private void OnDrawGizmos()
    {
        var direction = _currentFacingDirection == FacingDirection.Right ? Vector3.right : Vector3.left;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + (direction * _attackRadius), _attackRadius);
    }

    public void TakeDamage(int damage)
    {
        if (!_isDamageable)
            return;
        
        _healthController.TakeDamage(damage);

        // Flash de dano

        if (_healthController.Health <= 0 && !_isDead)
        {
            _isDead = true;
            Die();
        }
    }

    public void Die()
    {
        gameObject.SetActive(false);
        PlayerPrefs.SetFloat("Mana", 0);
        PlayerPrefs.DeleteKey("Health");
        OnPlayerDies?.Invoke();
    }

    public void ApplyKnockback(Vector3 origin, float intensity)
    {
        Vector3 knockbackDirection = (transform.position - origin).normalized;
        Vector3 stopMove = Vector3.up * _rb.linearVelocity.y;
        _animator.SetIdle();

        _rb.AddForce(stopMove, ForceMode2D.Impulse);
        _rb.AddForce(_knockbackStrenght * intensity * _rb.mass * knockbackDirection, ForceMode2D.Impulse);

        //KnockbackTimer().Forget();
    }

    //private async UniTaskVoid KnockbackTimer()
    //{
    //    _knockbackCancelToken.SafeCancel();
    //    _knockbackCancelToken = new CancellationTokenSource();
    //    try
    //    {
    //        _isMovementEnable = false;
    //        float velocityMagnitude;
    //        do
    //        {
    //            await UniTask.Delay(TimeSpan.FromSeconds(0.05f));
    //            velocityMagnitude = new Vector2(_rb.velocity.x, _rb.velocity.z).magnitude;
    //            await UniTask.NextFrame(cancellationToken: _knockbackCancelToken.Token);
    //        }
    //        while (velocityMagnitude > _data.VelocityToCancelKnockback);

    //        _isMovementEnable = true;
    //    }
    //    catch
    //    {

    //    }
    //}

    #endregion

    #region Movement

    private void ApplyMovement()
    {
        if (!_isMovementEnable)
        {
            print("Movimento não ativado");
            return;
        }

        if (_isAttacking)
        {
            ToggleMovement(false);
            return;
        };

        var moveDirection = _inputVector * _currentMoveSpeed;
        _rb.linearVelocity = new Vector3(moveDirection.x, moveDirection.z, 0);
    }

    public void ToggleMovement(bool enable)
    {
        _isMovementEnable = enable;

        if (!enable)
            _rb.linearVelocity = Vector3.zero;
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
        if (!_isMovementEnable) return;

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

