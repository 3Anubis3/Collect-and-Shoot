using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data.Script {
  public class ItemControl : MonoBehaviour {
    private string _name;
    private Type _type;

    void Start() {
      if (gameObject.GetComponent<WeaponControl>() != null) { // 枪支
        _type = Type.WEAPON;
        _name = gameObject.GetComponent<WeaponControl>().Name;
      }
      else if (gameObject.GetComponent<BagControl>() != null) { // 背包
        _type = Type.BAG;
        _name = gameObject.GetComponent<BagControl>().Name;
      }
      else if (gameObject.GetComponent<AmmoControl>() != null) { // 弹药
        _type = Type.AMMO;
        _name = gameObject.GetComponent<AmmoControl>().Name;
      }
    }

    public new Type GetType() {
      return _type;
    }

    public string GetName() {
      return _name;
    }
  }
}