namespace Task3
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            using (MorphingApp app = new MorphingApp())
            {
                app.Run(60.0);
            }
        }
    }
}