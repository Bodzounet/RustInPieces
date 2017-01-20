using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace StrategistUI
{
    public class StrategistControls : MonoBehaviour
    {
        private Vector3 movement;
        private float _speed = 5000;

        private Vector2 _screenSize;

        private float topScreenScrollStart;
        private float bottomScreenScrollStart;
        private float leftScreenScrollStart;
        private float rightScreenScrollStart;

        private bool[] _canMoveToDir = new bool[4]; // left, right, up, down;

        private bool _isFocusing = false;

        private Rigidbody _rgbd;
        private Camera _camera;

        public float minXDistanceBetweenCamAndBorder = 30;
        public float minYDistanceBetweenCamAndBorder = 20;

        void Start()
        {
            _rgbd = GetComponent<Rigidbody>();
            _camera = GetComponent<Camera>();

            Cursor.lockState = CursorLockMode.None;
            Cursor.lockState = CursorLockMode.Confined;

            _OnScreenIsResized();
            FixedUpdate();

        }

        void FixedUpdate()
        {
            if (!(_canMoveToDir[0] = !Physics.Raycast(transform.position, -Vector3.right, minXDistanceBetweenCamAndBorder)) && _rgbd.velocity.x < 0)
                _rgbd.velocity = new Vector3(0, _rgbd.velocity.y, _rgbd.velocity.z);
            if (!(_canMoveToDir[1] = !Physics.Raycast(transform.position, Vector3.right, minXDistanceBetweenCamAndBorder)) && _rgbd.velocity.x > 0)
                _rgbd.velocity = new Vector3(0, _rgbd.velocity.y, _rgbd.velocity.z);
            if (!(_canMoveToDir[2] = !Physics.Raycast(transform.position, Vector3.forward, minXDistanceBetweenCamAndBorder)) && _rgbd.velocity.z > 0)
                _rgbd.velocity = new Vector3(_rgbd.velocity.x, _rgbd.velocity.y, 0);
            if (!(_canMoveToDir[3] = !Physics.Raycast(transform.position, -Vector3.forward, minXDistanceBetweenCamAndBorder)) && _rgbd.velocity.z < 0)
                _rgbd.velocity = new Vector3(_rgbd.velocity.x, _rgbd.velocity.y, 0);
        }

        void Update()
        {
            if (_DoesScreenHasBeenResize())
                _OnScreenIsResized();

            _Zoom();

            if (_IsOverUIElement())
                return;
            _Move();
        }

        public void Focus(HeroEntity target)
        {
            //Debug.Log("NEED TO FOCUS ON " + target.EntityName);
            //Debug.Log("current pos = " + transform.position);
            //Debug.Log("need to go to = " + target.gameObject.transform.position);

            StopCoroutine("Co_Focus");
            StartCoroutine("Co_Focus", new Vector3(target.gameObject.transform.position.x, transform.position.y, target.gameObject.transform.position.z - 8));

            //_rgbd.MovePosition(new Vector3(target.gameObject.transform.position.x, transform.position.y, target.gameObject.transform.position.z - 8));
            //_isFocusing = true;
        }

        private void _Move()
        {
            movement = Vector3.zero;
            if (Input.mousePosition.x <= leftScreenScrollStart && _canMoveToDir[0])
                movement += new Vector3(((Input.mousePosition.x / (float)Screen.width) - 0.1f) * 10, 0);
            if (Input.mousePosition.x >= rightScreenScrollStart && _canMoveToDir[1])
                movement += new Vector3(((Input.mousePosition.x / (float)Screen.width) - 0.9f) * 10, 0);
            if (Input.mousePosition.y >= topScreenScrollStart && _canMoveToDir[2])
                movement += new Vector3(0, 0, ((Input.mousePosition.y / (float)Screen.height) - 0.9f) * 10);
            if (Input.mousePosition.y <= bottomScreenScrollStart && _canMoveToDir[3])
                movement += new Vector3(0, 0, ((Input.mousePosition.y / (float)Screen.height) - 0.1f) * 10);

            if (movement != Vector3.zero)
                StopCoroutine("Co_Focus");

            _rgbd.AddForce(movement.normalized * _speed);
        }

        private void _Zoom()
        {
            float modif = Input.GetAxis("Mouse ScrollWheel");
            _speed -= modif * 500;

            _camera.fieldOfView -= modif * 10;
            _camera.fieldOfView = _camera.fieldOfView > 100 ? 100 : _camera.fieldOfView;
            _camera.fieldOfView = _camera.fieldOfView < 10 ? 10 : _camera.fieldOfView;
        }

        private bool _IsOverUIElement()
        {
            PointerEventData data = new PointerEventData(null);
            data.position = Input.mousePosition;
            List<RaycastResult> res = new List<RaycastResult>();
            EventSystem.current.RaycastAll(data, res);
            return res.Count != 0;
        }

        private bool _DoesScreenHasBeenResize()
        {
            return _screenSize.x != Screen.width || _screenSize.y != Screen.height;
        }

        private void _OnScreenIsResized()
        {
            _screenSize = new Vector2(Screen.width, Screen.height);

            topScreenScrollStart = (int)(Screen.height * 0.9f);
            bottomScreenScrollStart = (int)(Screen.height * 0.1f);
            leftScreenScrollStart = (int)(Screen.width * 0.1f);
            rightScreenScrollStart = (int)(Screen.width * 0.9f);
        }

        private IEnumerator Co_Focus(Vector3 to)
        {
            while (transform.position != to)
            {
                _rgbd.MovePosition(Vector3.Lerp(transform.position, to, 0.1f));
                yield return new WaitForEndOfFrame();
            }
        }
    }
}