Shader "Custom/NewSurfaceShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _GrayAmount ("Grayscale Amount", Range(0,1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float _GrayAmount;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                
                // Convert color to grayscale using luminosity method
                float lum = dot(col.rgb, float3(0.299, 0.587, 0.114));
                fixed3 grayColor = float3(lum, lum, lum);
                
                // Lerp between original and grayscale based on _GrayAmount
                col.rgb = lerp(col.rgb, grayColor, _GrayAmount);
                return col;
            }
            ENDCG
        }
    }
}
