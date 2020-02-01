using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.Basic_Objects
{
    public struct Vector3
    {
        public double x;
        public double y;
        public double z;

        public Vector3(double _x, double _y, double _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }

        public static double Dot(Vector3 a, Vector3 b)
        {
            return (a.x * b.x) + (a.y * b.y) + (a.z * b.z);
        }

        public static double Length(Vector3 a)
        {
            return (double)Math.Sqrt(Math.Pow(a.x, 2) + Math.Pow(a.y, 2) + Math.Pow(a.z, 2));
        }

        public static Vector3 operator /(Vector3 a, double divisor)
        {
            return new Vector3(a.x / divisor, a.y / divisor, a.z / divisor);
        }

        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static Vector3 operator *(Vector3 a, double multiplier)
        {
            return new Vector3(a.x * multiplier, a.y * multiplier, a.z * multiplier);
        }


        public static Vector3 operator *(double multiplier, Vector3 a)
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

        public static Vector3 operator +(Vector3 a, double additor)
        {
            return new Vector3(a.x + additor, a.y + additor, a.z + additor);
        }
    }
}
