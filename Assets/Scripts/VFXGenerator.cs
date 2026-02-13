using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class VFXGenerator : MonoBehaviour
{
    public GameObject vfxPrefab;
    private Dictionary<string, GameObject> vfxInstances = new Dictionary<string, GameObject>();
    private GameObject vfxParent;

    public void Init()
    {
        //销毁所有vfxInstances
        foreach (var kvp in vfxInstances)
        {
            if (kvp.Value != null)
            {
                Destroy(kvp.Value);
            }
        }
        vfxInstances.Clear();
    }

    public void GenerateVFXWithIndex(Vector3 position, Vector3 direction, int faceIndex, int x, int y, int z)
    {
        string key = GenerateKey(faceIndex, x, y, z);
        
        if (vfxPrefab != null)
        {
            // Get or create VFX parent
            GetVFXParent();
            
            GameObject vfxInstance = Instantiate(vfxPrefab, position, Quaternion.LookRotation(direction));
            vfxInstance.transform.SetParent(vfxParent.transform);
            vfxInstances[key] = vfxInstance;
        }
    }

    public void DestroyVFXByIndex(int faceIndex, int x, int y, int z)
    {
        string key = GenerateKey(faceIndex, x, y, z);

        if (vfxInstances.TryGetValue(key, out GameObject vfxInstance))
        {
            if (vfxInstance != null)
            {
                Destroy(vfxInstance);
                vfxInstances.Remove(key);
            }
            else
            {
                vfxInstances.Remove(key);
            }
        }
    }

    private string GenerateKey(int faceIndex, int x, int y, int z)
    {
        return $"{faceIndex}_{x}_{y}_{z}";
    }

    private void GetVFXParent()
    {
        if (vfxParent == null)
        {
            vfxParent = GameObject.Find("VFX");
            if (vfxParent == null)
            {
                vfxParent = new GameObject("VFX");
            }
        }
    }

    public void ClearAllVFX()
    {
        // Clear from dictionary
        foreach (var kvp in vfxInstances)
        {
            if (kvp.Value != null)
            {
#if UNITY_EDITOR
                if (Application.isEditor && !Application.isPlaying)
                {
                    DestroyImmediate(kvp.Value);
                }
                else
#endif
                {
                    Destroy(kvp.Value);
                }
            }
        }
        vfxInstances.Clear();

        GetVFXParent();
        foreach (Transform child in vfxParent.transform)
        {
#if UNITY_EDITOR
            if (Application.isEditor && !Application.isPlaying)
            {
                DestroyImmediate(child.gameObject);
            }
            else
#endif
            {
                Destroy(child.gameObject);
            }
        }
    }
}