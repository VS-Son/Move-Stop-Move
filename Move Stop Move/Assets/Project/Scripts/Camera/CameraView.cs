using Cinemachine;
using Project.Scripts.UI.Manager;
using UnityEngine;

namespace Project.Scripts.Camera
{
    public class CameraView : MonoBehaviour
    {
        public CinemachineVirtualCamera cinemaCamera;
        public float zoomSpeed = 5f;

        [Header("Camera Offset")] public Vector3 menuOffset = new(0, 1, -5);

        public Vector3 gameplayOffset = new(0, 7, -5);
        public Vector3 menuRotation = new(12f, 0, 0);
        public Vector3 gameplayRotation = new(50f, 0, 0);
        private Vector3 _targetOffset;
        private Vector3 _targetRotation;
        private CinemachineTransposer _transpose;

        private void Start()
        {
            _transpose = cinemaCamera.GetCinemachineComponent<CinemachineTransposer>();
        }

        private void FixedUpdate()
        {
            if (StateUI.Instance.IsState(StateType.MainMenu))
            {
                _transpose.m_FollowOffset = menuOffset;
                cinemaCamera.transform.eulerAngles = menuRotation;
            }
            else
            {
                _targetOffset = gameplayOffset;
                _targetRotation = gameplayRotation;

                _transpose.m_FollowOffset = Vector3.Lerp(
                    _transpose.m_FollowOffset,
                    _targetOffset,
                    Time.fixedDeltaTime * zoomSpeed
                );

                cinemaCamera.transform.eulerAngles = Vector3.Lerp(
                    cinemaCamera.transform.eulerAngles,
                    _targetRotation,
                    Time.fixedDeltaTime * zoomSpeed
                );
            }
        }
    }
}