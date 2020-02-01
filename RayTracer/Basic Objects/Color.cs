using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.Basic_Objects
{

    public struct Color
    {
        public int r;
        public int g;
        public int b;

        public Color(int _r, int _g, int _b)
        {
            _r = LimitRange(_r);
            _g = LimitRange(_g);
            _b = LimitRange(_b);

            r = _r;
            g = _g;
            b = _b;
        }

        public static Color operator *(Color a, double multiplier)
        {
            return new Color((int)(a.r * multiplier), (int)(a.g * multiplier), (int)(a.b * multiplier));
        }

        public static Color operator +(Color a, Color b)
        {
            return new Color(a.r + b.r, a.g + b.g, a.b + b.b);
        }

        public string toString()
        {
            return "R: " + r + " G: " + g + " B: " + b;
        }

        public static int LimitRange(int channel)
        {
            if (channel > 255 )
                channel = 255;
            if (channel < 0 )
                channel = 0;




            return channel;
        }
    }
}
