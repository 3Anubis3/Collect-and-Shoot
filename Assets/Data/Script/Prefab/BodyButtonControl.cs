using Data.Structure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data.Script.Prefab {
  public class BodyButtonControl : MonoBehaviour {
    public GameObject Character;

    private CharacterControl script;

    void Start() {
      script = Character.GetComponent<CharacterControl>();
    }

    public void RemovePrimary(int slot) {
      script.RemoveGun(slot);
    }

    public void RemoveSub() {
      script.RemoveGun(3);
    }

    public void RemoveThrowable() {
      script.RemoveThrowable();
    }

    public void RemoveClose() {
      script.RemoveClose();
    }

    public void RemoveHelmet() {
      script.RemoveHelmet();
    }

    public void RemoveArmor() {
      script.RemoveArmor();
    }

    public void RemoveBag() {
      script.RemoveBag();
    }
  }
}