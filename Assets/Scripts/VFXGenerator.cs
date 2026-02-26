using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class VFXGenerator : MonoBehaviour
{
    public GameObject vfxPrefab_Bird;
    public GameObject vfxPrefab_Splash;
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

    public void GenerateVFXWithIndex(Vector3 position, Vector3 direction, int faceIndex, int x, int y, int z, VFXType vfxType)
    {
        string key = GenerateKey(faceIndex, x, y, z);

        GameObject vfx = null;

        switch (vfxType)
        {
            case VFXType.Bird:
                vfx = vfxPrefab_Bird;
                break;
            case VFXType.Splash:
                vfx = vfxPrefab_Splash;
                break;
        }

        if (vfx != null)
        {
            GetVFXParent();

            switch (vfxType)
            {
                case VFXType.Bird:
                    GameObject vfxInstance = Instantiate(vfxPrefab_Bird, position, Quaternion.LookRotation(direction));
                    vfxInstance.transform.SetParent(vfxParent.transform);
                    vfxInstances[key] = vfxInstance;
                    break;
                case VFXType.Splash:
                    vfxInstance = Instantiate(vfxPrefab_Splash, position, Quaternion.LookRotation(direction));
                    vfxInstance.transform.SetParent(vfxParent.transform);
                    vfxInstances[key] = vfxInstance;
                    break;
            }
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

    public void ClearVFXByType(VFXType vfxType)
    {
        List<string> keysToRemove = new List<string>();
        
        foreach (var kvp in vfxInstances)
        {
            if (kvp.Value != null)
            {
                if (IsVFXOfType(kvp.Value, vfxType))
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
                    keysToRemove.Add(kvp.Key);
                }
            }
        }

        foreach (string key in keysToRemove)
        {
            vfxInstances.Remove(key);
        }
    }

    private bool IsVFXOfType(GameObject vfxObject, VFXType vfxType)
    {
        string objectName = vfxObject.name.ToLower();
        
        switch (vfxType)
        {
            case VFXType.Bird:
                return objectName.Contains("bird");
            case VFXType.Splash:
                return objectName.Contains("splash");
            default:
                return false;
        }
    }
}

public enum VFXType
{
    Bird,
    Splash
}