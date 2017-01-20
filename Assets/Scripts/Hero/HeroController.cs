using UnityEngine;
using System.Collections;

public class HeroController : MonoBehaviour
{
    [SerializeField]
    GameObject model;

    [SerializeField]
    public int cameraHorizontalSpeed;

    [SerializeField]
    public int cameraVerticalSpeed;

    [SerializeField]
    public int heroSpeed;

    [SerializeField]
    public int jumpHeight;

    Entity _entity;
    Transform _heroTransform;
    Transform _modelTransform;
    Transform _camTransform;
    CharacterController _characterController;
	HeroCameraController _cameraController;

    private Animator animator;

    bool _isFrozen;
    public bool IsFrozen
    {
        get { return (_isFrozen); }
        set
        {
            _isFrozen = value;
            Cursor.visible = value;
            Cursor.lockState = (value) ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }

    bool _movementBlocked;
    public bool MovementBlocked
    {
        get { return (_movementBlocked); }
        set { _movementBlocked = value; }
    }

    bool _cameraBlocked;
    public bool CameraBlocked
    {
        get { return (_cameraBlocked); }
        set { _cameraBlocked = value; }
    }

    void Start()
    {
        _entity = this.GetComponent<Entity>();
        _heroTransform = this.GetComponent<Transform>();
		if (GetComponentInChildren<HeroCameraController>() != null)
		_cameraController = GetComponentInChildren<HeroCameraController>();
        _characterController = this.GetComponent<CharacterController>();
        _modelTransform = model.GetComponent<Transform>();
        _camTransform = transform.Find("Main Camera");

        _isFrozen = false;
        _movementBlocked = false;
        _cameraBlocked = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        GetComponent<Entity>().addCallbackState(Entity.e_EntityState.STUN, OnStunChanged);
        GameObject.Find("SpellsUI").GetComponent<SpellsUI>().enabled = true;
        animator = model.GetComponent<Animator>();
        this.GetComponent<Spells.SpellLauncher>().LaunchCallback += OnSpellLaunched;
    }

    void Update()
    {
        if (!_isFrozen && !_cameraBlocked)
        {
            HandleCursorVisibility();
            HandleCameraMovement();
        }
        if (!_isFrozen && !_movementBlocked && _entity.getRemainingStateTime(Entity.e_EntityState.ROOT) <= 0)
        {
            HandleHeroMovement();
        }
    }

    void HandleCursorVisibility()
    {
        if (Input.GetButton("UnlockCursor"))
        {
            _cameraBlocked = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void HandleCameraMovement()
    {
		if (_cameraController != null)
		{
			_cameraController.handle = true;
		}
		else
		{
			float camRotation = _camTransform.localEulerAngles.x;
			float playerInput = -Input.GetAxis("Vertical");

			if ((camRotation >= 0f && camRotation < 180 && playerInput < 0) || ((camRotation <= 60f || camRotation > 180) && playerInput > 0))
			{
				_camTransform.RotateAround(_heroTransform.position, _camTransform.right, playerInput * Time.deltaTime * cameraVerticalSpeed);
			}
			_camTransform.RotateAround(_heroTransform.position, Vector3.up, Input.GetAxis("Horizontal") * Time.deltaTime * cameraHorizontalSpeed);
		}
	}

    void HandleHeroMovement()
    {
        float frontMovement = 0f;
        float sideMovement = 0f;
        Vector3 camAngle = _camTransform.localEulerAngles;
        Vector3 translation;

        if (Input.GetButton("HeroForwardMovement"))
        {
            frontMovement = _entity.getStat(Entity.e_StatType.SPEED) / 150;
        }
        if (Input.GetButton("HeroBackwardMovement"))
        {
            frontMovement = -_entity.getStat(Entity.e_StatType.SPEED) / 150;
        }
        if (Input.GetButton("HeroLeftMovement"))
        {
            sideMovement = -_entity.getStat(Entity.e_StatType.SPEED) / 150;
        }
        if (Input.GetButton("HeroRightMovement"))
        {
            sideMovement = _entity.getStat(Entity.e_StatType.SPEED) / 150;
        }

        if (frontMovement != 0 || sideMovement != 0)
            animator.SetBool("walk", true);
        else
            animator.SetBool("walk", false);

        _camTransform.eulerAngles = new Vector3(0, camAngle.y, camAngle.z);
        translation = (sideMovement * _camTransform.right + frontMovement * _camTransform.forward) * heroSpeed;
        translation.y = 0;
        _camTransform.eulerAngles = camAngle;
        if (translation != Vector3.zero)
        {
            _characterController.SimpleMove(translation);
            LookForward();
            _cameraBlocked = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void BlockDuringCast(float blockTime)
    {
        _movementBlocked = true;
        Invoke("Unblock", blockTime);
    }

    void Unblock()
    {
        _movementBlocked = false;
    }

    public void LookForward()
    {
        _modelTransform.localEulerAngles = new Vector3(0, _camTransform.localEulerAngles.y, 0);
    }

    public void OnStunChanged(float time)
    {
        if (time > 0f)
            _isFrozen = true;
        else
            _isFrozen = false;
    }

    public void OnSpellLaunched(int spellId)
    {
        LookForward();
    }
}