#version 330
 
// shader input
in vec4 position;
in vec4 normal;
in vec2 uv;            // interpolated texture coordinates
uniform vec3 cameraPos;
uniform vec3 lightPos;
uniform vec3 lightColor;
uniform sampler2D text;    // texture sampler

// shader output
out vec4 outputColor;

// fragment shader
void main()
{
	//ambient
	float aStrength = 0.1;
	vec3 ambient = aStrength * lightColor;

	//diffuse
	vec3 L = lightPos - position.xyz;
	vec3 normalizer = normalize(normal.xyz);
	float attenuation = 1.0 / dot(L, L);
	L = normalize(L);
	float dStrength = max(dot(normalizer, L), 0.0);
	vec3 diffuseColor = texture(text, uv).rgb;
	vec3 diffuse = dStrength * lightColor;

	//specular
	float sStrength = 0.5;
	vec3 viewDir = normalize(cameraPos - position.xyz);
	vec3 reflectDir = reflect(-L, normalizer);
	float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32.0);
	vec3 specular = spec * sStrength * lightColor;
	vec3 result = (ambient + diffuse + specular) * diffuseColor *attenuation;
	outputColor = vec4(result, 1.0);
}   