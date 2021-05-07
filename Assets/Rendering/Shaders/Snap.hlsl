#ifndef SNAP_NODE_INCLUDED
#define SNAP_NODE_INCLUDED
    
void Snap_float(float4 A, float2 Offset, out float4 Snapped)
{
    float2 hpc = _ScreenParams.xy;
    float2 pixelPos = round (hpc * A.xy / A.w - Offset);
    
    A.xy = (pixelPos + Offset) / hpc * A.w;
    Snapped = A;
} 

#endif