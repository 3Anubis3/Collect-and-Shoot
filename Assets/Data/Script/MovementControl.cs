using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data.Script {
  public class MovementControl : MonoBehaviour {
    public float MoveSpeed; // 移动速度
    public float JumpPower; // 跳跃力
    public float H_Resistance, V_Resistance; // 地面阻尼、空气阻力

    private bool _isGround;
    private Rigidbody _selfRigidbody;
    private Vector3 _totalMoveVector, _totalJumpVector;

    void Start() {
      _isGround = false;
      _selfRigidbody = GetComponent<Rigidbody>();
    }

    void Update() {
      // 锁定X、Z两轴
      float rotY = transform.localEulerAngles.y;
      if (!(transform.rotation.x == 0.0f && transform.rotation.z == 0.0f)) {
        transform.localEulerAngles = new Vector3(0, rotY, 0);
      }

      if (_isGround) {
        double rot = transform.localEulerAngles.y * Math.PI / 180;

        // 前后左右移动
        if (Input.GetKey(KeyCode.W)) {
          _totalMoveVector += new Vector3(MoveSpeed * (float) Math.Sin(rot), 0, MoveSpeed * (float) Math.Cos(rot));
        }

        if (Input.GetKey(KeyCode.A)) {
          _totalMoveVector += new Vector3(-MoveSpeed * (float) Math.Cos(rot), 0, MoveSpeed * (float) Math.Sin(rot));
        }

        if (Input.GetKey(KeyCode.S)) {
          _totalMoveVector += new Vector3(-MoveSpeed * (float) Math.Sin(rot), 0, -MoveSpeed * (float) Math.Cos(rot));
        }

        if (Input.GetKey(KeyCode.D)) {
          _totalMoveVector += new Vector3(MoveSpeed * (float) Math.Cos(rot), 0, -MoveSpeed * (float) Math.Sin(rot));
        }

        // 跳跃
        if (Input.GetKey(KeyCode.Space)) {
          _isGround = false;
          _totalJumpVector += new Vector3(0, JumpPower, 0);
        }

        _totalMoveVector /= 1 + H_Resistance;
        _totalJumpVector /= 1 + V_Resistance;
        _selfRigidbody.velocity = _totalMoveVector * (Input.GetKey(KeyCode.LeftShift) ? 2 : 1) +
                                  _totalJumpVector * (Input.GetKey(KeyCode.LeftShift) ? 1.5f : 1); // 按住左Shift键移动2倍、跳跃1.5倍
      }

      // 蹲伏
      if (Input.GetKey(KeyCode.LeftControl)) {
        transform.localScale = new Vector3(1, 0.6f, 1);
      }

      if (Input.GetKeyUp(KeyCode.LeftControl)) {
        transform.localScale = new Vector3(1, 1, 1);
      }
    }

    void OnCollisionStay(Collision collision) {
      _isGround = true;
    }

    void OnCollisionExit(Collision collision) {
      _isGround = false;
    }
  }
}