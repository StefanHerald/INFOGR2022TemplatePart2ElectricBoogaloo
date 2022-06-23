#version 330
 
// shader input
in vec2 vUV;			// vertex uv coordinate
in vec3 vNormal;		// untransformed vertex normal
in vec3 vPosition;		// untransformed vertex position
uniform mat4 transform;		//Matrix to transform from object to screen space
uniform mat4 toWorld;		//Matrix to transform from object to world space

// shader output
out vec4 normal;		// transformed vertex normal
out vec2 uv;				
out vec4 position;
 
// vertex shader
void main()
{
	// transform vertex using supplied matrix
	gl_Position = transform * vec4(vPosition, 1.0);
	position = toWorld * vec4(vPosition, 1.0);
	normal = toWorld * vec4(vNormal, 0.0);
	uv = vUV;
}