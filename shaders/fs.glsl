#version 330
 
// shader input
in vec4 position;
in vec4 normal;
in vec2 uv;			// interpolated texture coordinates
uniform vec3 cameraPos;
uniform vec3 lightPos;
uniform vec3 lightColor;
uniform sampler2D tex;	// texture sampler

// shader output
out vec4 color;

// fragment shader
void main()
{
	//diffuse
	vec3 L = lightPosition-position.xyz;
	float attenuation = 1.0 / dot(L, L);
	float Dot = max(0.0, dot(normalize(L), normalize(normal.xyz)));
	vec3 diffuseColor = texture(tex, uv).rgb;

	//specular
	float strength = 0.5;
	vec3 viewDirection = normalize(cameraPos - position);
	vec3 reflectDirection = reflect(-L, normalize(normal))
	float spec = pow(max(dot(viewDirection, reflect direction),0.0), 32)
	color = vec4(lightColor * diffuseColor * attenuation * Dot, 1.0);
}   