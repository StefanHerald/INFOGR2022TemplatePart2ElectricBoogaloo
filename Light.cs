using System;
using OpenTK;

namespace Template
{
    internal class Light
    {
        public Vector3 lightPosition;
        public Vector3 lightColor; 
        public Light(Vector3 lightPos, Vector3 color)
        {
            this.lightPosition = lightPos;
            this.lightColor = color;
        }
    }
}
