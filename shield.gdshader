shader_type canvas_item;

uniform vec4 color : source_color;

float circle(vec2 position, float radius, float feather)
{
	return smoothstep(radius, radius + feather, length(position - vec2(0.5)));
}

void fragment() {
	float outer = circle(UV, 0.25, 0.01);
	
	float fade_effect = sin(TIME) * 0.02;
	
	float inner = 1.0 - circle(UV, 0.130, 0.1 - fade_effect);
	
	COLOR = color;
	COLOR.a -= outer + inner;
}

void vertex(){
	//VERTEX += vec2(cos(TIME)*10.0, sin(TIME)*10.0);
}
