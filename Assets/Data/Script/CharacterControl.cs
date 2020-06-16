using Data.Structure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data.Script {
  public class CharacterControl : MonoBehaviour {
    public float MaxHp;
    public float BaseCapacity;

    private Character _character;

    void Start() {
      _character = new Character(BaseCapacity, MaxHp);
    }


    void Update() {
      // 判断是否存活
      if (!_character.IsAlive()) {
        Destroy(gameObject);
      }

      // 切换武器
      if (Input.GetKeyDown(KeyCode.Alpha1)) {
        _character.SetActiveSlot(1);
        gameObject.GetComponent<ShootingControl>().CatchActiveGun();
      }
      else if (Input.GetKeyDown(KeyCode.Alpha2)) {
        _character.SetActiveSlot(2);
        gameObject.GetComponent<ShootingControl>().CatchActiveGun();
      }
      else if (Input.GetKeyDown(KeyCode.Alpha3)) {
        _character.SetActiveSlot(3);
        gameObject.GetComponent<ShootingControl>().CatchActiveGun();
      }
    }

    public Character GetCharacter() {
      return _character;
    }

    public bool AddAmmo(Ammo ammo, int number) {
      return _character.AddAmmo(ammo, number);
    }

    public bool AddGun(PrimaryGun primaryGun) {
      if (_character.GetPrimaryGun(1) == null) {
        _character.SetPrimaryGun(1, primaryGun);
      } else if (_character.GetPrimaryGun(1).GetName() != primaryGun.GetName()) {
        if (_character.GetPrimaryGun(2) == null) {
          _character.SetPrimaryGun(2, primaryGun);
        } 
        else if(_character.GetPrimaryGun(2).GetName() != primaryGun.GetName()) {
          return false;
        }
      }
      return true;
    }

    public bool AddGun(SubGun subGun) {
      return _character.SetSubGun(subGun);
    }

    public Vector2Int[] GetEquippedAmmo() {
      return _character.GetEquippedAmmo();
    }

    public int GetActiveSlot() {
      return _character.GetActiveSlot();
    }

    public Camera GetCurCamera() {
      return gameObject.GetComponent<CameraControl>().GetActiveCamera();
    }

    public Gun GetActiveGun() {
      return _character.GetActiveGun();
    }

    public void RemoveAmmoByName(string name, int removesNumber) {
      _character.RemoveAmmoByName(name, removesNumber);
    }

    public bool AddBag(Bag bag) {
      return _character.AddBag(bag);
    }

    public string[] GetBodyNames() {
      return _character.GetBodySlotStrings();
    }

    public void RemoveGun(int slot) {
      if (slot == 3) {
        _character.SetSubGun(null);
        return;
      }
      _character.SetPrimaryGun(slot, null);
    }

    public void RemoveThrowable() {
      _character.SetThrowable(null);
    }

    public void RemoveClose() {
      _character.SetClose(null);
    }

    public bool Shot(float damage) {
      return _character.Shot(damage);
    }

    public Dictionary<Item, int> GetItems() {
      return _character.GetItems();
    }

    public void RemoveHelmet() {
      _character.SetHelmet(null);
    }

    public void RemoveArmor() {
      _character.SetArmor(null);
    }

    public void RemoveBag() {
      _character.SetBag(null);
    }
  }
}