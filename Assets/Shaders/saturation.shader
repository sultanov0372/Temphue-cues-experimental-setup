Shader "Custom/saturation"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (1, 1, 1, 1) // Default to white
        _SaturationRange ("Saturation Range", Float) = 4.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert

        struct Input
        {
            float3 worldPos;
        };

        float4 _BaseColor;
        float _SaturationRange;

        // Declare global shader properties
        float4 _GlobalPlayerPosition;
        float4 _GlobalTargetPosition;

        float3 ToGrayscale(float3 color)
        {
            // Convert color to grayscale using luminosity method
            float gray = dot(color, float3(0.299, 0.587, 0.114));
            return float3(gray, gray, gray);
        }

        void surf(Input IN, inout SurfaceOutput o)
        {
            float3 playerToTarget = _GlobalTargetPosition.xyz - _GlobalPlayerPosition.xyz;
            float distance = length(playerToTarget);

            // Calculate saturation factor: closer = more saturated
            float saturationFactor = saturate(distance / _SaturationRange);

            // Convert base color to grayscale
            float3 grayscaleColor = ToGrayscale(_BaseColor.rgb);

            // Interpolate between grayscale and base color based on saturation factor
            float3 finalColor = lerp(grayscaleColor, _BaseColor.rgb, saturationFactor);

            // Assign final color to output
            o.Albedo = finalColor;
            o.Alpha = _BaseColor.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}