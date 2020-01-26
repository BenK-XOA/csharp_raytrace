using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer
{
    public class Scene
    {
        public Sphere[] spheres;
        public Light[] lights;

        public Scene(Sphere[] _spheres, Light[] _lights)
        {
            spheres = _spheres;
            lights = _lights;
        }
    }

    public class Sphere
    {
        public Vector3 center;
        public float radius;
        public Color color;
        public float specular;
        public float reflective;

        public Sphere(Vector3 _center, float _radius, Color _color, float _spec, float _reflec)
        {
            center = _center;
            radius = _radius;
            color = _color;
            specular = _spec;
            reflective = _reflec;
        }
    }

    public class Light
    {
        public LightType type;
        public float intensity;
        public Vector3 position;
        public Vector3 direction;

        public Light(LightType t, float i, Vector3 pos, Vector3 dir)
        {
            type = t;
            intensity = i;
            position = pos;
            direction = dir;
        }
    }

    public enum LightType
    {
        ambient,
        point,
        directional
    }

    public struct Vector3
    {
        float x;
        float y;
        float z;

        public Vector3(float _x, float _y, float _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }

        public static float Dot(Vector3 a, Vector3 b)
        {
            return (a.x * b.x) + (a.y * b.y) + (a.z * b.z);
        }

        public static float Length (Vector3 a)
        {
            return (float)Math.Sqrt(Math.Pow(a.x, 2) + Math.Pow(a.y, 2) + Math.Pow(a.z, 2));
        }

        public static Vector3 operator /(Vector3 a, float divisor)
        {
            return new Vector3(a.x / divisor, a.y / divisor, a.z / divisor);
        }

        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static Vector3 operator * (Vector3 a, float multiplier)
        {
            return new Vector3(a.x * multiplier, a.y * multiplier, a.z * multiplier);
        }
        

        public static Vector3 operator *(float multiplier, Vector3 a)
        {
            return new Vector3(a.x * multiplier, a.y * multiplier, a.z * multiplier);
        }

        public static Vector3 operator *(Vector3 a, int multiplier)
        {
            return new Vector3(a.x * multiplier, a.y * multiplier, a.z * multiplier);
        }

        public static Vector3 operator *(int multiplier, Vector3 a)
        {
            return new Vector3(a.x * multiplier, a.y * multiplier, a.z * multiplier);
        }

        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static Vector3 operator -(Vector3 a)
        {
            return new Vector3(-a.x, -a.y, -a.z);
        }

        public static Vector3 operator +(Vector3 a, float additor)
        {
            return new Vector3(a.x + additor, a.y + additor, a.z + additor);
        }
    }

    public struct Color
    {
        public float r;
        public float g;
        public float b;

        public Color (float _r, float _g, float _b)
        {
            _r = LimitRange(_r);
            _g = LimitRange(_g);
            _b = LimitRange(_b);

            r = _r;
            g = _g;
            b = _b;
        }

        public static Color operator *(Color a, float multiplier)
        {
            return new Color(a.r * multiplier, a.g * multiplier, a.b * multiplier);
        }

        public static Color operator +(Color a, Color b)
        {
            return new Color(a.r + b.r, a.g + b.g, a.b + b.b);
        }

        public string toString()
        {
            return "R: " + r + " G: " + g + " B: " + b;
        }

        public static float LimitRange(float channel)
        {
            if (channel > 255 || float.IsPositiveInfinity(channel) || float.IsNaN(channel))
                channel = 255;
            if (channel < 0  || float.IsNegativeInfinity(channel))
                channel = 0;

           


            return channel;
        }
    }
}
