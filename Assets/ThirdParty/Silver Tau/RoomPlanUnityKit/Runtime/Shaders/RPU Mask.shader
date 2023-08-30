Shader "SilverTau/RPU Mask" {
	Properties {
	    _Ref("Ref", Float) = 1
	    [Enum(UnityEngine.Rendering.CompareFunction)] _StencilComp ("Stencil Comparison", Float) = 6
    }

    SubShader {
        Tags {"Queue"="Geometry-15"}
        Lighting Off
        ColorMask 0
        
        Pass {
            Stencil {
                Ref [_Ref]
                Comp [_StencilComp]
                Pass keep
            }
        }
    }
}
