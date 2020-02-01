using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.Basic_Objects
{
    public struct Quaternion
    {
        public double x;
        public double y;
        public double z;
        public double w;

        public Quaternion(double _x, double _y, double _z, double _w)
        {
            x = _x;
            y = _y;
            z = _z;
            w = _w;
        }

        public Quaternion (Vector3 v, float _w)
        {
            x = v.x;
            y = v.y;
            z = v.z;
            w = _w;
        }

        public static Quaternion operator *(Quaternion q1, Quaternion q2)
        {
            double nw = q1.w * q2.w - q1.x * q2.x - q1.y * q2.y - q1.z * q2.z;
            double nx = q1.w * q2.x + q1.x * q2.w + q1.y * q2.z - q1.z * q2.y;
            double ny = q1.w * q2.y + q1.y * q2.w + q1.z * q2.x - q1.x * q2.z;
            double nz = q1.w * q2.z + q1.z * q2.w + q1.x * q2.y - q1.y * q2.x;
            return new Quaternion(nw, nx, ny, nz);
        }


    }
}
