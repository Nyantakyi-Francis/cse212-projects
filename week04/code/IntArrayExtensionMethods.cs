using System.Collections;
using System.Linq; // Required for .Cast<int>()

public static class IntArrayExtensionMethods
{
 
    public static string AsString(this IEnumerable array)
    {
      
        return "<IEnumerable>{" + string.Join(", ", array.Cast<int>()) + "}";
    }
}
