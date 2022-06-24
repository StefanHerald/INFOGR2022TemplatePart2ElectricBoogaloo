#version 330
 
// shader input
in vec4 position;
in vec4 normal;
in vec2 uv;            // interpolated texture coordinates
uniform vec3 cameraPos;
<<<<<<< Updated upstream
uniform vec3 lightPos;
uniform vec3 lightColor;
uniform sampler2D text;    // texture sampler
=======
uniform vec3 ray1;
uniform vec3 ray2;
uniform int SpotB;
uniform vec3 Length;
uniform sampler2D texture;    // texture sampler
>>>>>>> Stashed changes

// shader output
out vec4 outputColor;

// fragment shader
void main()
{
	//ambient
	float aStrength = 0.1;
	vec3 ambient = aStrength * lightColor;
	//initializing spotlights
	float diamL = 0;
	float diamW = 0;
	if(SpotB == 1)
	{
    float dot = dot(ray1.xy, ray2.xy);
    vec2 temp = sin(dot) * ray1.xy;
	diamL = temp.x;
    dot = dot(ray1.zy, ray2.zy);
    temp = sin(dot) * ray1.zy;
	diamW = temp.x;
	}


	//diffuse
	vec3 L = lightPos - position.xyz;
	float attenuation = 1.0 / dot(L, L);
	L = normalize(L);
	float dStrength = max(dot(normalizer, L), 0.0);
	vec3 diffuseColor = texture(text, uv).rgb;
	float dStrength = 0.0;
	if((L.x >=  (diamL + Length.x)&& L.x < Length.x && L.z >= (diamW + Length.z)&& L.z < Length.z)
	|| SpotB != 1)
	{
		dStrength = max(dot(normalizer, L), 0.0);
	}
	vec3 diffuseColor = texture(texture, uv).rgb;
>>>>>>> Stashed changes
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