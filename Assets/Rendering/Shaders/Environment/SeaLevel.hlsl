#ifndef SEA_LEVEL_INCLUDED
#define SEA_LEVEL_INCLUDED

float2 noisePos()
{
	float t = _Time.x * 2;
	return float2(_CosTime.x * 2 + t, _SinTime.x * 2 + t);
}

float2 applyXStretch(float2 p, float4 texelSize)
{
	return float2(p.x * texelSize.x * texelSize.w, p.y);
}


float SeaLevel(
	float2    screenPos,
	sampler2D seaSquaresBase,
	sampler2D noise,
	float4    noiseSize,
	sampler2D ripple,
	float4    rippleSize,
	bool      addSeaBase)
{
	const float  PI = 3.14159265f;
	const float2 pp = round(screenPos * _ScreenParams.xy / 2);
	const float2 p = pp / _ScreenParams.xy * 2;

	float foamLevel = addSeaBase * tex2D(seaSquaresBase, p);

	// Noise

	foamLevel +=
	(
		tex2D(
			noise,
			applyXStretch(p, noiseSize) + _Time.x * float2(0.4, -0.1)
		).x
	) * 0.09;

	foamLevel +=
	(
		tex2D(
			noise,
			applyXStretch(p, noiseSize) + _Time.x * float2(-0.2, 0.7)
		).x
	) * 0.09;

	// Ripple

	foamLevel +=
	(
		tex2D(
			ripple,
			applyXStretch(p, rippleSize) * 2.5 + _Time.x * float2(-0.7, -0.1)
		).x
	) * 0.1;

	foamLevel +=
	(
		tex2D(
			ripple,
			applyXStretch(p, rippleSize) * 2.5 + _Time.x * float2(0.4, 0.2)
		).x
	) * 0.1;

	// Waves

	float wavePhase = sin(screenPos.y + screenPos.x * 0.7) * 60;

	foamLevel += abs(sin(_Time.x * 30 + wavePhase * 0.1) * 0.4 + 0.6 - foamLevel) < 0.1 ? 0.3 : 0;
	foamLevel += abs(sin(_Time.x * 30 + PI + wavePhase * 0.15) * 0.4 + 0.6 - foamLevel) < 0.05 ? 0.3 : 0;

	// Dither
	foamLevel += ((pp.x / 2 + pp.y) % 2 * 2 - 1) * 0.006;

	// Posterize
	return clamp(foamLevel, 0, 1);
}
#endif
