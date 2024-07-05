using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rb;
    private TrailRenderer _trailRenderer;
    private GunSystem _weapon;

    [Header("-----Player Health Bar")]
    [SerializeField]
    private HealthBar _healthBar;

    [Header("----------Player Stat----------")]
    [SerializeField]
    private float playerHealth;
    [SerializeField]
    private float movementSpeed = 10f;
    [SerializeField]
    private float dashSpeed = 50f;
    [SerializeField]
    private float dashDuration = 0.5f;
    [SerializeField]
    private float immortalTime;

    [Header("----------Gun----------")]
    [SerializeField]
    private int selectedWeapon = 0;
    [SerializeField]
    private Transform _gunPosition;

    private float currentPlayerHealth;
    private Animator _animator;
    private Vector2 _movement;
    private Vector2 _mousePosition;
    private Vector2 lookDir;
    private float _playerSpeed;
    private bool _canDash = false;
    private Vector3 _aimDir;
    private bool _isShoot = false;
    private Transform aimTransform;
    private float _holdShoot;
    private bool _isGamePause;
    private int changeSlot;
    private float cooldown;

    private void Awake()
    {
        _isGamePause = false;
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _trailRenderer = GetComponent<TrailRenderer>();
        aimTransform = transform.Find("Aim");
        _weapon = _gunPosition.GetComponentInChildren<GunSystem>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _playerSpeed = movementSpeed;
        currentPlayerHealth = playerHealth;
        SelectedWeapon();
        cooldown = 0f;
    }

    private void FixedUpdate()
    {
        cooldown -= Time.fixedDeltaTime;
        PlayerDirection();
        PlayerRotation();
        AimWeapon();
        if (_weapon.GetHoldShoot() == true)
        {
            WeaponHoldShoot();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Create animation that can rotate character in 2D
        var velocity = _rb.velocity;
        if (velocity.magnitude > 0.1f)
        {
            _animator.SetBool("IsRunning", true);
        }
        else
        {
            _animator.SetBool("IsRunning", false);
        }

        _animator.SetFloat("Horizontal", lookDir.x);
        _animator.SetFloat("Vertical", lookDir.y);
        ChangeWeapon();
    }
    

    public void DamageToPlayer(float damage)
    {
        if (cooldown <= 0f)
        {
            currentPlayerHealth -= damage;
            immortalTime = 3f;
            if (currentPlayerHealth <= 0)
            {
                GameManager.ExitGame();
            }
            cooldown = immortalTime;
        }
        _healthBar.UpdateHealthBar(playerHealth, currentPlayerHealth);
    }

    public void Heal(float heal)
    {
        Debug.Log(currentPlayerHealth);
        currentPlayerHealth += heal;
        if (currentPlayerHealth >= 10)
        {
            currentPlayerHealth = playerHealth;
        }
        _healthBar.UpdateHealthBar(playerHealth, currentPlayerHealth);
    }

    private void PlayerDirection()
    {
        //Move Character by WASD
        _rb.velocity = _movement * _playerSpeed;
    }

    private void PlayerRotation()
    {
        //Rotate Character smoothly
        lookDir = _mousePosition - _rb.position;
        lookDir.Normalize();
    }

    public void MovementInput(Vector2 input)
    {
        //Set input to movement
        _movement = input;
    }

    public void MousePosition(Vector2 mousePosition)
    {
        //Set input by mouse position to rotate player
        _mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
    }

    public void Dashing()
    {
        //Check Dashing for player
        if (!_canDash)
        {
            _animator.SetBool("IsDash", true);
            _canDash = true; //If player ready to dash, so start coroutine
            SoundManager.PlaySound(SoundType.Dash);
            StartCoroutine(DashingTime());
        }
        else
        {
            return;
        }

    }

    private IEnumerator DashingTime()
    {
        _playerSpeed = dashSpeed; //Change normal speed to dash speed to give speed to player a little bit time 
        _trailRenderer.emitting = true; //When player dash, Line trail will appear
        yield return new WaitForSeconds(dashDuration);
        _trailRenderer.emitting = false;
        _playerSpeed = movementSpeed; //Set speed back to normal
        _animator.SetBool("IsDash", false);
        _canDash = false;
    }

    private void AimWeapon()
    {
        //Set weapon aim with cursor
        _aimDir = _mousePosition - _rb.position;
        _aimDir.Normalize();
        float angle = Mathf.Atan2(_aimDir.y, _aimDir.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0, 0, angle);

        //Check degree of weapon to flip
        Vector3 aimLocalScale = Vector3.one;
        if (angle > 90 || angle < -90)
        {
            aimLocalScale.y = -1f;
        }
        else
        {
            aimLocalScale.x = +1f;
        }
        aimTransform.localScale = aimLocalScale;
    }

    public void WeaponSlot1(int value)
    {
        changeSlot = value;
    }

    public void WeaponSlot2(int value)
    {
        changeSlot = value;
    }

    public void WeaponSlot3(int value)
    {
        changeSlot = value;
    }

    private void SelectedWeapon()
    {
        int i = 0;
        foreach (Transform weapon in _gunPosition)
        {
            if (i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
            i++;
            _weapon = _gunPosition.GetComponentInChildren<GunSystem>();
        }
    }

    private void ChangeWeapon()
    {
        int previousSelectedWeapon = selectedWeapon;
        if (changeSlot == 0)
        {
            selectedWeapon = 0;
        }
        else if (changeSlot == 1 && _gunPosition.childCount >= 2 && _gunPosition.GetChild(1) != null)
        {
            selectedWeapon = 1;
        }
        else if (changeSlot == 2 && _gunPosition.childCount >= 3 && _gunPosition.GetChild(2) != null)
        {
            selectedWeapon = 2;
        }

        if (previousSelectedWeapon != selectedWeapon)
        {
            SelectedWeapon();
        }
    }

    public void WeaponShoot()
    {
        if (!_isShoot)
        {
            _isShoot = true;
            _weapon?.Shoot();
            _isShoot = false;
        }
        else
        {
            return;
        }
    }

    private void WeaponHoldShoot()
    {
        if(_holdShoot == 1)
        {
            _isShoot = true;
            _weapon?.Shoot();
        }
        else
        {
            _isShoot = false;
        }
    }

    public void WeaponHoldShootInput(float value)
    {
        _holdShoot = value;
    }

    public void ESCInput()
    {
        if(!_isGamePause)
        {
            GameManager.Pause();
            _isGamePause = true;
        }
        else
        {
            GameManager.Resume();
            _isGamePause = false;
        }
    }

    public void PlayerInteract()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Weapon"))
        {
            foreach (Transform weapon in _gunPosition)
            {
                weapon.gameObject.SetActive(false);
            }
            GameObject newWeapon = Instantiate(other.gameObject);
            newWeapon.transform.parent = _gunPosition;
            newWeapon.transform.position = _gunPosition.position;
            newWeapon.transform.rotation = _gunPosition.rotation;
            _weapon = newWeapon.GetComponent<GunSystem>();
            changeSlot = _gunPosition.childCount - 1;
            Destroy(other.gameObject);
        }
    }
}
