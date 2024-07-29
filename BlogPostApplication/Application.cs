using System.Reflection;

namespace BlogPostApplication
{
    public class Application
    {
        public static Assembly Assembly { get; } = typeof(Application).Assembly;
    }
}
