using OpenTK;

public class MousePicker
{
    public Ray GetRay(Vector2 mouse, Camera cam, int w, int h)
    {
        var proj = cam.GetProjection(w, h);
        var view = cam.GetView();

        Vector4 clip = new Vector4(
            2f * mouse.X / w - 1f,
            1f - 2f * mouse.Y / h,
            -1f,
            1f);

        Matrix4 inv = Matrix4.Invert(view * proj);
        Vector4 world = Vector4.Transform(clip, inv);
        world /= world.W;

        Vector3 origin = cam.Position;
        Vector3 dir = Vector3.Normalize(world.Xyz - origin);

        return new Ray(origin, dir);
    }

    public Vector3 IntersectPlane(Ray r)
    {
        float t = -r.Origin.Y / r.Direction.Y;
        return r.Origin + r.Direction * t;
    }

    public Tile Pick(Board b, Vector3 p)
    {
        foreach (var t in b.Tiles)
        {
            if (System.Math.Abs(p.X - t.Position.X) < 0.6f &&
                System.Math.Abs(p.Z - t.Position.Z) < 0.6f)
            {
                return t;
            }
        }
        return null;
    }
}