shader_type canvas_item;

void fragment() {
	vec2 ts = UV.xy;
	ts.y = 1.0 - ts.y;
	COLOR = vec4(ts.x, ts.y, 0.0, 1.0);
	// Place fragment code here.
}
