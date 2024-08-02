using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rb;
    private TrailRenderer _trailRenderer;
    private GunSystem _weapon;

    [Header("-----Player UI-----")]
    [SerializeField]
    private TMP_Text _textUI;
    [SerializeField]
    private Button _rBTN;
    [SerializeField]
    private Image[] _weaponImages;
    [SerializeField]
    private Image[] borders;

    [Header("-----Player Stat-----")]
    [SerializeField]
    private int playerHealth;
    [SerializeField]
    private float movementSpeed = 10f;
    [SerializeField]
    private float dashSpeed = 50f;
    [SerializeField]
    private float dashDuration = 0.5f;
    [SerializeField]
    private float immortalTime;
    [SerializeField]
    private GameObject barrier;

    [Header("-----Gun-----")]
    [SerializeField]
    private int selectedWeapon = 0;
    [SerializeField]
    private Transform _gunPosition;

    private PlayerInputController _input;
    private int currentPlayerHealth = 0;
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
    private LightTrigger _lightTrigger;

    [SerializeField]
    private UnityEvent<float> onHealthChange;

    public int CurrentPlayerHealth
    { get
        {
            return currentPlayerHealth;
        }
        set  
        {
            currentPlayerHealth = value; 
            onHealthChange?.Invoke((float)currentPlayerHealth / playerHealth);
        } 
    }

    private void Awake()
    {
        _isGamePause = false;
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _trailRenderer = GetComponent<TrailRenderer>();
        aimTransform = transform.Find("Aim");
        _weapon = _gunPosition.GetComponentInChildren<GunSystem>();
        _input = GetComponent<PlayerInputController>();
    }

    private void OnEnable()
    {
        _input.OnInteract += Interact;
    }

    // Start is called before the first frame update
    void Start()
    {
        _playerSpeed = movementSpeed;

        if (currentPlayerHealth == 0) CurrentPlayerHealth = playerHealth;

        SelectedWeapon();
        cooldown = 0f;
    }

    private void FixedUpdate()
    {
        Debug.Log(CurrentPlayerHealth);
        cooldown -= Time.fixedDeltaTime;
        PlayerDirection();
        PlayerRotation();
        AimWeapon();
        
        if(_weapon != null)
        {
            if (_weapon.GetHoldShoot() == true)
            {
                WeaponHoldShoot();
            }
        }

        if(cooldown <= 0)
        {
            barrier.gameObject.SetActive(false);
        }
        else
        {
            barrier.gameObject.SetActive(true);
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

    


    public void DamageToPlayer(int damage)
    {
        if (cooldown <= 0f)
        {
            CurrentPlayerHealth -= damage;
            if (CurrentPlayerHealth <= 0)
            {
                Destroy(gameObject);
                Time.timeScale = 0f;
                _textUI.text = "Defeat";
                _textUI.color = Color.red;
            }
            cooldown = immortalTime;
        }
    }

    public void Heal(int heal)
    {
        CurrentPlayerHealth += heal;
        if (CurrentPlayerHealth >= 10)
        {
            CurrentPlayerHealth = playerHealth;
        }
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
            aimLocalScale.y = +1f;
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
            borders[0].color = Color.green;
            borders[1].color = Color.white;
            borders[2].color = Color.white;
        }
        else if (changeSlot == 1 && _gunPosition.childCount >= 2 && _gunPosition.GetChild(1) != null)
        {
            selectedWeapon = 1;
            borders[0].color = Color.white;
            borders[1].color = Color.green;
            borders[2].color = Color.white;
        }
        else if (changeSlot == 2 && _gunPosition.childCount >= 3 && _gunPosition.GetChild(2) != null)
        {
            selectedWeapon = 2;
            borders[0].color = Color.white;
            borders[1].color = Color.white;
            borders[2].color = Color.green;
        }

        if (previousSelectedWeapon != selectedWeapon)
        {
            SelectedWeapon();
        }
    }

    public void WeaponShoot()
    {
        if (!_isShoot && Time.timeScale == 1f)
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
        if(_holdShoot == 1 && Time.timeScale == 1f)
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
        if(CurrentPlayerHealth > 0)
        {
            if (!_isGamePause)
            {
                Time.timeScale = 0f;
                _isGamePause = true;
                _textUI.text = "Pause";
            }
            else
            {
                Time.timeScale = 1f;
                _isGamePause = false;
            }
        }
    }

    public void Interact()
    {
        if (_lightTrigger != null)
        {
            _lightTrigger.ToggleLights();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Weapon"))
        {
            foreach (Transform weapon in _gunPosition)
            {
                weapon.gameObject.SetActive(false);
            }

            //Add gun to slot
            GameObject newWeapon = Instantiate(other.gameObject);
            newWeapon.transform.parent = _gunPosition;
            newWeapon.transform.position = _gunPosition.position;
            newWeapon.transform.rotation = _gunPosition.rotation;
            newWeapon.transform.localScale = Vector3.one;
            _weapon = newWeapon.GetComponent<GunSystem>();
            _weapon.GetComponent<Collider2D>().enabled = false;
            changeSlot = _gunPosition.childCount - 1;
            Destroy(other.gameObject);

            //Add weapon sprite to UI
            int weaponImageCount = _gunPosition.childCount - 1;
            _weaponImages[weaponImageCount].enabled = true;
            _weaponImages[weaponImageCount].sprite = _weapon.GetImage();
        }

        if(other.gameObject.CompareTag("Finish"))
        {
            SaveSystem.instance.ResetData();
            Destroy(gameObject);
            Time.timeScale = 0f;
            _textUI.text = "You can escape!";
            _textUI.color = Color.white;
            _rBTN.gameObject.SetActive(false);
        }

        _lightTrigger = other.GetComponent<LightTrigger>();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _lightTrigger = null;
    }
}
