using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using UnityEngine.Events;

public class AfterImg : MonoBehaviour
{
    [SerializeReference] private float activeTime = 0.5f;
    [Header ("Mesh related")]
    [SerializeReference] private float meshRefreshRate = 0.1f;
    private bool isTrailActive;
    private SkinnedMeshRenderer[] skinnedMeshRenderers;
    public Transform positionToSpawn;
    [SerializeReference] private float destroyDelay = 0.4f;
    [Header("shader related")]
    public Material mat;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void StartTrail()
    {
        if (!isTrailActive)
        {
            isTrailActive = true;
            StartCoroutine(ActivateTrail(activeTime));
        }
  
    }
    IEnumerator ActivateTrail (float time)
    {
        while(time > 0)
        {
            time -= meshRefreshRate;

            if (skinnedMeshRenderers == null)
            {
                skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
            }
                for(int i=0; i<skinnedMeshRenderers.Length; i++)
                {
                    GameObject meshes = new GameObject();
                    meshes.transform.SetPositionAndRotation(positionToSpawn.position, positionToSpawn.rotation);
                    MeshRenderer mr = meshes.AddComponent<MeshRenderer>();
                    MeshFilter mf = meshes.AddComponent<MeshFilter>();

                    Mesh mesh = new Mesh();
                    skinnedMeshRenderers[i].BakeMesh(mesh);

                    mf.mesh = mesh;
                    mr.material = mat;
                    Destroy(mesh, destroyDelay);
                }
            



            yield return new WaitForSeconds(meshRefreshRate);
        }
        isTrailActive = false;
    }
}
