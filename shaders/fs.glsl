#version 330
 
// shader input
in vec4 position;
in vec4 normal;
in vec2 uv;			// interpolated texture coordinates
uniform vec3 cameraPos;
uniform vec3 lightPos;
uniform vec3 lightColor;
uniform sampler2D texture;	// texture sampler

// shader output
out vec4 color;

// fragment shader
void main()
{
    color = ;
}