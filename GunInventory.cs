using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum MenuStyle {
    horizontal, vertical
}

public class GunInventory : MonoBehaviour {
    [Tooltip("Current weapon gameObject.")]
    public GameObject currentGun;
    private Animator currentHandsAnimator;
    private int currentGunCounter = 0;

    [Tooltip("List of weapon objects.")]
    public List<GameObject> gunsIHave = new List<GameObject>();
    [Tooltip("Icons from weapons.")]
    public Texture[] icons;

    private List<GameObject> weaponObjects = new List<GameObject>();
    [HideInInspector]
    public float switchWeaponCooldown;

    void Awake() {
        StartCoroutine(UpdateIconsFromResources());
        SpawnWeapons(); // Preload all weapons
    }

    void SpawnWeapons() {
        foreach (GameObject weapon in gunsIHave) {
            if (weapon != null) {
                GameObject instantiatedWeapon = Instantiate(weapon, transform.position, Quaternion.identity);
                instantiatedWeapon.SetActive(false); // Initially deactivate all weapons
                weaponObjects.Add(instantiatedWeapon);
            }
        }
        if (weaponObjects.Count > 0) {
            ActivateWeapon(0);
        } else {
            Debug.Log("No guns in the inventory");
        }
    }

    void Update() {
        switchWeaponCooldown += Time.deltaTime;
        if (switchWeaponCooldown > 1.2f && !Input.GetKey(KeyCode.LeftShift)) {
            HandleWeaponSwitch();
        }
    }

    void HandleWeaponSwitch() {
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetAxis("Mouse ScrollWheel") > 0) {
            switchWeaponCooldown = 0;
            currentGunCounter = (currentGunCounter + 1) % weaponObjects.Count;
            ActivateWeapon(currentGunCounter);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetAxis("Mouse ScrollWheel") < 0) {
            switchWeaponCooldown = 0;
            currentGunCounter = (currentGunCounter - 1 + weaponObjects.Count) % weaponObjects.Count;
            ActivateWeapon(currentGunCounter);
        }
    }

    void ActivateWeapon(int index) {
        if (currentGun != null) {
            currentGun.SetActive(false);
        }
        currentGun = weaponObjects[index];
        currentGun.SetActive(true);
        AssignHandsAnimator(currentGun);
    }

    IEnumerator UpdateIconsFromResources() {
        yield return new WaitForEndOfFrame();
        icons = new Texture[gunsIHave.Count];
        for (int i = 0; i < gunsIHave.Count; i++) {
            icons[i] = (Texture)Resources.Load("Weap_Icons/" + gunsIHave[i].name + "_img");
        }
    }

    void AssignHandsAnimator(GameObject _currentGun) {
        GunScript gunScript = _currentGun.GetComponent<GunScript>();
        if (gunScript != null) {
            currentHandsAnimator = gunScript.handsAnimator;
        }
    }

    public void DeadMethod() {
        if (currentGun != null) {
            currentGun.SetActive(false);
        }
    }
}
