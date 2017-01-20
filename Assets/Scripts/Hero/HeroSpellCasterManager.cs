using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HeroSpellCasterManager : MonoBehaviour
{
    private Projector _projector;
    private Transform _camTransform;
    private Spells.SpellLauncher _launcher;
    private PhotonView _pView;
    [SerializeField]
    private string[] _inputNames;
    private Animator _animator;
    private Entity _entity;
    private bool _castingSpell;

    void Start()
    {
        _projector = GetComponentInChildren<Projector>();
        _camTransform = Camera.main.gameObject.transform;
        _projector.enabled = false;
        _launcher = GetComponent<Spells.SpellLauncher>();
        _animator = this.GetComponentInChildren<Animator>();
        _entity = GetComponent<Entity>();
        _castingSpell = false;

        _pView = GetComponent<PhotonView>();
        if (_pView == null && PhotonNetwork.connectionState != ConnectionState.Connected)
            Debug.LogWarning("Attention, le lanceur de sort n'a pas de photon view alors que le jeu est en reseau");
    }

    void HandleKeyDown(int index)
    {
        string spellId = _launcher.GetSpellIDByIndex(index);
        Spells.SpellInfo.e_CastType type = _launcher.GetSpellCastTypeByIndex(index);

        if (_launcher.IsSpellInCooldown(index) && (_entity.getRemainingStateTime(Entity.e_EntityState.ROOT) <= 0 || !_launcher.getSpellInfoFromId(spellId).producesMovement))
        {
            _castingSpell = true;
            switch (type)
            {
                case Spells.SpellInfo.e_CastType.NO_TARGET:
                    break;
                case Spells.SpellInfo.e_CastType.POSITION:
                    _projector.enabled = true;
                    break;
                case Spells.SpellInfo.e_CastType.TARGET:
                    _projector.enabled = true;
                    break;
                case Spells.SpellInfo.e_CastType.TOGGLE:
                    break;
            }
        }
    }

    void HandleKeyUp(int index)
    {
        string spellId = _launcher.GetSpellIDByIndex(index);
        Spells.SpellInfo.e_CastType type = _launcher.GetSpellCastTypeByIndex(index);
        HeroSpellLaunchedInfo info = new HeroSpellLaunchedInfo();

        info.casterPhotonId = GetComponent<PhotonView>().viewID;
        info.castType = type;
        info.spellId = spellId;
        info.casterPosition = transform.position;
        info.casterRotation = gameObject.GetComponentInChildren<Animator>().transform.eulerAngles;
        if (_castingSpell == true && _launcher.IsSpellInCooldown(index) && (_entity.getRemainingStateTime(Entity.e_EntityState.ROOT) <= 0 || !_launcher.getSpellInfoFromId(spellId).producesMovement))
        {
            _castingSpell = false;
            RaiseEventOptions reo = new RaiseEventOptions();
            reo.Receivers = ExitGames.Client.Photon.ReceiverGroup.All;
            switch (type)
            {
                case Spells.SpellInfo.e_CastType.NO_TARGET:
                    if (PhotonNetwork.connectionState == ConnectionState.Connected)
                    {
                        //PhotonNetwork.RaiseEvent(EventCode.HERO_SPELL_LAUNCHED, (object)info, true, RaiseEventOptions.Default);
                        PhotonNetwork.RaiseEvent(EventCode.HERO_SPELL_LAUNCHED, (object)info, true, reo);
                    }
                    HandleAnimator(index, _animator);
                    //_launcher.Launch(_launcher.GetSpellIDByIndex(index));
                    break;

                case Spells.SpellInfo.e_CastType.POSITION:
                    RaycastHit hit;
                    Physics.Raycast(_projector.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity, LayerMask.GetMask("Ground"));
                    Debug.DrawLine(_projector.transform.position, hit.point);
                    if (PhotonNetwork.connectionState == ConnectionState.Connected)
                    {
                        info.spellPosition = hit.point;
                        //PhotonNetwork.RaiseEvent(EventCode.HERO_SPELL_LAUNCHED, (object)info, true, RaiseEventOptions.Default);
                        PhotonNetwork.RaiseEvent(EventCode.HERO_SPELL_LAUNCHED, (object)info, true, reo);
                    }
                    HandleAnimator(index, _animator);
                    //_launcher.Launch(_launcher.GetSpellIDByIndex(index), hit.point);
                    break;

                case Spells.SpellInfo.e_CastType.TARGET:
                    RaycastHit hitBox;
                    bool touched = Physics.BoxCast(_projector.transform.position, new Vector3(0.1f, 0.1f, 0.1f), Camera.main.transform.forward, out hitBox, Quaternion.identity, Mathf.Infinity, LayerMask.GetMask("Entity"));

                    if (touched)
                    {
                        if (PhotonNetwork.connectionState == ConnectionState.Connected)
                        {
                            info.target = hitBox.transform.gameObject.GetComponent<PhotonView>().viewID;
                            info.spellPosition = _launcher.gameObject.transform.position;
                            //PhotonNetwork.RaiseEvent(EventCode.HERO_SPELL_LAUNCHED, (object)info, true, RaiseEventOptions.Default);
                            PhotonNetwork.RaiseEvent(EventCode.HERO_SPELL_LAUNCHED, (object)info, true, reo);
                        }
                        HandleAnimator(index, _animator);
                        //_launcher.Launch(_launcher.GetSpellIDByIndex(index), new GameObject[] { hitBox.transform.gameObject }, new Vector3[] { _launcher.gameObject.transform.position });
                    }
                    break;

                case Spells.SpellInfo.e_CastType.SELFBUFF:
                    if (PhotonNetwork.connectionState == ConnectionState.Connected)
                    {
                        //PhotonNetwork.RaiseEvent(EventCode.HERO_SPELL_LAUNCHED, (object)info, true, RaiseEventOptions.Default);
                        PhotonNetwork.RaiseEvent(EventCode.HERO_SPELL_LAUNCHED, (object)info, true, reo);
                    }
                    HandleAnimator(index, _animator);
                    //_launcher.Launch(_launcher.GetSpellIDByIndex(index), new GameObject[] { gameObject });
                    break;
            }
        }
        _projector.enabled = false;
    }

    void HandleAnimator(int index, Animator animator)
    {
        switch (index)
        {
            case 0:
                animator.SetTrigger("AutoAttack");
                break;
            case 2:
                animator.SetTrigger("Spell1");
                break;
            case 3:
                animator.SetTrigger("Spell2");
                break;
            case 4:
                animator.SetTrigger("Spell3");
                break;
            case 5:
                animator.SetTrigger("Ult");
                break;
        }
    }

    void HandleSpells()
    {
        for (int index = 0; index < _inputNames.Count(); ++index)
        {
            if (_inputNames[index] == "")
                continue;
            if (Input.GetButtonUp(_inputNames[index]))
            {
                HandleKeyUp(index);
            }
            if (Input.GetButtonDown(_inputNames[index]))
            {
                HandleKeyDown(index);
            }
        }
    }

    void HandleProjector()
    {
        float camHeight = _camTransform.localPosition.y;
        _projector.fieldOfView = camHeight * 5;
        _projector.gameObject.transform.localPosition = new Vector3(0, 5.5f - camHeight);
    }

    void Update()
    {
        HandleSpells();
        HandleProjector();
    }
}
