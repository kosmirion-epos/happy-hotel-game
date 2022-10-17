public static class BoolExtensions
{
    public static int ToInt(this bool b) => b ? 1 : 0;

    // In C# 0 = false, alles was nicht 0 ist, ist true

    public static bool ToBool(this float f) => f != 0;
    public static bool ToBool(this int i) => i != 0;
}