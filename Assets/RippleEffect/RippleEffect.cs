using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

public class RippleEffect : MonoBehaviour
{
    public AnimationCurve waveform = new AnimationCurve(
        new Keyframe(0.00f, 0.50f, 0, 0),
        new Keyframe(0.05f, 1.00f, 0, 0),
        new Keyframe(0.15f, 0.10f, 0, 0),
        new Keyframe(0.25f, 0.80f, 0, 0),
        new Keyframe(0.35f, 0.30f, 0, 0),
        new Keyframe(0.45f, 0.60f, 0, 0),
        new Keyframe(0.55f, 0.40f, 0, 0),
        new Keyframe(0.65f, 0.55f, 0, 0),
        new Keyframe(0.75f, 0.46f, 0, 0),
        new Keyframe(0.85f, 0.52f, 0, 0),
        new Keyframe(0.99f, 0.50f, 0, 0)
    );

    [Range(0.01f, 1.0f)]
    public float refractionStrength = 0.5f;

    public Color reflectionColor = Color.gray;

    [Range(0.01f, 1.0f)]
    public float reflectionStrength = 0.7f;

    [Range(1.0f, 3.0f)]
    public float waveSpeed = 1.25f;

    [Range(0.01f, 1.0f)]
    public float radius = 0.5f;

    [SerializeField]
    Shader shader;
    public GameObject targetObject;
    private MeshFilter _meshFilter;
    public bool activation = false;

    class Droplet
    {
        Vector2 position;
        float time;

        MeshFilter _meshFilter;
        GameObject _targetObject;

        public Droplet(MeshFilter meshFilter, GameObject targetObject)
        {
            time = 1000;
            _meshFilter = meshFilter;
            _targetObject = targetObject;
        }

        public void Reset()
        {
            time = 0;
        }

        public void Update()
        {
            time += Time.deltaTime;
        }

        public Vector4 MakeShaderParameter(float aspect)
        {
            var _position = CalculateScreenUV(_meshFilter);
            return new Vector4(_position.x * aspect, _position.y, time, 0);
        }

        Vector2 CalculateScreenUV(MeshFilter meshFilter)
        {
            // 计算Mesh的包围盒中心（世界坐标）
            Vector3 meshCenter = meshFilter.sharedMesh.bounds.center;

            Vector3 worldCenter = _targetObject.transform.TransformPoint(meshCenter);

            // 将世界坐标转换为视口坐标(0-1范围)
            Camera mainCamera = Camera.main;
            if (mainCamera == null)
            {
                return new Vector2(0.5f, 0.5f);
            }

            Vector3 viewportPoint = mainCamera.WorldToViewportPoint(worldCenter);
            Vector2 uvCoord = new Vector2(viewportPoint.x, viewportPoint.y);
            return uvCoord;
        }
    }

    Droplet[] droplets;
    Texture2D gradTexture;
    private Material material;
    float timer;
    int dropCount;

    void UpdateShaderParameters()
    {
        var c = Camera.main;
        if (c == null || material == null)
            return;

        material.SetVector("_radius", new Vector4(radius, 0.0f, 0.0f, 0.0f));
        material.SetVector("_Drop1", droplets[0].MakeShaderParameter(c.aspect));
        material.SetVector("_Drop2", droplets[1].MakeShaderParameter(c.aspect));
        material.SetVector("_Drop3", droplets[2].MakeShaderParameter(c.aspect));

        material.SetColor("_Reflection", reflectionColor);
        material.SetVector("_Params1", new Vector4(c.aspect, 1, 1 / waveSpeed, 0));
        material.SetVector("_Params2", new Vector4(1, 1 / c.aspect, refractionStrength, reflectionStrength));
    }

    void Awake()
    {
        _meshFilter = targetObject.GetComponent<MeshFilter>();
        droplets = new Droplet[3];
        droplets[0] = new Droplet(_meshFilter, targetObject);
        droplets[1] = new Droplet(_meshFilter, targetObject);
        droplets[2] = new Droplet(_meshFilter, targetObject);
        droplets[0].Reset();

        //texture构建
        gradTexture = new Texture2D(2048, 1, TextureFormat.Alpha8, false);
        gradTexture.wrapMode = TextureWrapMode.Clamp;
        gradTexture.filterMode = FilterMode.Bilinear;
        for (var i = 0; i < gradTexture.width; i++)
        {
            var x = 1.0f / gradTexture.width * i;
            var a = waveform.Evaluate(x);
            gradTexture.SetPixel(i, 0, new Color(a, a, a, a));
        }
        gradTexture.Apply();

        // 创建材质
        if (shader != null)
        {
            material = new Material(shader);
            material.hideFlags = HideFlags.DontSave;
            material.SetTexture("_GradTex", gradTexture);
            UpdateShaderParameters();
        }
        else
        {
            Debug.LogError("Shader not assigned to RippleEffect component!");
        }
    }

    void Update()
    {
        if (activation)
        {
            if (timer <= 2.0f)
            {
                droplets[0].Update();
                UpdateShaderParameters();
                timer += Time.deltaTime;
            }
            if (timer > 2.0f)
            {
                activation = false;
            }
        }
        else
        {
            timer = 0.0f;
            droplets[0].Reset();
        }
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (material != null)
        {
            Graphics.Blit(source, destination, material);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }

    public void Emit()
    {
        droplets[dropCount++ % droplets.Length].Reset();
    }
}