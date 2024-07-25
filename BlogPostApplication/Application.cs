using System.Reflection;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("BlogPostApplication.IntegrationTests")]
namespace BlogPostApplication
{
    public class Application
    {
        public static Assembly Assembly { get; } = typeof(Application).Assembly;
    }
}
