using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DynamicCamera : MonoBehaviour
{
    //�͈͂̊p
    [SerializeField] Transform leftDownPoint;
    [SerializeField] Transform rightUpPoint;
    //�v���C���[
    [SerializeField] Transform focus;
    //�I�t�Z�b�g
    [SerializeField] Vector3 offset;
    //���݃J�����̊p
    Vector2 cameraLDPoint;
    Vector2 cameraURPoint;

    float originalCameraSize;

    Camera cam;
    //�O�̈ʒu
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
        //�Q�[���p�b�h
        if (Gamepad.current != null)
        {
            gamepad = Gamepad.current;
        }

        //�O�̈ʒu
        lastPosition = transform.position;
        transform.position = focus.position + offset;

        //�͈͂̊m�F
        CameraGetAngles();
        
        if (cameraLDPoint.x < leftDownPoint.position.x)
            transform.position = new Vector3(lastPosition.x, transform.position.y, transform.position.z);
        if (cameraLDPoint.y < leftDownPoint.position.y)
            transform.position = new Vector3(transform.position.x, lastPosition.y, transform.position.z);
        if (cameraURPoint.x > rightUpPoint.position.x)
            transform.position = new Vector3(lastPosition.x, transform.position.y, transform.position.z);
        if (cameraURPoint.y > rightUpPoint.position.y)
            transform.position = new Vector3(transform.position.x, lastPosition.y, transform.position.z);


        //�X���[�W���O
        if (gamepad.rightShoulder.isPressed)
        {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, 7.5f, 0.05f);
        }
        else
        {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, originalCameraSize, 0.05f);
        }
        
        //TODO:�@�T�C�Y��ς��鎞�ɁA�͈͂���o�Ȃ��悤��
    }

    //�J�����̋��̌v�Z
    void CameraGetAngles()
    {
        cameraLDPoint = (Vector2)cam.ScreenToWorldPoint(new Vector3(0, 0, cam.transform.position.z));
        cameraURPoint = (Vector2)cam.ScreenToWorldPoint(new Vector3(cam.scaledPixelWidth, cam.scaledPixelHeight, cam.transform.position.z));
    }
}
