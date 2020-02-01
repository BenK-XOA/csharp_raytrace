using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.Scene_Objects
{
    public class Camera : SceneObject
    {
        public Viewport viewport;
    }

    /// <summary>
    /// Struct of viewport
    /// contains width, height and distance from camera in worldspace units
    /// try it with 1,1,1, but it's relative to scene objects ofc.
    /// </summary>
    public struct Viewport
    {
        public double width;
        public double height;
        public double distance;

        public Viewport(double _w, double _h, double _d)
        {
            width = _w;
            height = _h;
            distance = _d;
        }
    }

}
