Shader "Unlit/TestShader"
{
    Properties
    {
        // _MainTex ("Texture", 2D) = "white" {}
        _Color1   ("Main Color (A=Opacity)", Color) = (1,1,1,1)
        _Color2   ("Main Color (A=Opacity)", Color) = (1,1,1,1)
        _Height("Height", Range(0, 3)) = 0.25
    }
    SubShader
    {
        Tags {"Queue"="Transparent"  "RenderType"="Transparent"  "IgnoreProjector" = "true" }//  "RenderType"="Opaque" 
        Zwrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                // float2 uv : TEXCOORD0;
                // UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float height: TEXCOORD0;
            };

            // sampler2D _MainTex;
            // float4 _MainTex_ST;
            float  _Height;
            float4 _Color1;
            float4 _Color2;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                // o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.height = v.vertex.y;
                // UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                // fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                // UNITY_APPLY_FOG(i.fogCoord, col);

                // return col;

                // float4 col = float4(0.5, 0.5, 0.5, 0.0);
                // float y = (i.height + 1) /2;
                // if(i.height > 0.5) 
                // {
                //     y = 
                // }
                // if(i.height > _Height) 
                // {
                //     return _Color1;
                // } else 
                // {
                //     return _Color2;
                // }
                if(i.height > _Height) {
                    fixed4 col = _Color1;
                    col.a = 0.0;
                    return col;
                    // return _Color1;
                } else 
                {
                    return _Color2;
                    // float green = _Height/3.0;
                    // return fixed4(1 - green, green , 0.0, 1.0);
                }

            }
            ENDCG
        }
    }
}
