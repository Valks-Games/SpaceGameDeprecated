shader_type canvas_item;

uniform vec4 color : source_color;
uniform float intensity : hint_range(0, 0.6);

float circle(vec2 position, float radius, float feather)
{
	return smoothstep(radius, radius + feather, length(position - vec2(0.5)));
}

void fragment() {
	float fade_effect = sin(TIME) * cos(TIME) * 0.04;
	float outer = circle(UV, 0.22, 0.02) + 0.6 - intensity;
	float inner = 1.0 - circle(UV, 0.1, 0.1 - fade_effect);
	
	COLOR = vec4(UV, color.g, color.b);
	COLOR.a -= outer + inner;
}

void vertex(){
	//VERTEX += vec2(cos(TIME)*10.0, sin(TIME)*10.0);
}
