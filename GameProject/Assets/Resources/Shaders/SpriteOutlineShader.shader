
shader "Sprites/Outline"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        
        // 描边颜色
        _OutlineColor ("Outline Color", Color) = (1,1,1,1) 
        // 描边宽度
        _OutlineWidth ("Outline Width", Range(0, 10)) = 1 
        // 是否开启描边
        _OutlineEnabled ("Outline Enabled", Float) = 0     
    }

    SubShader
    {
        Tags
        { 
            "Queue"="Transparent" 
            "RenderType"="Transparent" 
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            fixed4 _Color;
            fixed4 _OutlineColor;
            float _OutlineWidth;
            float _OutlineEnabled;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color * _Color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 mainColor = tex2D(_MainTex, i.uv) * i.color;
                
                // 如果描边未启用，或主颜色是透明的，就直接返回
                if (_OutlineEnabled < 0.5 || mainColor.a > 0.0)
                {
                    return mainColor;
                }

                // 检查上下左右的像素
                float2 texelSize = _MainTex_TexelSize.xy * _OutlineWidth;

                fixed4 outlineSample = tex2D(_MainTex, i.uv + float2(texelSize.x, 0));
                outlineSample += tex2D(_MainTex, i.uv - float2(texelSize.x, 0));
                outlineSample += tex2D(_MainTex, i.uv + float2(0, texelSize.y));
                outlineSample += tex2D(_MainTex, i.uv - float2(0, texelSize.y));
                
                // 如果任何相邻像素不透明 (a > 0)，就绘制描边颜色
                if (outlineSample.a > 0.0)
                {
                    return _OutlineColor * i.color.a;
                }
                // 否则返回透明
                return mainColor; 
            }
        ENDCG
        }
    }
}修改这个shader 让shader适配圆形，弧形