using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace RayTracer
{
    partial class Program
    {

        public Viewport v;
        public Bitmap canvas;
        public Scene scene;
        public Color backgroundcolor;

        public Program(Viewport _v, Bitmap _c, Scene _s, Color _bg)
        {
            v = _v;
            canvas = _c;
            scene = _s;
            backgroundcolor = _bg;
        }


        Vector3 CanvasToViewPort(float x, float y)
        {
            return new Vector3(x * v.width / canvas.Width, y * v.height / canvas.Height, v.distance);
            
        }

        Vector3 ReflectRay(Vector3 R, Vector3 N)
        {
            return 2 * N * Vector3.Dot(N, R) - R;
        }

        float ComputeLighting(Vector3 P, Vector3 N, Vector3 V, float s)
        {
            float intensity = 0.0f;
            float t_max;
            Vector3 L;
            
            foreach(Light light in scene.lights)
            {
                if (light.type == LightType.ambient)
                    intensity += light.intensity;
                else
                {
                    if(light.type == LightType.point)
                    {
                        L = light.position - P;
                        t_max = 1;
                    }
                    else
                    {
                        L = light.direction;
                        t_max = float.MaxValue;
                    }

                    //Shadow check
                    (Sphere shadow_sphere, float shadow_t) = ClosestIntersection(P, L, 0.001f, t_max);
                    if (shadow_sphere != null)
                        continue;

                    //diffuse
                    float n_dot_1 = Vector3.Dot(N, L);
                    if (n_dot_1 > 0)
                        intensity += light.intensity * n_dot_1 / (Vector3.Length(N) * Vector3.Length(L));

                    //Specular
                    if(s != -1)
                    {
                        Vector3 R = ReflectRay(L, N);
                        float r_dot_v = Vector3.Dot(R, V);
                        if (r_dot_v > 0)
                            intensity += light.intensity * (float)Math.Pow(r_dot_v / (Vector3.Length(R) * Vector3.Length(V)), s);
                    }
                }
            }

            return intensity;
        }

        (Sphere, float) ClosestIntersection(Vector3 O, Vector3 D, float t_min, float t_max)
        {
            float closest_t = float.MaxValue;
            Sphere closest_sphere = null;

            foreach(Sphere sphere in scene.spheres)
            {
                (float t1, float t2) = IntersectRaySphere(O, D, sphere);
                if(t1 > t_min && t1 < t_max && t1 < closest_t)
                {
                    closest_t = t1;
                    closest_sphere = sphere;
                }
                if(t2 > t_min && t2 < t_max && t2 < closest_t)
                {
                    closest_t = t2;
                    closest_sphere = sphere;
                }
            }

            return (closest_sphere, closest_t);
        }

        (float, float) IntersectRaySphere(Vector3 O, Vector3 D, Sphere sphere)
        {
            Vector3 C = sphere.center;
            float r = sphere.radius;
            Vector3 oc = O - C;

            float k1 = Vector3.Dot(D, D);
            float k2 = 2 * Vector3.Dot(oc, D);
            float K3 = Vector3.Dot(oc, oc) - r * r;

            float discriminant = k2 * k2 - 4 * k1 * K3;

            if (discriminant < 0)
                return (float.MaxValue, float.MaxValue);

            float t1 = (-k2 + (float)Math.Sqrt(discriminant)) / (2 * k1);
            float t2 = (-k2 - (float)Math.Sqrt(discriminant)) / (2 * k1);

            return (t1, t2);
        }

        Color TraceRay(Vector3 O, Vector3 D, float t_min, float t_max, float depth)
        {
            (Sphere closest_Sphere, float closest_t) = ClosestIntersection(O, D, t_min, t_max);

            if (closest_Sphere == null)
                return backgroundcolor;

            //Compute local color
            Vector3 P = O + closest_t * D; //comute intersection
            Vector3 N = P - closest_Sphere.center; //compute sphere normal at intersection
            N = N / Vector3.Length(N);

            Color local_color = closest_Sphere.color * ComputeLighting(P, N, -D, closest_Sphere.specular);

            //if we hit recursion limit or object is not reflective, we're done;
            float r = closest_Sphere.reflective;
            if (depth <= 0 || r <= 0)
                return local_color;

            //compute reflected color
            Vector3 R = ReflectRay(-D, N);
            Color reflected_color = TraceRay(P, R, 0.001f, float.MaxValue, depth - 1);

            return local_color * (1 - r) + reflected_color * r;
        }


        public void Render()
        {
            for(int x = -canvas.Width/2; x < canvas.Width / 2; x++)
            {
                for(int y = -canvas.Height / 2; y < canvas.Height / 2; y++)
                {
                    Vector3 D = CanvasToViewPort(x, y);
                    Color color = TraceRay(new Vector3(0, 0, 0), D, 1, float.MaxValue, 30);
                    //Console.Write(color.toString());
                    canvas.SetPixel(x + canvas.Width / 2, y + canvas.Height /2, System.Drawing.Color.FromArgb((int)color.r, (int)color.g, (int)color.b));
                }
            }
        }
    }

    public struct Viewport
    {
        public float width;
        public float height;
        public float distance;

        public Viewport (float _w, float _h, float _d)
        {
            width = _w;
            height = _h;
            distance = _d;
        }
    }
}
