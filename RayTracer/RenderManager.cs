using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer
{
    class RenderManager
    {
        public static Scene scene { get; private set; }


        public static void Render()
        {
            raytracer.Render();
            rendering.RotateFlip(RotateFlipType.Rotate180FlipNone);
            this.pictureBox1.Image = rendering;
        }
    }
}
