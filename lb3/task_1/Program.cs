
namespace Program
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            using (var window = new Window.Window())
            {
                window.Run(60.0);
            }
        }
    }
}