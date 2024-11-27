# Course Project (Same Repository as Assignment 1)
**All assets used in the project (models and textures) were made by me using Blender and Substance Painter**

**Third-Party Scripts Used:**
- Post Processing (Depth of field, bloom, colour grading, lens distortion, ambient occlusion)
- Dynamic Bones (Used to make tail and ears on model sway with movement)

## NEW Implemented Effects
### Vertex Distortion
Randomly offsets each vertex outward by different amounts. Changes with time, making it "almost vibrate".
I felt that this effect greatly enhanced the ethereal, nightmare-esque aesthetic when added to the enemy specters.

![VertexDistortion](https://github.com/user-attachments/assets/cea1287a-0e67-49d0-8451-23689d20dbe1)
```hlsl
float noise(float2 pos) 
{
     return frac(sin(dot(pos, float2(12.9898, 78.233))) * 43758.5453); //Values from here: https://www.reddit.com/r/GraphicsProgramming/comments/3kp644/this_generates_pseudorandom_numbers_fractsindota/
}

v2f vert(appdata v) 
{
    v2f o;
    if (_distortionStrength < 0.01) // If the distortion strength is less than 0.01, which is basically nothing, it will not distort the mesh at all, saving performance
    {
        o.pos = UnityObjectToClipPos(v.vertex);
        o.uv = v.uv;
    }
    else 
    {
        float noiseValue = noise(v.vertex.xy * _distortionSpeed + _Time.y); //Generate a random number using the noise function
        float3 distortion = v.normal * noiseValue * _distortionStrength * 0.0001; //Gets the distortion offset for the vertex
        v.vertex.xyz += distortion; //Applies the distortion to the vertex
        o.pos = UnityObjectToClipPos(v.vertex); //Sets the position of the vertex
        o.uv = v.uv; //Sets the UV of the vertex
    }
    return o;
}
```
The effect works by taking each vertex, and generating a random offset, which is then added to its current position, therefore changing it. The offset is different with each frame.

### Texture Blurring and Rotation
Blurs the textures, and optionally rotates them around in a swirl
I felt this effect was perfect in making the world feel unnatural and dream-like, as it's supposed to take place in a nightmare. It adds a lot to the environment, and immediately tells the player that the world isn't natural.

![BlurExample](https://github.com/user-attachments/assets/d5a3bb71-acbc-4c60-9745-9a4bbfbdfa03) ![BlurDiagramLR](https://github.com/user-attachments/assets/75718ca9-881e-4b68-9097-776baa4c7ed6)


```hlsl
float noise(float2 pos) 
{
    return frac(sin(dot(_Time.xy + pos, float2(12.9898, 78.233))) * 43758.5453); //Values from here: https://www.reddit.com/r/GraphicsProgramming/comments/3kp644/this_generates_pseudorandom_numbers_fractsindota/
}

float3 distortedPos = float3((IN.uv_mainTexture.x + noise(IN.uv_mainTexture) * _blurStrength * cos(_Time.y * _blurSpeed)), (IN.uv_mainTexture.y + noise(IN.uv_mainTexture) * _blurStrength * sin(_Time.y * _blurSpeed)), 0.0);
```
This effect works by distorting the UV position for the pixels by a random amount, causing them to become more blurry as the strength is increased.
The swirl effect is done by multiplying the x and y offsets using cos and sin values calculated using time, so they constantly oscillate

## Old Effects (Assignment 1)
### Outlines
Outlines highlight the edge of a mesh, and give it a sort of "focus". I felt this fit nicely to highlight the player, as well as the enemies.
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
Used specifically for the enemies, giving them an otherworldy, "ghosty" feel. It also makes them very visible, as a red rim light with emission makes them stand out from the environment.
![image](https://github.com/user-attachments/assets/7177cb81-6587-4217-9a29-0f48a3d5ede9)
```hlsl
half rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal)); // Calculates the rim lighting, using the angle of a given normal and the view direction
o.Emission += _rimColour.rgb * pow(rim, 8 - _rimPower) * _rimEmission; // Adds the rim lighting to the emission, as it should be emissive
```
Rim lighting takes the angle between a given normal of the mesh, and the viewing angle. The larger the angle (the more a normal on the mesh faces away) the brighter the rim lighting is. The rim lighting is then added to the emission, so that it glows as well

### PBR Options
These contribute to the realism that I aimed for, as I felt it was more eery
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
