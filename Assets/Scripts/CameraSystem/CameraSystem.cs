using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

    public class CameraSystem : MonoBehaviour {

        [SerializeField] private CinemachineCamera _cinemachineCamera;
        [SerializeField] private bool _useEdgeScrolling = false;
        [SerializeField] private bool _useDragPan = false;

        [SerializeField] private float _followOffsetMinY = 10f;
        [SerializeField] private float _followOffsetMaxY = 50f;

        private bool _dragPanMoveActive;
        private Vector2 _lastMousePosition;

        private Vector3 _followOffset;
        private CinemachineFollow _cameraFollow;

        private void Awake() {
            _cameraFollow = _cinemachineCamera.GetComponent<CinemachineFollow>();
            _followOffset = _cameraFollow.FollowOffset;
        }

        private void Update() {
            HandleCameraMovement();

            if (_useEdgeScrolling) {
                HandleCameraMovementEdgeScrolling();
            }

            if (_useDragPan) {
                HandleCameraMovementDragPan();
            }

            HandleCameraRotation();

            HandleCameraZoom_LowerY();
        }

        private void HandleCameraMovement() {
            Vector3 inputDir = new Vector3(0, 0, 0);

            if (Input.GetKey(KeyCode.W)) inputDir.z = +1f;
            if (Input.GetKey(KeyCode.S)) inputDir.z = -1f;
            if (Input.GetKey(KeyCode.A)) inputDir.x = -1f;
            if (Input.GetKey(KeyCode.D)) inputDir.x = +1f;

            Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;

            float moveSpeed = 50f;
            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }

        private void HandleCameraMovementEdgeScrolling() {
            Vector3 inputDir = new Vector3(0, 0, 0);

            int edgeScrollSize = 20;

            if (Input.mousePosition.x < edgeScrollSize) {
                inputDir.x = -1f;
            }
            if (Input.mousePosition.y < edgeScrollSize) {
                inputDir.z = -1f;
            }
            if (Input.mousePosition.x > Screen.width - edgeScrollSize) {
                inputDir.x = +1f;
            }
            if (Input.mousePosition.y > Screen.height - edgeScrollSize) {
                inputDir.z = +1f;
            }

            Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;

            float moveSpeed = 50f;
            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }

        private void HandleCameraMovementDragPan() {
            Vector3 inputDir = new Vector3(0, 0, 0);

            if (Input.GetMouseButtonDown(1)) {
                _dragPanMoveActive = true;
                _lastMousePosition = Input.mousePosition;
            }
            if (Input.GetMouseButtonUp(1)) {
                _dragPanMoveActive = false;
            }

            if (_dragPanMoveActive) {
                Vector2 mouseMovementDelta = (Vector2)Input.mousePosition - _lastMousePosition;

                float dragPanSpeed = 1f;
                inputDir.x = mouseMovementDelta.x * dragPanSpeed;
                inputDir.z = mouseMovementDelta.y * dragPanSpeed;

                _lastMousePosition = Input.mousePosition;
            }

            Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;

            float moveSpeed = 50f;
            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }

        private void HandleCameraRotation() {
            float rotateDir = 0f;
            if (Input.GetKey(KeyCode.Q)) rotateDir = +1f;
            if (Input.GetKey(KeyCode.E)) rotateDir = -1f;

            float rotateSpeed = 100f;
            transform.eulerAngles += new Vector3(0, rotateDir * rotateSpeed * Time.deltaTime, 0);
        }
        private void HandleCameraZoom_LowerY() {
            float zoomAmount = 3f;
            if (Input.mouseScrollDelta.y > 0) {
                _followOffset.y -= zoomAmount;
            }
            if (Input.mouseScrollDelta.y < 0) {
                _followOffset.y += zoomAmount;
            }

            _followOffset.y = Mathf.Clamp(_followOffset.y, _followOffsetMinY, _followOffsetMaxY);

            float zoomSpeed = 10f;
            _cameraFollow.FollowOffset = Vector3.Lerp(_cameraFollow.FollowOffset, _followOffset, Time.deltaTime * zoomSpeed);

        }

    }