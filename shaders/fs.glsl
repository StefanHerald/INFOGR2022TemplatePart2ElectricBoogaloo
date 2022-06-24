#version 330
 
// shader input
in vec4 position;			//position of the fragment
in vec4 normal;				//normal of the fragment
in vec2 uv;					// interpolated texture coordinates
uniform vec3 cameraPos;		//position of the camera
uniform vec3 lightPos;		//position of the light
uniform vec3 lightColor;	//color of the light
uniform sampler2D text;		// texture sampler
uniform vec3 ray1;			//most left ray from the spotlight
uniform vec3 ray2;			//most right ray from the spotlight
uniform int SpotB;			//boolean that checks wether there is a spotlight (0 is no spotlight, 1 is spotlight)
uniform vec3 Length;		//Describes the length between the light an the start of the ellipse (on the x and z axis)
// shader output
out vec4 outputColor;

// fragment shader
void main()
{

	//ambient lighting
	float aStrength = 0.1;
	vec3 ambient = aStrength * lightColor;

	//initializing spotlights
	float diamL = 0;
	float diamW = 0;
	if(SpotB == 1)
	{
    float Dot = dot(ray1.xy, ray2.xy);
    vec2 temp = sin(Dot) * ray1.xy;
	diamL = temp.x;
    float Dot2 = dot(ray1.zy, ray2.zy);
    temp = sin(Dot2) * ray1.zy;
	diamW = temp.x;
	}


	//diffuse lighting
	vec3 normalizer = normalize(normal.xyz);
	vec3 L = lightPos - position.xyz;
	float attenuation = 1.0 / dot(L, L);
	L = normalize(L);
	float dStrength = 0.0;
	if((L.x >=  (diamL + Length.x)&& L.x < Length.x && L.z >= (diamW + Length.z)&& L.z < Length.z)
	|| SpotB != 1)
	{
		dStrength = max(dot(normalizer, L), 0.0);
	}
	vec3 diffuseColor = texture(text, uv).rgb;
	vec3 diffuse = dStrength * lightColor;

	//specular lighting
	float sStrength = 0.5;
	vec3 viewDir = normalize(cameraPos - position.xyz);
	vec3 reflectDir = reflect(-L, normalizer);
	float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32.0);
	vec3 specular = spec * sStrength * lightColor;
	vec3 result = (ambient + diffuse + specular) * diffuseColor *attenuation;
	outputColor = vec4(result, 1.0);
}