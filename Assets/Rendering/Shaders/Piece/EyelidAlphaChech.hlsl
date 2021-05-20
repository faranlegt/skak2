#ifndef EYELID_ALPHA_CHECK_NODE_INCLUDED
#define EYELID_ALPHA_CHECK_NODE_INCLUDED
    
void EyeLidAlphaCheckNode_float(float angle, float radius, float c, float closeLevel, float2 pos, out float alpha)
{
	const float s = sign(angle);
	const float signed_c = c * s;
	const float threshold = radius * cos(angle);
	const float dy = abs(pos.y - c);
	const float closeAngle = lerp(0, PI, closeLevel);
	const float closeMaxY =  abs(pos.x - c) * tan(closeAngle);

	alpha = ((1 - pos.x) * s > threshold + signed_c) || (pos.y > pos.x);
} 
#endif