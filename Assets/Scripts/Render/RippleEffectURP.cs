using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class RippleEffectURP : MonoBehaviour
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
    private Shader shader;

    public GameObject targetObject;
    private MeshFilter _meshFilter;
    public bool activation = false;

    private Material material;
    private Texture2D gradTexture;
    private float timer;

    class Droplet
    {
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
            if (!_targetObject || !meshFilter || !Camera.main)
                return Vector2.zero;

            Vector3 meshCenter = meshFilter.sharedMesh.bounds.center;
            Vector3 worldCenter = _targetObject.transform.TransformPoint(meshCenter);
            Vector3 viewportPoint = Camera.main.WorldToViewportPoint(worldCenter);
            return new Vector2(viewportPoint.x, viewportPoint.y);
        }
    }

    private Droplet droplet;

    void Awake()
    {
        if (targetObject)
            _meshFilter = targetObject.GetComponent<MeshFilter>();

        droplet = new Droplet(_meshFilter, targetObject);
        droplet.Reset();

        // 创建梯度纹理
        gradTexture = new Texture2D(2048, 1, TextureFormat.RGBA32, false);
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
        if (shader)
        {
            material = new Material(shader);
            material.hideFlags = HideFlags.DontSave;
            material.SetTexture("_GradTex", gradTexture);

            // 设置材质到渲染通道
            RippleEffectRenderPass.SetMaterial(material);
        }
    }

    void Update()
    {
        if (!material) return;

        if (activation)
        {
            if (timer <= 2.0f)
            {
                droplet.Update();
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
            droplet.Reset();
        }
    }

    void UpdateShaderParameters()
    {
        if (!material || !Camera.main)
            return;

        var c = Camera.main;

        material.SetVector("_radius", new Vector4(radius, 0.0f, 0.0f, 0.0f));
        material.SetVector("_Drop1", droplet.MakeShaderParameter(c.aspect));
        material.SetVector("_Drop2", Vector4.zero);
        material.SetVector("_Drop3", Vector4.zero);

        material.SetColor("_Reflection", reflectionColor);
        material.SetVector("_Params1", new Vector4(c.aspect, 1, 1 / waveSpeed, 0));
        material.SetVector("_Params2", new Vector4(1, 1 / c.aspect, refractionStrength, reflectionStrength));
    }

    void OnDestroy()
    {
        if (material)
            DestroyImmediate(material);

        if (gradTexture)
            DestroyImmediate(gradTexture);

        // 清理材质引用
        RippleEffectRenderPass.SetMaterial(null);
    }

    // 用于手动触发波纹效果
    public void ActivateRipple()
    {
        activation = true;
        timer = 0f;
        droplet.Reset();
    }
}