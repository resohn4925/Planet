using UnityEditor;
using UnityEditor.TerrainTools;
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

    public bool activation = false;

    private Material material;
    private Texture2D gradTexture;
    private float timer;

    class Droplet
    {
        float time;
        Vector3 worldPosition;

        public Droplet()
        {
            time = -1000;
            worldPosition = Vector3.zero;
        }

        public void SetWorldPosition(Vector3 position)
        {
            worldPosition = position;
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
            var _position = CalculateScreenUV(worldPosition);
            return new Vector4(_position.x * aspect, _position.y, time, 0);
        }

        Vector2 CalculateScreenUV(Vector3 worldPos)
        {
            Camera camera = GetSceneViewCamera();
            if (camera == null)
                return Vector2.zero;

            Vector3 viewportPoint = camera.WorldToViewportPoint(worldPos);
            return new Vector2(viewportPoint.x, viewportPoint.y);
        }

        Camera GetSceneViewCamera()
        {
            #if UNITY_EDITOR
            foreach (SceneView sceneView in SceneView.sceneViews)
            {
                if (sceneView.camera != null)
                    return sceneView.camera;
            }
            #endif
            return Camera.main;
        }
    }

    private Droplet droplet;

    private void OnEnable()
    {
        //Init();

        // 注册编辑器模式下的更新
        #if UNITY_EDITOR
        EditorApplication.update += EditorUpdate;
        #endif
    }

    private void OnDisable()
    {
        // 注销编辑器模式下的更新
        #if UNITY_EDITOR
        EditorApplication.update -= EditorUpdate;
        #endif
    }

    private void EditorUpdate()
    {
        if (!Application.isPlaying)
        {
            // 编辑器模式下的更新逻辑
            if (!material) return;

            if (activation)
            {
                if (timer <= 2.0f)
                {
                    droplet.Update();
                    UpdateShaderParameters();
                    timer += Time.deltaTime;
                    // 强制重绘场景视图
                    SceneView.RepaintAll();
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
    }

    public void Init()
    {
        droplet = new Droplet();
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

        // 如果shader未设置，尝试自动查找
        if (shader == null)
        {
            shader = Shader.Find("Hidden/RippleEffect");
            if (shader == null)
            {
                Debug.LogError("未找到RippleEffect shader，请在Inspector中手动设置shader字段");
                return;
            }
            else
            {
                Debug.Log("成功找到RippleEffect shader");
            }
        }

        // 创建材质
        if (shader)
        {
            material = new Material(shader);
            material.hideFlags = HideFlags.DontSave;
            material.SetTexture("_GradTex", gradTexture);

            // 设置材质实例到渲染通道
            RippleEffectRenderPass.SetMaterial(material);
            Debug.Log("RippleEffect材质已创建并设置到渲染通道");
        }
        else
        {
            Debug.LogError("shader为空，无法创建材质");
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
    }

    void UpdateShaderParameters()
    {
        if (!material) return;

        Camera camera = GetCamera();
        if (camera == null)
            return;

        var c = camera;

        material.SetVector("_radius", new Vector4(radius, 0.0f, 0.0f, 0.0f));
        material.SetVector("_Drop1", droplet.MakeShaderParameter(c.aspect));
        material.SetVector("_Drop2", Vector4.zero);
        material.SetVector("_Drop3", Vector4.zero);

        material.SetColor("_Reflection", reflectionColor);
        material.SetVector("_Params1", new Vector4(c.aspect, 1, 1 / waveSpeed, 0));
        material.SetVector("_Params2", new Vector4(1, 1 / c.aspect, refractionStrength, reflectionStrength));
    }

    Camera GetCamera()
    {
        #if UNITY_EDITOR
        foreach (SceneView sceneView in SceneView.sceneViews)
        {
            if (sceneView.camera != null)
                return sceneView.camera;
        }
        #endif
        return Camera.main;
    }

    void OnDestroy()
    {
        if (material)
            DestroyImmediate(material);

        if (gradTexture)
            DestroyImmediate(gradTexture);

        RippleEffectRenderPass.SetMaterial(null);
    }

    //public void ActivateRipple()
    //{
    //    activation = true;
    //    timer = 0f;
    //    droplet.Reset();
    //}

    public void ActivateRipple(Vector3 worldPosition)
    {
        droplet.SetWorldPosition(worldPosition);
        activation = true;
        timer = 0f;
        droplet.Reset();
    }
}