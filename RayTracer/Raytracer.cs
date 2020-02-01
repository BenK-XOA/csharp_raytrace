using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RayTracer.Basic_Objects;


namespace RayTracer
{
    /// <summary>
    /// The main raytracer. Construct with necessary data, then call the Render() method.
    /// Rendered image is found in the canvas bitmap.
    /// </summary>
    partial class Raytracer
    {
        /// <summary>
        /// The viewport definiton
        /// </summary>
        public Viewport v { get; private set; }
        /// <summary>
        /// The canvas we render the pixels to
        /// </summary>
        public Bitmap canvas { get; private set; }
        /// <summary>
        /// The scene containing objects and lights
        /// </summary>
        public Scene scene { get; private set; }
        /// <summary>
        /// fallback color if no object is hit
        /// </summary>
        public Color backgroundcolor { get; private set; }
        /// <summary>
        /// The camera to render from
        /// </summary>
        public Camera camera { get; private set; }

        public Viewport v;
        public Bitmap canvas;
        public Scene scene;
        public Color backgroundcolor;

        /// <summary>
        /// Constructs the raytracer
        /// </summary>
        /// <param name="cam">the camera to render from</param>
        /// <param name="_v">the viewport</param>
        /// <param name="_c">the bitmap to render to</param>
        /// <param name="_s">the scene definition</param>
        /// <param name="_bg"><the fallback color/param>
        public Raytracer(Camera cam, Viewport _v, Bitmap _c, Scene _s, Color _bg)
        {
            v = _v;
            canvas = _c;
            scene = _s;
            backgroundcolor = _bg;
            camera = cam;
        }

        /// <summary>
        /// Converts pixel coordinate to viewport coordinate
        /// </summary>
        /// <param name="x">pixel x</param>
        /// <param name="y">pixel y</param>
        /// <returns>Vector 3 pixel coordinate of pixel in viewport in worldspace/returns>
        Vector3 CanvasToViewPort(float x, float y)
        {
            return new Vector3(x * v.width / canvas.Width, y * v.height / canvas.Height, v.distance);
            
        }

        /// <summary>
        /// Reflects a vector 3 R along Vector3 normal N
        /// </summary>
        /// <param name="R">the vector to reflect</param>
        /// <param name="N">the normal to reflect along</param>
        /// <returns>the reflected vector3</returns>
        Vector3 ReflectRay(Vector3 R, Vector3 N)
        {
            return 2 * N * Vector3.Dot(N, R) - R;
        }

        /// <summary>
        /// Computes to intensity of
        /// </summary>
        /// <param name="P">Hitpoint of ray</param>
        /// <param name="N">Normal at hitpoint</param>
        /// <param name="V">Viewport worldpace coordinate of pixel</param>
        /// <param name="s">specularity of object</param>
        /// <returns></returns>
        float ComputeLighting(Vector3 P, Vector3 N, Vector3 V, float s)
        {
            double intensity = 0.0f;
            double t_max;
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
                        t_max = double.MaxValue;
                    }

                    //Shadow check
                    (Sphere shadow_sphere, double shadow_t) = ClosestIntersection(P, L, 0.001f, t_max);
                    if (shadow_sphere != null)
                        continue;

                    //diffuse
                    double n_dot_1 = Vector3.Dot(N, L);
                    if (n_dot_1 > 0)
                        intensity += light.intensity * n_dot_1 / (Vector3.Length(N) * Vector3.Length(L));

