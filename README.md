# CGAssignment1
**All assets used in the project (models and textures) were made by me using Blender and Substance Painter**
## Implemented Effects
### Outlines
![image](https://github.com/user-attachments/assets/40578e95-9d47-4f5e-8d6e-e0d2a98df9d5)
```hlsl
// Dedicated Outline Pass
        Pass {
            Name "Outline"
            Tags { "LightMode" = "Always" } // Makes it so that the outline is not affected by light, as that would look...weird
            Cull Front // Set to front, as outlines need to only render the inside, or else the mesh will become just a solid color of the outline
            ZWrite On
            ZTest LEqual
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            struct appdata {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct v2f {
                float4 pos : POSITION;
                float4 color : COLOR;
            };
            uniform float _outlineThickness; // Only need thickness and colour for the outline
            uniform float4 _outlineColour;
            v2f vert (appdata v) {
                // Extrudes the outline along the normals
                v2f o;
                float3 norm = mul((float3x3)unity_ObjectToWorld, v.normal); // Gets the normals to extrude from (this line was given by ChatGPT, as I didn't know how to get the normals)
                o.pos = UnityObjectToClipPos(v.vertex + norm * (_outlineThickness/10000)); // Divided by 10000, as the outlines were waaaay too thick otherwise
                o.color = _outlineColour;
                return o;
            }
            half4 frag (v2f i) : SV_Target {
                return i.color; // The final output for the pass
            }
            ENDCG
        }
```
Outline works by effectively taking the normals of the mesh, and extruding them slightly outword. It then uses front face culling to give it the appearance of being an outline. The lighting is disabled, so the outlines aren't affected by shadows/lights, so they stay consistent

### Rim Lighting
![image](https://github.com/user-attachments/assets/7177cb81-6587-4217-9a29-0f48a3d5ede9)
```hlsl
half rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal)); // Calculates the rim lighting, using the angle of a given normal and the view direction
o.Emission += _rimColour.rgb * pow(rim, 8 - _rimPower) * _rimEmission; // Adds the rim lighting to the emission, as it should be emissive
```
Rim lighting takes the angle between a given normal of the mesh, and the viewing angle. The larger the angle (the more a normal on the mesh faces away) the brighter the rim lighting is. The rim lighting is then added to the emission, so that it glows as well

### PBR Options
![image](https://github.com/user-attachments/assets/5fd81d53-09c0-4b49-af2f-7ad8a706a950)

**Normal Mapping**
```hlsl
o.Normal = UnpackNormal(tex2D(_mainNormal, IN.uv_mainTexture)); // Sets the normal map
```

**Smoothness (With Invert Option)**
```hlsl
if (_InvertSmoothness > 0.5)
    o.Smoothness = (1.0 - tex2D(_smoothnessMap, IN.uv_mainTexture).r) * _smoothness; // Used to invert the smoothness map, so the user can use roughness maps instead
else
    o.Smoothness = tex2D(_smoothnessMap, IN.uv_mainTexture).r * _smoothness;
```
Option to invert the smoothness map, so that roughness maps can be used without having to invert them in photo editing software

**Metallic**
```hlsl
o.Metallic = tex2D(_metallicMap, IN.uv_mainTexture).r * _metallic;
```
