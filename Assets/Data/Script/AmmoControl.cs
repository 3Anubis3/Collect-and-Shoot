using Data.Structure;
using UnityEngine;

namespace Data.Script {
  public class AmmoControl : MonoBehaviour {
    private Ammo _ammo;
    public string Name;
    public int Number;
    public float Capacity;

    void Start() {
      _ammo = new Ammo(Name, Capacity);
    }


    void Update() {

    }

    /*
    void OnCollisionEnter(Collision collision) {
      if (collision.gameObject.tag == "Character") {
        if (collision.gameObject.GetComponent<CharacterControl>().AddAmmo(_ammo, Number)) {
          Destroy(gameObject);
        }
      }
    }
    */

    public void Add(GameObject character) {
      if (character.GetComponent<CharacterControl>().AddAmmo(_ammo, Number)) {
        Destroy(gameObject);
      }
    }
  }
}