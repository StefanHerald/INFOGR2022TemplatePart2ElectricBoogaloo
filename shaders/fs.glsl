#version 330
 
// shader input
in vec4 position;
in vec4 normal;
in vec2 uv;            // interpolated texture coordinates
uniform vec3 cameraPos;
uniform vec3 lightPos;
uniform vec3 lightColor;
uniform sampler2D texture;    // texture sampler

// shader output
out vec4 outputColor;

// fragment shader
void main()
{
//    lightPos = vec3(0,14,0);
//    lightColor = vec3(255,255,255);
    //diffuse
    vec3 L = lightPos-position.xyz;
    float attenuation = 1.0 / dot(L, L);
    float Dot = max(0.0, dot(normalize(L), normalize(normal.xyz)));
    vec3 diffuseColor = texture(texture, uv).rgb;

    //specular

    float strength = 0.5;
    vec3 viewDirection = normalize(cameraPos - position.xyz);
    vec3 reflectDirection = reflect(-L, normalize(normal.xyz));
    float spec=pow(max(dot(viewDirection, reflectDirection),0.0), 32.0);
    outputColor = vec4( lightColor * Dot * (diffuseColor) * attenuation , 1.0);
}