using Data.Structure;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Data.Script {
  public class CameraControl : MonoBehaviour {
    public float RotateSpeed, MinRotateValue, MaxRotateValue; // 旋转相关参数
    public float DefaultZoomValue, ZoomSpeed, MinZoomValue, MaxZoomValue; // 视角缩放相关参数

    public Camera TpsCamera; // 第三人称视角摄像机
    public Vector3 TpsCameraPosition; // 摄像机初始位置
    public Vector3 TpsCameraQuaternion; // 摄像机初始角度
    public Camera AimedCamera; // 瞄准视角摄像机
    public Vector3 AimedCameraPosition; // 摄像机初始位置
    public Vector3 AimedCameraQuaternion; // 摄像机初始角度

    public GameObject UIAiming; // 准星

    private float _zoomValue;
    private bool _isAimed;

    void Start() {
      _zoomValue = DefaultZoomValue;
    }


    void Update() {
      if (!Global.isMenu) {
        // 人物+摄像机旋转
        var mouseX = Input.GetAxis("Mouse X"); // 获取鼠标X轴移动
        transform.RotateAround(transform.position, Vector3.up, mouseX * RotateSpeed);

        // 限制俯角
        var mouseY = -Input.GetAxis("Mouse Y"); // 获取鼠标Y轴移动
        if (_isAimed) {
          // 瞄准镜头
          Transform aimedTransform = AimedCamera.transform;
          if (aimedTransform.localEulerAngles.x > 180 && aimedTransform.localEulerAngles.x - 360 < MinRotateValue) {
            aimedTransform.RotateAround(transform.position, transform.right,
              MinRotateValue - aimedTransform.localEulerAngles.x + 360); // 陀螺BUG
          }
          else if (aimedTransform.localEulerAngles.x < 180 && aimedTransform.localEulerAngles.x > MaxRotateValue) {
            aimedTransform.RotateAround(transform.position, transform.right, MaxRotateValue - aimedTransform.localEulerAngles.x); // 陀螺BUG
          }
          else {
            aimedTransform.RotateAround(transform.position, transform.right, mouseY * RotateSpeed);
          }
        }
        else {
          // 全局镜头
          Transform tpsTransform = TpsCamera.transform;
          if (tpsTransform.localEulerAngles.x > 180 && tpsTransform.localEulerAngles.x - 360 < MinRotateValue) {
            tpsTransform.RotateAround(transform.position, transform.right, MinRotateValue - tpsTransform.localEulerAngles.x + 360);
          }
          else if (tpsTransform.localEulerAngles.x < 180 && tpsTransform.localEulerAngles.x > MaxRotateValue) {
            tpsTransform.RotateAround(transform.position, transform.right, MaxRotateValue - tpsTransform.localEulerAngles.x);
          }
          else {
            tpsTransform.RotateAround(transform.position, transform.right, mouseY * RotateSpeed);
          }
        }

        // 缩放视角
        if (Input.GetAxis("Mouse ScrollWheel") < 0) { // 放大
          if (_zoomValue < MaxZoomValue) {
            _zoomValue += ZoomSpeed;
          }
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0) { // 缩小
          if (_zoomValue > MinZoomValue) {
            _zoomValue -= ZoomSpeed;
          }
        }

        Vector3 direction = TpsCamera.transform.localPosition.normalized; // 获取方向向量（摄像机 -> 角色）并单位化
        TpsCamera.transform.localPosition = direction * _zoomValue;

        // 避障
        RaycastHit[] hits = Physics.RaycastAll(new Ray(transform.position, TpsCamera.transform.position - transform.position));
        if (hits.Length > 0) {
          RaycastHit stand = hits[0];
          foreach (RaycastHit hit in hits) {
            if (hit.collider.gameObject.tag == "Obstruction") {
              if (hit.distance < stand.distance) {
                stand = hit;
              }
            }
          }


          TpsCamera.transform.localPosition = direction * (stand.distance < _zoomValue ? stand.distance : _zoomValue);
        }

        // 瞄准模式切换
        if (Input.GetMouseButton(1)) {
          if (!_isAimed) {
            TpsCamera.gameObject.SetActive(false);
            AimedCamera.gameObject.SetActive(true);
            TpsCamera.transform.localEulerAngles = TpsCameraQuaternion;
            TpsCamera.transform.localPosition = TpsCameraPosition;
            AimedCamera.transform.localEulerAngles = AimedCameraQuaternion;
            AimedCamera.transform.localPosition = AimedCameraPosition;
            _isAimed = true;
            UIAiming.SetActive(true);
          }
        }
        else {
          if (_isAimed) {
            TpsCamera.gameObject.SetActive(true);
            AimedCamera.gameObject.SetActive(false);
            TpsCamera.transform.localEulerAngles = TpsCameraQuaternion;
            TpsCamera.transform.localPosition = TpsCameraPosition;
            AimedCamera.transform.localEulerAngles = AimedCameraQuaternion;
            AimedCamera.transform.localPosition = AimedCameraPosition;
            _isAimed = false;
            UIAiming.SetActive(false);
          }
        }
      }
    }

    public Camera GetActiveCamera() {
      if (TpsCamera.gameObject.activeSelf) {
        return TpsCamera;
      }
      return AimedCamera;
    }

    public void Translate(Vector2 offset) {
      GetActiveCamera().transform.RotateAround(transform.position, transform.right, -offset.x); // 上下抖动
      GetActiveCamera().transform.RotateAround(transform.position, transform.up, -offset.y); // 左右抖动

    }
  }
}