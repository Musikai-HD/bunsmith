using Unity.VisualScripting;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    Camera mainCam;
    [Range(0f, 1f)] public float playerMouseRatio;
    public float camSpeed;

    void Awake()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        Vector3 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 targetPlayer = GameManager.instance.pc ? GameManager.instance.pc.transform.position : Vector3.zero; 
        Vector3 targetPos = (targetPlayer * (1f-playerMouseRatio)) + (mousePos * playerMouseRatio);
        mainCam.transform.position = Vector3.Slerp(mainCam.transform.position, new Vector3(targetPos.x, targetPos.y, mainCam.transform.position.z), camSpeed * Time.deltaTime);
    }
}
