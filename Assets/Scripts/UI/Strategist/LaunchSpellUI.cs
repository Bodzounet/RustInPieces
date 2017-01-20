using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace StrategistUI
{
    public class LaunchSpellUI : MonoBehaviour
    {
        [SerializeField]
        private Spells.SpellLauncher _spellLauncher;
		private GameObject _damageText;
        public Spells.SpellLauncher SpellLauncher
        {
            get { return _spellLauncher; }
        }

        public Camera cam;

		private Canvas _canvas;
        public GameObject markPrefab;

        public Projector projector;

        private StrategistManager _strategistManager;
        public StrategistManager StrategistManager
        {
            get { return _strategistManager; }
        }

        private TeamFactory _teamfactory;
        private PhotonView _photonView;
        private string _spellProjected = null;

        void Awake()
        {
            projector.enabled = false;
            _strategistManager = GetComponentInParent<StrategistManager>();
            _photonView = GetComponentInParent<PhotonView>();
            _teamfactory = GameObject.FindObjectsOfType<TeamFactory>().Single(x => x.MyTeam == _strategistManager.Team);
			_damageText = Resources.Load("UI/DamageDoneText") as GameObject;
			_canvas = this.GetComponentInParent<Canvas>();
		}

        void Update()
        {
            if (_photonView.isMine)
            {
                HandleProjector();
                if (Input.GetMouseButtonDown(0))
                    OnLeftClick();
                if (Input.GetMouseButtonDown(1))
                    OnRightClick();
            }
        }

        private void HandleProjector()
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Ground"));
            projector.transform.position = new Vector3(hit.point.x, hit.point.y + 10, hit.point.z - 2);
        }

        public void Launch(string spellId)
        {
            Spells.SpellInfo infos = _spellLauncher.getSpellInfoFromId(spellId);
            switch (infos.CastType)
            {
                case Spells.SpellInfo.e_CastType.NO_TARGET:
                LaunchInstant(spellId);
                _spellProjected = null;
                projector.enabled = false;
                break;
                case Spells.SpellInfo.e_CastType.SELFBUFF:
                LaunchToGo(spellId, _strategistManager.gameObject);
                _spellProjected = null;
                projector.enabled = false;
                break;
                case Spells.SpellInfo.e_CastType.TARGET:
                case Spells.SpellInfo.e_CastType.POSITION:
                projector.enabled = true;
                _spellProjected = spellId;
                break;
                default:
                break;
            }
        }


        private void OnLeftClick()
        {
            if (projector.enabled && _spellProjected != null)
            {
                Spells.SpellInfo infos = _spellLauncher.getSpellInfoFromId(_spellProjected);

                if (_spellLauncher.IsSpellInCooldown(_spellProjected))
                {
                    switch (infos.CastType)
                    {
                        case Spells.SpellInfo.e_CastType.POSITION:
                        RaycastHit hit;
                        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                        Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Ground"));
                        if (_spellLauncher.isTargetValid(_spellProjected, _strategistManager.gameObject, _strategistManager.Team, hit.collider.gameObject) == Spells.SpellLauncher.e_LaunchReturn.ok)
                        {
                            LaunchToPos(_spellProjected, hit.point);
                            OnRightClick();
                        }
						else
							{
								//POS WRONG
								GameObject newText = Instantiate(_damageText, _canvas.transform) as GameObject;
								newText.transform.position = Input.mousePosition;
								Destroy(newText.GetComponent<BillboardGraphics>());
								newText.GetComponent<DamageDone>().Text("Invalid Position", Color.white);
								newText.transform.localScale = Vector3.one;
							}
                        break;
                        case Spells.SpellInfo.e_CastType.TARGET:
                        RaycastHit hitBox;
                        bool touched = Physics.BoxCast(cam.transform.position, new Vector3(0.1f, 0.1f, 0.1f), cam.ScreenPointToRay(Input.mousePosition).direction, out hitBox, Quaternion.identity, Mathf.Infinity, LayerMask.GetMask("Entity", "MinionEntity", "HeroEntity"));
                        if (touched)
                        {
                            Entity ent = hitBox.collider.GetComponent<Entity>();
                            touched = _spellLauncher.isTargetValid(_spellProjected, _strategistManager.gameObject, _strategistManager.Team, hitBox.collider.gameObject) == Spells.SpellLauncher.e_LaunchReturn.ok ? true : false;
                            if (touched)
                            {
                                LaunchToGo(_spellProjected, hitBox.collider.gameObject);
                                OnRightClick();
                            }
                        }
                        if (!touched)
                        {
								// WRONG ! (NOT VALID TARGET)

								GameObject newText = Instantiate(_damageText, _canvas.transform) as GameObject;
								newText.transform.position = Input.mousePosition;
								Destroy(newText.GetComponent<BillboardGraphics>());
								newText.GetComponent<DamageDone>().Text("Invalid Target", Color.white);
								newText.transform.localScale = Vector3.one;
						}
                        break;
                        default:
                        Debug.Log("something is wrong");						
                        break;
                    }
                }
                else
                {
					// WRONG ! (CD)
					GameObject newText = Instantiate(_damageText, _canvas.transform) as GameObject;
					newText.transform.position = new Vector3(100, 50);
					Destroy(newText.GetComponent<BillboardGraphics>());
					newText.GetComponent<DamageDone>().Text("Spell in Cooldown", Color.white);
					newText.transform.localScale = Vector3.one;
				}
            }
        }

        private void OnRightClick()
        {
            _spellProjected = null;
            projector.enabled = false;
        }

        private void LaunchInstant(string id)
        {
            //Debug.Log("I should launch an INSTANT but there is no spell created yet");

            RaiseEventOptions reo = new RaiseEventOptions();
            reo.Receivers = ExitGames.Client.Photon.ReceiverGroup.All;

            RaiseEventStrategist.LaunchInstantData lid = new RaiseEventStrategist.LaunchInstantData();
            lid.spellId = id;
            lid.pid = _strategistManager.GetStrategistPhotonView.viewID;

            PhotonNetwork.RaiseEvent(EventCode.STRAT_LAUNCH_INSTANT, lid, true, reo);
        }

        private void LaunchToPos(string id, Vector3 pos)
        {
            RaiseEventOptions reo = new RaiseEventOptions();
            reo.Receivers = ExitGames.Client.Photon.ReceiverGroup.All;

            RaiseEventStrategist.LaunchToPositionData ltpd = new RaiseEventStrategist.LaunchToPositionData();
            ltpd.spellId = id;
            ltpd.position = pos;
            ltpd.pid = _strategistManager.GetStrategistPhotonView.viewID;

            PhotonNetwork.RaiseEvent(EventCode.STRAT_LAUNCH_TO_POSITION, ltpd, true, reo);

            //Debug.Log("I should launch an AOE but there is no spell created yet");
        }

        private void LaunchToGo(string id, GameObject go)
        {

            RaiseEventOptions reo = new RaiseEventOptions();
            reo.Receivers = ExitGames.Client.Photon.ReceiverGroup.All;

            RaiseEventStrategist.LaunchToTargetData lttd = new RaiseEventStrategist.LaunchToTargetData();
            lttd.spellId = id;
            lttd.pid = _strategistManager.GetStrategistPhotonView.viewID;

            //List<int> mt = new List<int>();
            List<int> ht = new List<int>();

            foreach (var v in PlayersInfos.Instance.heroList)
            {
                if (v == go)
                {
                    ht.Add(go.GetPhotonView().viewID);
                }
            }

            lttd.heroTargets = ht.ToArray().Clone() as int[];

            ht.Clear();
            foreach (var v in PlayersInfos.Instance.strategistList)
            {
                foreach (var w in v.GetComponent<MinionManager>().Minions)
                {
                    if (w == go)
                        ht.Add(go.GetComponent<MinionManager_CleanMinion>().IdMinion);
                }
            }

            lttd.minionTargets = ht.ToArray().Clone() as int[];
            PhotonNetwork.RaiseEvent(EventCode.STRAT_LAUNCH_TO_TARGET, lttd, true, reo);
        }


        //private MarkActivator _currentMark;
        //private void AddMarks(SpellUI.e_TargetType targetType, SpellUI.e_TargetAffiliation targetAffiliation)
        //{
        //    if (targetType == SpellUI.e_TargetType.HeroEntity)
        //    {
        //        e_Team targetTeam = (targetAffiliation == SpellUI.e_TargetAffiliation.Ally ? _strategistManager.Team : (_strategistManager.Team == e_Team.TEAM1 ? e_Team.TEAM2 : e_Team.TEAM1));
        //        foreach (var v in PlayersInfos.Instance.heroList.Where(x => x.GetComponent<Entity>().Team == targetTeam))
        //        {
        //            AddMarkOnGo(v);
        //        }
        //    }
        //    else
        //    {
        //        foreach (var v in GetComponentInParent<StrategistManager>().minionManager.Minions)
        //        {
        //            AddMarkOnGo(v);
        //        }
        //        _teamfactory.OnMinionIsCreated += AddMarkOnGo;
        //    }

        //    _currentMark = this.gameObject.AddComponent<MarkActivator>();
        //    _currentMark.layermaskToTrack = targetType == SpellUI.e_TargetType.Entity ? LayerMask.NameToLayer("Entity") : (targetType == SpellUI.e_TargetType.HeroEntity ? LayerMask.NameToLayer("HeroEntity") : LayerMask.NameToLayer("MinionEntity"));
        //    _currentMark.cam = cam;
        //}

        //private void RemoveMarks()
        //{
        //    // cleaning marks

        //    foreach (var v in PlayersInfos.Instance.heroList)
        //    {
        //        var c = v.transform.FindChild("Mark");
        //        if (c != null)
        //        {
        //            Destroy(c.gameObject);
        //        }
        //    }

        //    foreach (var v in GetComponentInParent<StrategistManager>().minionManager.Minions)
        //    {
        //        var c = v.transform.FindChild("Mark");
        //        if (c != null)
        //        {
        //            Destroy(c.gameObject);
        //        }
        //    }
        //    _teamfactory.OnMinionIsCreated -= AddMarkOnGo;

        //    if (_currentMark != null)
        //    {
        //        Destroy(_currentMark);
        //        _currentMark = null;
        //    }
        //}

        //private GameObject AddMarkOnGo(GameObject go)
        //{
        //    var mark = Instantiate(markPrefab);
        //    mark.name = "Mark";
        //    mark.transform.SetParent(go.transform);
        //    mark.transform.forward = Vector3.up;
        //    mark.transform.localPosition = Vector3.zero + Vector3.up * 2;
        //    mark.transform.localScale = Vector3.one;

        //    return mark;
        //}

        //private int _layermaskToTrack = 1 << LayerMask.NameToLayer("MinionEntity"); // for now, because i can't track anything else
        //private Collider _highlightedMark = null;
        //private void _HightlightMarks()
        //{
        //    RaycastHit hit;
        //    Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        //    Physics.Raycast(ray, out hit, Mathf.Infinity, _layermaskToTrack);
        //    if (hit.collider != _highlightedMark)
        //    {
        //        if (_highlightedMark != null)
        //            _highlightedMark.transform.root.BroadcastMessage("OnMouseExitCustom");

        //        _highlightedMark = hit.collider;
        //        if (_highlightedMark != null)
        //            _highlightedMark.transform.root.BroadcastMessage("OnMouseEnterCustom");
        //    }
        //}
    }
}