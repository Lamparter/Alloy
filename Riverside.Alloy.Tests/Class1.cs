// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Riverside.Alloy.Tests
{
    public class Class1 : IValue
    {
#if WinUI
        public static string GetMessage() => "Hello, WinUI!";
#endif
#if UWP
        public static string GetMessage() => "Hello, UWP!";
#endif
    }
}
