using Data.Script.Prefab;
using Data.Structure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Data.Script {
  public class UIControl : MonoBehaviour {
    public GameObject Character;
    public GameObject HintPanel;
    public GameObject CharacterPanel;
    public Button[] buttons;
    public GameObject ItemList;
    public GameObject EnvList;
    public float DetectRadius;

    private GameObject _character;
    private HashSet<GameObject> _containingObjects;

    void Start() {
      _character = GameObject.FindGameObjectWithTag("Character"); // 单机
      _containingObjects = new HashSet<GameObject>();
    }


    void Update() {
      if (Input.GetKeyDown(KeyCode.Tab)) {
        RefreshItem(); // 逐帧更新会导致按钮无法交互
        RefreshEnv();
      }

      if (Input.GetKey(KeyCode.Tab)) {
        Global.isMenu = true;
        CharacterPanel.SetActive(true);
        HintPanel.SetActive(false);
      }
      else {
        Global.isMenu = false;
        CharacterPanel.SetActive(false);
        HintPanel.SetActive(true);
      }

      if (!Global.isMenu) {
        RefreshHint();
      }
      else {
        RefreshBody();
      }
    }

    private void RefreshHint() {
      if (_character.GetComponent<CharacterControl>().GetActiveGun() == null) {
        HintPanel.transform.Find("Weapon").gameObject.GetComponent<Text>().text = 
          "No Weapon in Slot " + _character.GetComponent<CharacterControl>().GetActiveSlot();
        HintPanel.transform.Find("Ammo").gameObject.GetComponent<Text>().text = "";
      }
      else {
        HintPanel.transform.Find("Weapon").gameObject.GetComponent<Text>().text = _character.GetComponent<CharacterControl>().GetActiveGun().GetName();
        Vector2Int ammo =
          _character.GetComponent<CharacterControl>().GetEquippedAmmo()[_character.GetComponent<CharacterControl>().GetActiveSlot() - 1];
        HintPanel.transform.Find("Ammo").gameObject.GetComponent<Text>().text = ammo.x + " / " + (ammo.y < 0 ? 0 : ammo.y);
      }
    }

    private void RefreshItem() {
      foreach (Transform child in ItemList.transform) {
        Destroy(child.gameObject);
      }
      var items = Character.GetComponent<CharacterControl>().GetItems();


      ItemList.GetComponent<RectTransform>().sizeDelta = new Vector2(250, 50 * items.Count + 20);
      foreach (var item in items) {
        GameObject itemButton = Instantiate(Resources.Load("Prefabs/ItemButton") as GameObject);
        itemButton.transform.Find("Text").gameObject.GetComponent<Text>().text = item.Key.GetName() + " - " + item.Value;
        switch (item.Key.GetType()) {
          case Type.AMMO:
            var ammo = Instantiate(Resources.Load("Prefabs/Ammo") as GameObject);
            ammo.transform.position = new Vector3(-100, -100, -100);
            ammo.GetComponent<AmmoControl>().Name = item.Key.GetName();
            ammo.GetComponent<AmmoControl>().Number = item.Value;
            ammo.GetComponent<AmmoControl>().Capacity = item.Key.GetCapacity();
            itemButton.GetComponent<ItemButtonControl>().SetContain(ammo, _character);
            itemButton.GetComponent<ItemButtonControl>().SetType(Type.AMMO);
            break;
        }
        itemButton.transform.SetParent(ItemList.transform, false);
      }
    }

    private void RefreshEnv() {
      _containingObjects.Clear();
      foreach (Collider collider in Physics.OverlapSphere(Character.transform.position, DetectRadius)) {
        if (collider.gameObject.tag == "Item") {
          _containingObjects.Add(collider.gameObject);
        }
      }

      foreach (Transform child in EnvList.transform) {
        Destroy(child.gameObject);
      }

      EnvList.GetComponent<RectTransform>().sizeDelta = new Vector2(250, 50 * _containingObjects.Count + 20);
      foreach (var gameObject in _containingObjects) {
        GameObject envItemButton = Instantiate(Resources.Load("Prefabs/EnvItemButton") as GameObject);
        envItemButton.GetComponent<ItemButtonControl>().SetContain(gameObject, _character);
        envItemButton.transform.Find("Text").gameObject.GetComponent<Text>().text = gameObject.GetComponent<ItemControl>().GetName();
        envItemButton.transform.SetParent(EnvList.transform, false);
      }
    }

    private void RefreshBody() {
      string[] bodyNames = Character.GetComponent<CharacterControl>().GetBodyNames();
      for (int i = 0; i < 8; i++) {
        buttons[i].transform.Find("Text").gameObject.GetComponent<Text>().text = bodyNames[i];
      }
    }
  }
}