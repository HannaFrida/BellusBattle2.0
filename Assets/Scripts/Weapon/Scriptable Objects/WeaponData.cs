using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Weapon/Gun")]
public class WeaponData : ScriptableObject
{
    
    [Header("Pooling")]
    private Queue<GameObject> spawnedObjs;
    public int amountToSpawn;
    private Transform parent;

    [Header("Info")]
    public new string name;

    [Header("Prefab")]
    public GameObject weaponPrefab;

    [Header("Shooting")]
    public float damage;
    public float maxDistance;
    //[SerializeField] public Transform muzzle;
    [SerializeField] public GameObject projectile;
    [SerializeField, Tooltip("The amount of force placed on the projectile.")]
    public float projectileForce;

    [Header("Reloading")]
    public int currentAmmo;
    public int initialAmmo;
    public int magSize;
    [Tooltip("In RPM")] public float fireRate;
    //public float reloadTime;
    //[HideInInspector] public bool reloading;

    [Header("Sounds")]
    [SerializeField, Tooltip("Sound made when picking up weapon")]
    public AudioSource pickupSound;
    [SerializeField, Tooltip("Sound made when using weapon")]
    public AudioSource shootAttackSound;

    [Header("VFX")]
    private int placeholder;

    // Getters
    public int Ammo { get => currentAmmo; }

    
    public Queue<GameObject> SpawnedObjs { get => spawnedObjs; }

    // Metods
    public void SpawnPool()
    {
        if (spawnedObjs == null || spawnedObjs.Count == 0)
        {
            spawnedObjs = new Queue<GameObject>();
        }

        if (spawnedObjs.Count >= amountToSpawn)
        {
            return;
        }

        if (!parent)
        {
            parent = new GameObject(name).transform;
        }

        while(spawnedObjs.Count < amountToSpawn)
        {
            GameObject obj = Instantiate(weaponPrefab, parent);
            obj.SetActive(false);
            spawnedObjs.Enqueue(obj);
        }
    }

    public GameObject GetPooledObject(Vector3 newPos, Quaternion newRot)
    {
        if (spawnedObjs == null || spawnedObjs.Count == 0)
        {
            SpawnPool();
            Debug.LogWarning($"{name} spawned mid-game. consider spawning it at the beginning of the game");
        }

        GameObject obj = spawnedObjs.Dequeue();
        spawnedObjs.Enqueue(obj);
        obj.SetActive(false);

        obj.transform.position = newPos;
        obj.transform.rotation = newRot;

        obj.SetActive(true);

        return obj;
    }
    
    


    /*
    public void ChangeAmmoBy(int changeBy)
    {
        currentAmmo += changeBy;
    }

    public void SetNewAmmoAmount(int newAmount)
    {
        currentAmmo = newAmount;
    }
    */

    public void ResetAmmo()
    {
        currentAmmo = initialAmmo;
    }
}
