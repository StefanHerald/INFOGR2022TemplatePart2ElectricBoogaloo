using System;
using OpenTK;

namespace Template
{
    internal class Light
    {
        public Vector3 lightPosition;
        public Vector3 lightColor;
        public bool isSpot;
        public Vector3 ray1, ray2;
        public float xLength, zLength;
        public Light(Vector3 lightPos, Vector3 color, bool isSpot, Vector3 Center, Vector3 Radius, Vector3 ray1, Vector3 ray2 )
        {
            //sets all the different variables
            lightPosition = lightPos;
            lightColor = color;
            this.isSpot = isSpot;
            //if it is a spotlight, does some calculations before it enters the pipeline
            if (isSpot)
            {
                this.ray1 += Center - Radius;
                this.ray2 += Center + Radius;
                this.ray1 *= lightPos.Y / ray1.Y;
                this.ray2 *= lightPos.Y / ray2.Y;
                xLength = (float)(Math.Pow(ray2.Xy.Length, 2) - Math.Pow(lightPos.Y, 2));
                zLength = (float)(Math.Pow(ray2.Zy.Length, 2) - Math.Pow(lightPos.Y, 2));
            }
        }
    }
}
