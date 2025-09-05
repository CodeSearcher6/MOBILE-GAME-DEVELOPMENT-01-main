using UnityEngine;
using Cinemachine;

public class Camera_Change : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera mainCam;
    [SerializeField] private CinemachineVirtualCamera topCam;
    [SerializeField] private CinemachineVirtualCamera sideCam;

    private void Start()
    {
        SwitchToMainCamera();
    }
        private void Update()
{
         if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchToMainCamera();
         if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchToTopCamera();
         if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchToSideCamera();
    }


    public void SwitchToMainCamera()
    {
        SetCameraPriority(mainCam);
    }

    public void SwitchToTopCamera()
    {
        SetCameraPriority(topCam);
    }

    public void SwitchToSideCamera()
    {
        SetCameraPriority(sideCam);
    }

    private void SetCameraPriority(CinemachineVirtualCamera activeCam)
    {
        mainCam.Priority = (activeCam == mainCam) ? 10 : 0;
        topCam.Priority = (activeCam == topCam) ? 10 : 0;
        sideCam.Priority = (activeCam == sideCam) ? 10 : 0;
    }
}
