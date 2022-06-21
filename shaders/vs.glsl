#version 330
 
// shader input
in vec2 vUV;			// vertex uv coordinate
in vec3 vnormal;		// untransformed vertex normal
in vec3 vposition;		// untransformed vertex position
uniform mat4 O2S;		//Matrix to transform from object to screen space
uniform mat4 O2W;		//Matrix to transform from object to world space

// shader output
out vec4 normal;		// transformed vertex normal
out vec2 uv;				
out vec4 position;
 
// vertex shader
void main()
{
	// transform vertex using supplied matrix
	gl_Position = O2S * vec4(vposition, 1.0);
	position = O2W * vec4(vposition, 1.0);
	position = O2W * vec4(vposition, 0.0);
	uv = vUV;
}