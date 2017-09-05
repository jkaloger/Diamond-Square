// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Jack Kaloger 2017
// Phong illumination shader for COMP30019
// based on the work provided in labs, as well as the wikibooks tutorial:
// https://en.wikibooks.org/wiki/Cg_Programming/Unity/Diffuse_Reflection
Shader "Landscape Shader" {
    Properties {
        // landscape props
        _Height ("Landscape height", Float) = 1
        _GrassLevel ("Level that grass starts (% of height)", Float) = 0.33
        _RockLevel ("Level that rock starts (% of height)", Float) = 0.5
        _IceLevel ("Level that ice starts (% of height)", Float) = 0.9

        // colour props
        _GroundColour ("Ground/Dirt Colour", Color) = (1,1,1,1)
        _GrassColour ("Grass Colour", Color) = (1,1,1,1)
        _RockColour ("Rock Colour", Color) = (1,1,1,1)
        _IceColour ("Ice Colour", Color) = (1,1,1,1)

        // specular props
        _Specular ("Specular Colour", Color) = (1,1,1,1) 
        _Gloss ("Gloss (smaller == bigger specular)", Float) = 10
    }
    SubShader {
        Pass {  
            Tags { "LightMode" = "ForwardBase" } 

            CGPROGRAM

            #pragma vertex vert  
            #pragma fragment frag 

            #include "UnityCG.cginc"

            // global var for directional light colour
            // (Our sun)
            uniform float4 _LightColor0; 

            // landscape data
            uniform float _Height;
            uniform float _GrassLevel;
            uniform float _RockLevel;
            uniform float _IceLevel;

            // colours
            uniform float4 _GroundColour;
            uniform float4 _GrassColour;
            uniform float4 _RockColour;
            uniform float4 _IceColour;

            // specular values
            uniform float4 _Specular;
            uniform float _Gloss;

            struct vIn {
                float4 pos : POSITION;
                float3 norm : NORMAL;
            };
            struct vOut {
                float4 pos : SV_POSITION;
                float4 col : COLOR;
            };

            vOut vert(vIn input) {
                vOut o;

                // first we'll calculate where this vertex is on our landscape,
                // and from that determine what colour it should be..
                float4 colour = _GroundColour; // by default we'll just say its dirt..
                if( input.pos.y > (_Height * _GrassLevel) )
                    colour = _GrassColour; // vertex is grass or higher
                if( input.pos.y > (_Height * _RockLevel) )
                    colour = _RockColour; // vertex is rock or higher
                if( input.pos.y > (_Height * _IceLevel) )
                    colour = _IceColour; // vertex is ice

                // we need the normal from the vertex/surface, transformed from world coords
                float3 normDir = normalize( mul(input.norm, unity_WorldToObject) );
                // we also need the direction of our camera
                float3 viewDir = normalize(_WorldSpaceCameraPos - mul(unity_ObjectToWorld, input.pos).xyz);

                // since we're only using a directional light, grab its direction straight from the constant
                // (attenuation value of the directional light will be 1.0 so we can ignore it..)
                float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);

                // Phong illumination model is made up of Ambient, Diffuse and Specular components

                // Ambient component radiated light intensity
                // here, Ia is grabbed from the unity lightmodel
                // and the albedo is simply our colour :)
                float3 amb = UNITY_LIGHTMODEL_AMBIENT.rgb * colour.rgb;

                // Lambertian component/diffuse we say Intensity * reflectivity * cos(theta)
                // here, we use our surface normal and light source directions in place
                // of cos, where cos(theta) == N . L (this has to be at least 0)
                // _LightColor0 grabs our sun's colour
                float3 diff = _LightColor0.rgb * colour.rgb * max(0.0, dot(normDir, lightDir));

                // Specular is determined by orientation ,distance from viewer
                // and light source.
                float3 spec;
                // check if the light is behind the vertex
                if (dot(normDir, lightDir) < 0.0) {
                    spec = float3(0.0,0.0,0.0);
                } else { // otherwise add specular
                	// we say specular is intensity * reflectivity * cosa^n
                	// here, we replace cos once again with the dot product
                	// our n value is gloss (the specular reflection exponent)
                    spec = _LightColor0.rgb *
                            _Specular.rgb * pow(max(0.0, dot(
                            reflect(-lightDir, normDir),
                            viewDir)), _Gloss);
                }

                // float3(ambient + diffuse + specular), alpha 
                o.col = float4(amb + diff + spec, 1.0);
                // translate using the unity matrix constant
                o.pos = UnityObjectToClipPos(input.pos);

                // done here..
                return o;
            }

            float4 frag(vOut input) : COLOR {
                // just a vertex shader :)
                return input.col;
            }

            ENDCG
        }
    }
}