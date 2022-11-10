using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMaterialHolder : MonoBehaviour
{
    [SerializeField] private List<GameObject> objMaterials = new List<GameObject>();
    [SerializeField] private Color newColor;
    // Start is called before the first frame update
    void Start()
    {
        objMaterials[0].GetComponent<MeshRenderer>().material.color = newColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
