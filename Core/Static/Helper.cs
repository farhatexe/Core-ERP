
/// <summary>
/// Custom string utility methods.
/// </summary>
public static class Helper
{
    /// <summary>
    /// Get a substring of the first N characters.
    /// </summary>
    public static string Truncate(string source, int length)
    {
        if (source.Length >= length)
        {
            source = source.Substring(0, length);
        }

        return source;
    }
}