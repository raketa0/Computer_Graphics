namespace RayTracer;

public interface ISceneObject
{
    bool Intersect(Ray ray, out HitInfo hit);
}