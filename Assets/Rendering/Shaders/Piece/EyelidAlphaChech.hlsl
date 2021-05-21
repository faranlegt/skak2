#ifndef EYELID_ALPHA_CHECK_NODE_INCLUDED
#define EYELID_ALPHA_CHECK_NODE_INCLUDED
    
void EyeLidAlphaCheckNode_float(float angle, float radius, float c, float closeLevelUp, float closeLevelDown, float2 pos, out float alpha)
{
	const float s = sign(angle);
	const float signed_c = c * s;
	const float threshold = radius * cos(angle);

	alpha = ((1 - pos.x) * s > threshold + signed_c)
			|| pos.y > closeLevelUp
			|| pos.y < closeLevelDown;
} 
#endif