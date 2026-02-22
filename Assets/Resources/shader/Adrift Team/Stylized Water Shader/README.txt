This is a custom water shader made for Unity using Shader Graph. It includes water color, foam, distortion, flow movement, and depth fade effects.

Requirements:
- Unity 2021 or newer
- URP (Universal Render Pipeline)
- Shader Graph
- Camera depth texture enabled

How to Use:
1. Import the shader into your project.
2. Create a new Material using the shader.
3. Apply the material to a mesh (e.g. a plane or quad).
4. Assign the required textures:
   - Noise Texture (for surface movement)
   - Refraction Texture (for distortion)
   - Foam Texture (for shoreline foam)

Material Properties:
- WaterColor: Base color of the water
- FadeColor: Color for depth-based fade
- FoamColor: Color of the foam
- FlowDirection: Direction of movement (0–1)
- DistortionSpeed / Strength: Controls water flow and distortion
- Tiling: Controls how repeated the textures are
- FadeDistance: How far the fade effect reaches
- FoamAmount / Cutoff / Scale: Controls foam edges
- NoiseOpacity: How visible the noise effect is

Camera Setup:
- Enable "Depth Texture" on your main camera to allow depth-based fade and foam.

Tips:
- Use grayscale noise textures.
- Tweak Foam and Fade settings for realistic edges.
- Animate FlowDirection or Speed for dynamic water.


