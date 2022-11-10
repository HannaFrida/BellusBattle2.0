using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    [SerializeField]private Gun[] weapons;
    private WeaponSpawnerManager manager;

    void Start()
    {
        manager = gameObject.GetComponentInParent<WeaponSpawnerManager>();
    }
    public void SpawnRandomWeapon()
    {
        //if (isholdingWeapon) return;
        Instantiate(weapons[Random.Range(0, weapons.Length)], transform);
    }
    public void WeaponIsPicked() // Den här ska försättas med PickUp Script
    {
        manager.AddEmptySpawnerToChooseFrom(this);
    }

}
