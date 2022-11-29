Shader "Unlit/HealthSilhouetteShader"
{
    Properties
    {
        _Color1   ("Main Color (A=Opacity)", Color) = (1,1,1,1)
        _Color2   ("Main Color (A=Opacity)", Color) = (1,1,1,1)
        _Height("Height", Range(-3, 3)) = 0.25
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
                float2 uv2 : TEXCOORD2;

            };

            struct v2f
            {

                float4 vertex : SV_POSITION;
                float height: TEXCOORD2;
            };


            float  _Height;
            float4 _Color1;
            float4 _Color2;

            v2f vert (appdata v)
            {
                v2f o;
                o.height = v.uv2.y;

                o.vertex = UnityObjectToClipPos(v.vertex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {


                if(i.height > _Height) {
                    fixed4 col = _Color1;
                    col.a = 0.0;
                    return col;
                } else 
                {
                    return _Color2;

                }

            }
            ENDCG
        }
    }
}
