using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShroomGuyRandomTexture : MonoBehaviour
{
    [SerializeField] Material[] materials;
    void Start()
    {
        GetComponent<SkinnedMeshRenderer>().material = materials[Random.Range(0, materials.Length)];
    }
}
