Shader "Custom/SpriteRevealTopToBottom"
{
    Properties{
        [PerRendererData]_MainTex("Sprite",2D)="white"{}
        _Color("Tint", Color) = (1,1,1,1)
        _Fill("Fill(Top->Bottom)", Range(0,1)) = 0
        _Feather("Feather(px)", Range(0,0.1)) = 0.02
    }
    SubShader{
        Tags{ "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" "CanUseSpriteAtlas"="True"}
        ZWrite Off Blend SrcAlpha OneMinusSrcAlpha Cull Off
        Pass{
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            sampler2D _MainTex; float4 _MainTex_ST;
            fixed4 _Color; float _Fill; float _Feather;
            struct v2f{ float4 pos:SV_POSITION; float2 uv:TEXCOORD0; };
            v2f vert(appdata_full v){
                v2f o; o.pos=UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord,_MainTex); return o;
            }
            fixed4 frag(v2f i):SV_Target{
                fixed4 c = tex2D(_MainTex,i.uv) * _Color;
                // 从上到下显现：uv.y 小于 Fill 的保留
                float t = saturate(_Fill);
                float reveal = smoothstep(1.0 - t, 1.0 - t + _Feather, i.uv.y);
                c.a *= reveal;
                return c;
            }
            ENDCG
        }
    }
}
