using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DynamicCamera : MonoBehaviour
{
    //範囲の角
    [SerializeField] Transform leftDownPoint;
    [SerializeField] Transform rightUpPoint;
    //プレイヤー
    [SerializeField] Transform focus;
    //オフセット
    [SerializeField] Vector3 offset;
    //現在カメラの角
    Vector2 cameraLDPoint;
    Vector2 cameraURPoint;

    float originalCameraSize;

    Camera cam;
    //前の位置
    Vector3 lastPosition;
    Gamepad gamepad;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        originalCameraSize = cam.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        //ゲームパッド
        if (Gamepad.current != null)
        {
            gamepad = Gamepad.current;
        }

        //前の位置
        lastPosition = transform.position;
        transform.position = focus.position + offset;

        //範囲の確認
        CameraGetAngles();
        
        if (cameraLDPoint.x < leftDownPoint.position.x)
            transform.position = new Vector3(lastPosition.x, transform.position.y, transform.position.z);
        if (cameraLDPoint.y < leftDownPoint.position.y)
            transform.position = new Vector3(transform.position.x, lastPosition.y, transform.position.z);
        if (cameraURPoint.x > rightUpPoint.position.x)
            transform.position = new Vector3(lastPosition.x, transform.position.y, transform.position.z);
        if (cameraURPoint.y > rightUpPoint.position.y)
            transform.position = new Vector3(transform.position.x, lastPosition.y, transform.position.z);


        //スムージング
        if (gamepad.rightShoulder.isPressed)
        {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, 7.5f, 0.05f);
        }
        else
        {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, originalCameraSize, 0.05f);
        }
        
        //TODO:　サイズを変える時に、範囲から出ないように
    }

    //カメラの隅の計算
    void CameraGetAngles()
    {
        cameraLDPoint = (Vector2)cam.ScreenToWorldPoint(new Vector3(0, 0, cam.transform.position.z));
        cameraURPoint = (Vector2)cam.ScreenToWorldPoint(new Vector3(cam.scaledPixelWidth, cam.scaledPixelHeight, cam.transform.position.z));
    }
}
