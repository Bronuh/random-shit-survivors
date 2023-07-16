shader_type canvas_item;

uniform float hit_opacity = 1;

void fragment(){
	vec3 white = vec3(1,1,1);
	vec3 color = texture(TEXTURE, UV).rgb;
	
	vec3 whitened_texture_rgb = mix(white, color, hit_opacity);
	
	COLOR.rgb = color;
	COLOR.a = texture(TEXTURE, UV).a;
}
