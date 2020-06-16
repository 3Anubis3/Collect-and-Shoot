using Data.Structure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data.Script {
  public class BagControl : MonoBehaviour {
    public string Name;
    public float BagCapacity;

    private Bag _bag;

    void Start() {
      _bag = new Bag(Name, BagCapacity);
    }

    void Update() {

    }

    /*
    void OnCollisionEnter(Collision collision) {
      if (collision.gameObject.tag == "Character" && _bag != null) {
        if (collision.gameObject.GetComponent<CharacterControl>().AddBag(_bag)) {
          Destroy(gameObject);
        }
      }
    }
    */

    public void Add(GameObject character) {
      if (character.GetComponent<CharacterControl>().AddBag(_bag)) {
        Destroy(gameObject);
      }
    }
  }
}