                    //Specular
                    if(s != -1)
                    {
                        Vector3 R = ReflectRay(L, N);
                        double r_dot_v = Vector3.Dot(R, V);
                        if (r_dot_v > 0)
                            intensity += light.intensity * (double)Math.Pow(r_dot_v / (Vector3.Length(R) * Vector3.Length(V)), s);
                    }
                }
            }

            return intensity;
        }

        /// <summary>
        /// Computes the closest intersected sphere
        /// </summary>
        /// <param name="O">camera position</param>
        /// <param name="D">pixel viewport worldspace coordinate</param>
        /// <param name="t_min">nearclip</param>
        /// <param name="t_max">farclip</param>
        /// <returns>Tuple with sphere if hit and distance of hit</returns>
        (Sphere, float) ClosestIntersection(Vector3 O, Vector3 D, float t_min, float t_max)
        {
            double closest_t = double.MaxValue;
            Sphere closest_sphere = null;

            foreach(Sphere sphere in scene.spheres)
            {
                (double t1, double t2) = IntersectRaySphere(O, D, sphere);
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

        /// <summary>
        /// Calculates ray intersection with sphere
        /// </summary>
        /// <param name="O">camera worldspace position</param>
        /// <param name="D">pixel viewport worldspace position</param>
        /// <param name="sphere">sphere to calculate intersection for</param>
        /// <returns>tuple containing both ray intersections with sphere</returns>
        (float, float) IntersectRaySphere(Vector3 O, Vector3 D, Sphere sphere)
        {
            Vector3 C = sphere.center;
            double r = sphere.radius;
            Vector3 oc = O - C;

            double k1 = Vector3.Dot(D, D);
            double k2 = 2 * Vector3.Dot(oc, D);
            double K3 = Vector3.Dot(oc, oc) - r * r;

            double discriminant = k2 * k2 - 4 * k1 * K3;

            if (discriminant < 0)
                return (double.MaxValue, double.MaxValue);

            double t1 = (-k2 + (double)Math.Sqrt(discriminant)) / (2 * k1);
            double t2 = (-k2 - (double)Math.Sqrt(discriminant)) / (2 * k1);

            return (t1, t2);
        }

        /// <summary>
        /// traces a ray from camera into scene and calculates pixel color
        /// </summary>
        /// <param name="O">camera position</param>
        /// <param name="D">pixel viewport worldspace coordinate</param>
        /// <param name="t_min">min hit distance</param>
        /// <param name="t_max">max hit distance</param>
        /// <param name="depth">maximum reflection trace recursion depth</param>
        /// <returns>color for pixel</returns>
        Color TraceRay(Vector3 O, Vector3 D, float t_min, float t_max, float depth)
        {
            (Sphere closest_Sphere, double closest_t) = ClosestIntersection(O, D, t_min, t_max);

            if (closest_Sphere == null)
                return backgroundcolor;

            //Compute local color
            Vector3 P = O + closest_t * D; //comute intersection
            Vector3 N = P - closest_Sphere.center; //compute sphere normal at intersection
            N = N / Vector3.Length(N);

            Color local_color = closest_Sphere.color * ComputeLighting(P, N, -D, closest_Sphere.specular);

            //if we hit recursion limit or object is not reflective, we're done;
            double r = closest_Sphere.reflective;
            if (depth <= 0 || r <= 0)
                return local_color;

            //compute reflected color
            Vector3 R = ReflectRay(-D, N);
            Color reflected_color = TraceRay(P, R, 0.001f, double.MaxValue, depth - 1);

            return local_color * (1 - r) + reflected_color * r;
        }

        /// <summary>
        /// renders the scene
        /// </summary>
        public void Render()
        {
            for(int x = -canvas.Width/2; x < canvas.Width / 2; x++)
            {
                for(int y = -canvas.Height / 2; y < canvas.Height / 2; y++)
                {
                    Vector3 D = CanvasToViewPort(x, y);
                    Color color = TraceRay(camera.position, D, 1, float.MaxValue, 30);
                    //Console.Write(color.toString());
                    canvas.SetPixel(x + canvas.Width / 2, y + canvas.Height /2, System.Drawing.Color.FromArgb((int)color.r, (int)color.g, (int)color.b));
                }
            }
        }
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

        public Viewport (double _w, double _h, double _d)
        {
            width = _w;
            height = _h;
            distance = _d;
        }
    }

    /// <summary>
    /// Struct holding data of camera
    /// </summary>
    public struct Camera
    {
        public Vector3 position;

        public Camera(Vector3 _pos)
        {
            position = _pos;
        }
    }
}
