using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracer.Basic_Objects;

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

   

}
