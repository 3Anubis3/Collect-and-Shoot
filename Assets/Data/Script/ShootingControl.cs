using Data.Structure;
using System.Collections;
using UnityEngine;
using Random = System.Random;


namespace Data.Script {
  public class ShootingControl : MonoBehaviour {
    private Random random;

    private Character _character;
    private Gun _activeGun;

    private bool _isReady;

    void Start() {
      random = new Random();

      _character = gameObject.GetComponent<CharacterControl>().GetCharacter();
      CatchActiveGun();
      _isReady = true;
    }


    void Update() {
      if (!Global.isMenu) {
        if (Input.GetMouseButton(0)) { // 射击
          if (_activeGun != null && _isReady) {
            if (_activeGun.Shot()) {
              Vector2 aimingVector = new Vector2(Screen.width / 2, Screen.height / 2);
              gameObject.GetComponent<CameraControl>().Translate(new Vector2(_activeGun.GetBaseRecoil() * (float) random.NextDouble(),
                _activeGun.GetBaseRecoil() * (0.5f - (float) random.NextDouble())));
              if (Physics.Raycast(gameObject.GetComponent<CameraControl>().GetActiveCamera().ScreenPointToRay(aimingVector), out var hit)) {
                if (hit.collider.gameObject.tag == "Character") {
                  hit.collider.gameObject.GetComponent<CharacterControl>().Shot(_activeGun.GetBaseDamage());
                }
              }

              _isReady = false;
              StartCoroutine(Recoil());
            }
          }
        }

        if (Input.GetKeyDown(KeyCode.R)) { // 换弹
          if (_activeGun != null && _isReady) {
            StartCoroutine(Prepare());
          }
        }
      }
    }

    private Gun GetActiveGun() {
      int weaponSlot = _character.GetActiveSlot();
      Gun gun;
      switch (weaponSlot) {
        case 1:
          gun = _character.GetPrimaryGun(1);
          break;
        case 2:
          gun = _character.GetPrimaryGun(2);
          break;
        case 3:
          gun = _character.GetSubGun();
          break;
        default:
          gun = null;
          break;
      }

      return gun;
    }

    public void CatchActiveGun() {
      // _isReady = false;
      _activeGun = GetActiveGun();
    }

    public bool IsReady() {
      return _isReady;
    }

    private IEnumerator Prepare() { // 切换枪械、换弹时的延时
      if (_activeGun == null) {
        yield break;
      }
      Vector2Int curAmmo = _character.GetEquippedAmmo()[_character.GetActiveSlot() - 1];
      yield return new WaitForSeconds(_activeGun.GetBaseReloadTime());
      if (curAmmo.y > 0 && curAmmo.x < _activeGun.GetBaseCapacity()) { // 如果需要上弹
        _isReady = false;

        if (_activeGun.GetBaseCapacity() - curAmmo.x < curAmmo.y) { // 能补满
          _activeGun.SetRemainCapacity(_activeGun.GetBaseCapacity()); // 就补满
          _character.RemoveAmmoByName(_activeGun.GetSuitableAmmoName(), _activeGun.GetBaseCapacity() - curAmmo.x);
        }
        else { // 补不满
          _activeGun.SetRemainCapacity(curAmmo.x + curAmmo.y); // 全补上
          _character.RemoveAmmoByName(_activeGun.GetSuitableAmmoName(), curAmmo.y);
        }
      }

      _isReady = true;
    }

    private IEnumerator Recoil() { // 发射间隔的枪机回弹延时
      yield return new WaitForSeconds(_activeGun.GetBaseRecoilTime());
      _isReady = true;
    }
  }
}