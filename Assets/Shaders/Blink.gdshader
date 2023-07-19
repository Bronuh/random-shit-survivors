shader_type canvas_item;

uniform float hit_opacity = 1;
uniform sampler2D screen : hint_screen_texture;

void fragment(){
	vec3 white = vec3(1,0.5,0.5);
	vec3 color = texture(TEXTURE, UV).rgb;
	
	vec3 whitened_texture_rgb = mix(white, color, hit_opacity);
	
	COLOR.rgb = mix(white, COLOR.rgb, hit_opacity);//color;
	COLOR.a = texture(TEXTURE, UV).a;
}
