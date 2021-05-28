#ifndef IS_PUPIL_NODE_INCLUDED
#define IS_PUPIL_NODE_INCLUDED
    
void IsPupil_float(float2 pos, float2 pupilPos, float pupilSize, out bool isPupil)
{
	const float pSize = pupilSize * 0.5f;
	const float2 centeredPos = pos - float2(0.5, 0.5);
	isPupil =
		centeredPos.x >= pupilPos.x - pSize
		&& centeredPos.x <= pupilPos.x + pSize
		&& centeredPos.y >= pupilPos.y - pSize
		&& centeredPos.y <= pupilPos.y + pSize;
} 
#endif