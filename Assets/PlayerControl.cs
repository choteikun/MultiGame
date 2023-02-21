using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerControl : MonoBehaviour
{
    [Tooltip("角色相機")]
    public Transform charCamTransform;
    [Tooltip("虛擬相機")]
    public CinemachineVirtualCamera virtualCam;
    [Tooltip("角色控制器")]
    public CharacterController characterController;

    [Tooltip("角色移動速度")]
    public float moveSpeed;

    CinemachineFramingTransposer framingTransposer;

    void Start()
    {
        charCamTransform = Camera.main.transform;
        characterController = GetComponent<CharacterController>();
        framingTransposer = virtualCam.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    void Update()
    {
        CharacterRotate();
        CharacterMovement();
    }

    void CharacterMovement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        if(Mathf.Abs(x)>0.1f|| Mathf.Abs(z) > 0.1f)
        {
            //移動方向
            Vector3 towardDir = new Vector3(x, 0, z);
            characterController.SimpleMove(towardDir.normalized * moveSpeed);
        }
    }
    void CharacterRotate()
    {
        Vector3 charTargetPos = Vector3.zero;
        

        //生成從相機發射的射線，該射線穿過當前鼠標位置
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //創建一個平面，第一個參數和第二個參數構成法線，且平面穿過第二個參數點
        Plane hitPlane = new Plane(Vector3.up, transform.position);

        //平面與射線相交返回發射點到相交點的距離
        float rayDistance;

        //Raycast計算相交點並返回距離
        if (hitPlane.Raycast(ray, out rayDistance))
        {
            //射線與平面的相交點
            charTargetPos = ray.GetPoint(rayDistance);

            Debug.DrawLine(ray.origin, charTargetPos, Color.red);
        }
        //角色朝向(鼠標預期方向)
        Vector3 heightCorrectedPoint = new Vector3(charTargetPos.x, transform.position.y, charTargetPos.z);
        transform.LookAt(heightCorrectedPoint);
    }
}
