#ifndef SNAP_NODE_INCLUDED
#define SNAP_NODE_INCLUDED
    
void Snap_float(float4 A, float2 Offset, float SnapSize, bool UseScreen, out float4 Snapped)
{
    float2 hpc = UseScreen ? _ScreenParams.xy : float2 (1, 1);
    float2 pixelPos = round ((hpc * A.xy - Offset) * SnapSize) / SnapSize;
    
    A.xy = (pixelPos + Offset) / hpc;
    Snapped = A;
} 
#endif