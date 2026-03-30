using OpenTK;

namespace Aquarium.Primitives
{
    interface IPrimitive
    {
        void Draw();
        void Move(Vector2 delta);
        List<Vector2> GetPosition();
    }
}