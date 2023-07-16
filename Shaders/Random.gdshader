shader_type canvas_item;

float random(vec2 st) {
	return fract(sin(dot(st.xy, vec2(14.11, 78.1122))) * 43758.1234);
}

void fragment() {
	COLOR = vec4(vec3(random(UV.xy)), 1.0);
}
