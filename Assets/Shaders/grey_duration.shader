Shader "Custom/grey_duration"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)  // Base Color
        _GrayAmount ("Grayscale Amount", Range(0,1)) = 0 // Controls the grayscale effect
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
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            float4 _Color;
            float _GrayAmount;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = _Color; // Use the color property instead of a texture
                
                // Convert to grayscale using the luminosity method
                float lum = dot(col.rgb, float3(0.299, 0.587, 0.114));
                fixed3 grayColor = float3(lum, lum, lum);
                
                // Lerp between original color and grayscale based on _GrayAmount
                col.rgb = lerp(col.rgb, grayColor, _GrayAmount);
                return col;
            }
            ENDCG
        }
    }
}
