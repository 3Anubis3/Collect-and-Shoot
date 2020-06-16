using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

namespace Data.Script.Prefab {
  public class ItemButtonControl : MonoBehaviour {
    private GameObject _uiPanel;
    private GameObject _character;
    private GameObject _contain;
    private Type _type;

    void Start() {
      _uiPanel = GameObject.Find("Shooting UI");
    }

    private void SetCharacter(GameObject character) {
      _character = character;
    }

    public void SetContain(GameObject contain, GameObject character) {
      _contain = contain;
      SetCharacter(character);
    }

    public void SetType(Type type) {
      _type = type;
    }

    public void PutIn() {
      switch (_contain.GetComponent<ItemControl>().GetType()) {
        case Type.WEAPON:
          _contain.GetComponent<WeaponControl>().Add(_character);
          break;
        case Type.AMMO:
          _contain.GetComponent<AmmoControl>().Add(_character);
          break;
        case Type.BAG:
          _contain.GetComponent<BagControl>().Add(_character);
          break;
      }

      _uiPanel.SendMessage("RefreshEnv");
      _uiPanel.SendMessage("RefreshItem");
    }

    public void DropOut() {
      switch (_type) {
        case Type.AMMO:
          GameObject ammo = Instantiate(Resources.Load("Prefabs/Ammo") as GameObject);
          var ammoScript = ammo.GetComponent<AmmoControl>();
          ammoScript.Name = _contain.GetComponent<AmmoControl>().Name;
          ammoScript.Number = _contain.GetComponent<AmmoControl>().Number;
          ammoScript.Capacity = _contain.GetComponent<AmmoControl>().Capacity;
          ammoScript.tag = "Item";
          ammo.AddComponent<Rigidbody>();
          ammo.transform.position = _character.transform.position + new Vector3(1, 2, 0);
          Destroy(_contain);
          ammo.transform.parent = GameObject.FindGameObjectWithTag("Environment").transform;

          _character.GetComponent<CharacterControl>().RemoveAmmoByName(ammoScript.Name, ammoScript.Number);
          break;
      }

      _uiPanel.SendMessage("RefreshEnv");
      _uiPanel.SendMessage("RefreshItem");
    }
  }
}