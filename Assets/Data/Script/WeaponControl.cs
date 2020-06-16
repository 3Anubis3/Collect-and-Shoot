using Data.Structure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data.Script {
  public class WeaponControl : MonoBehaviour {
    public bool IsPrimary;

    private PrimaryGun _primaryGun;
    private SubGun _subGun;

    public string Name;
    public int BaseCapacity;
    public float BaseReloadTime, BaseDamage, BaseDiffusion, BaseRecoil, BaseSpeed;
    public string SuitableAmmoName;

    void Start() {
      if (!IsPrimary) {
        _subGun = new SubGun(Name, BaseCapacity, BaseReloadTime, SuitableAmmoName, BaseDamage, BaseDiffusion, BaseRecoil, BaseSpeed);
      }
      else {
        _primaryGun = new PrimaryGun(Name, BaseCapacity, BaseReloadTime, SuitableAmmoName, BaseDamage, BaseDiffusion, BaseRecoil, BaseSpeed);
      }
    }


    void Update() {

    }

    /*
    void OnCollisionEnter(Collision collision) {
      if (collision.gameObject.tag == "Character" && !(_primaryGun == null && _subGun == null)) {
        if (!IsPrimary) {
          if (collision.gameObject.GetComponent<CharacterControl>().AddGun(_subGun)) {
            collision.gameObject.GetComponent<ShootingControl>().CatchActiveGun();
            Destroy(gameObject);
          }
          return;
        }
        if (collision.gameObject.GetComponent<CharacterControl>().AddGun(_primaryGun)) {
          collision.gameObject.GetComponent<ShootingControl>().CatchActiveGun();
          Destroy(gameObject);
        }
      }
    }
    */

    public void Add(GameObject character) {
      if (!IsPrimary) {
        if (character.GetComponent<CharacterControl>().AddGun(_subGun)) {
          character.GetComponent<ShootingControl>().CatchActiveGun();
          Destroy(gameObject);
        }
        return;
      }
      if (character.GetComponent<CharacterControl>().AddGun(_primaryGun)) {
        character.GetComponent<ShootingControl>().CatchActiveGun();
        Destroy(gameObject);
      }
    }
  }
}
