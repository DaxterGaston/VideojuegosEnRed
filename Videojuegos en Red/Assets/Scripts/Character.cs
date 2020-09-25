using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviourPun
{
    [SerializeField] private float speed = 5f;

    [SerializeField] private int _maxBullets = 6;
    private int _currentBullets;


    private bool _isDead;

    // [SerializeField] private int _deathCounter;


    private GameObject _tabCanvas;
    private Transform _spawnPoint;


    private Rigidbody _rigidbody;
    private Animator _animator;
    private float _hp;
    private int _facingDirection = 1;

    private float _h, _v;

    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject _viewCone;
    [SerializeField] private Collider _collider;
    

    [SerializeField] private Image _hpImage;
    [SerializeField] private Image _otherHpImage;


    [SerializeField] private Material _hideMaterial;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private Hashtable _customProp = new Hashtable();

    public Transform SpawnPoint { get => _spawnPoint; set => _spawnPoint = value; }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _animator = GetComponent<Animator>();
        _tabCanvas = GameObject.Find("TabbedCanvas");
        _hpImage = GameObject.Find("HPBar").GetComponent<Image>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _hp = 10;
        _hpImage.fillAmount = _hp / 10;
    }

    private void Start()
    {
        if (!photonView.IsMine)
        {
            _camera.gameObject.SetActive(false);
            _viewCone.SetActive(false);
            enabled = false;
            _spriteRenderer.material = _hideMaterial;
        }
        else
        {
            _otherHpImage.gameObject.SetActive(false);
        }
        // _customProp.Add("DeathCounter", _deathCounter);
        // PhotonNetwork.LocalPlayer.SetCustomProperties(_customProp);

        _currentBullets = _maxBullets;
    }

    private void Initialize()
    {
        _hp = 10;
        transform.position = _spawnPoint.position;
        UpdateHpBars();
    }

    private void UpdateHpBars()
    {
        _hpImage.fillAmount = _hp / 10;
        _otherHpImage.fillAmount = _hp / 10;
    }

    private void Update()
    {
        GetInputs();
        _animator.SetBool("IsWalking", _rigidbody.velocity.magnitude > 0.1f);

        
    }

    private void FixedUpdate() { _rigidbody.velocity = new Vector2(_h * speed, _v * speed); }

    private void GetInputs()
    {
        _h = Input.GetAxisRaw("Horizontal");
        _v = Input.GetAxisRaw("Vertical");

        _tabCanvas.SetActive(Input.GetKey(KeyCode.Tab));

        if (_isDead) return;

        if (Input.GetButtonDown("Fire1"))
        {
            // if (_currentWeapon != null) _currentWeapon.Shoot();

            if (_currentBullets <= 0)
            {
                Invoke("Reload", 2);
            }
            else
            {
                _currentBullets--;
                Shoot();
            }
        }


        // if (Input.GetKeyDown(KeyCode.I))
        // {
        //     _deathCounter++;
        //     _customProp["DeathCounter"] = _deathCounter;
        //     Debug.Log(_deathCounter);
        // }

        if (_h != 0f || _v != 0f)
        {
            _animator.SetFloat("X", _h);
            _animator.SetFloat("Y", _v);

            if (_h > 0)
            {
                _spriteRenderer.flipX = false;
            }
            else if (_h < 0)
            {
                _spriteRenderer.flipX = true;
            }
        }
    }

    public void Shoot()
    {
        var mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        var direction = (mousePosition - transform.position).normalized;
        var b = PhotonNetwork.Instantiate("Proyectile", transform.position + direction * 0.5f, Quaternion.identity);
        b.GetComponent<ProyectileController>().Creator = gameObject;

        b.transform.Rotate(0f, 0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
    }

    private void Reload() { _currentBullets = _maxBullets; }

    public void TakeDamage(int amount)
    {
        _hp -= amount;
        UpdateHpBars();

        if (_hp <= 0)
        {
            // _deathCounter += 1;

            // Debug.Log(_deathCounter);

            // _customProp["DeathCounter"] = _deathCounter;

            // Initialize();

            _isDead = true;
            var color = _spriteRenderer.color;
            _spriteRenderer.color = new Color(color.r, color.g, color.b, 0.5f);
            _collider.enabled = false;
            GetComponent<PhotonRigidbodyView>().enabled = false;
            
            photonView.RPC("DisableSprite", RpcTarget.Others);

        }
    }

    [PunRPC]
    private void DisableSprite()
    {
        _spriteRenderer.enabled = false;
    }

    private void OnCollisionEnter(Collision other) { }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger");


        if (photonView.IsMine && other.gameObject.CompareTag("Hit") && other.gameObject.GetComponent<ProyectileController>().Creator != gameObject)
        {
            Debug.Log("TriggeraDENTRO");

            TakeDamage(1);
        }
    }

    public void RestoreHealth(int amount)
    {
        if (_hp != 10)
            _hp += amount;
        if (_hp > 10)
            _hp = 10;

        UpdateHpBars();
    }
}