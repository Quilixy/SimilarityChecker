using System.Text.RegularExpressions;

namespace api.Helpers;

public class HybridPhoneticHelper
{
    public static string NormalizePhonetic(string input)
    {
        input = input.ToLowerInvariant();

        Dictionary<string, string> phoneticMap = new()
        {
            {"ç", "c"}, {"ş", "s"}, {"ğ", "g"}, {"ı", "i"}, {"ö", "o"}, {"ü", "u"},
            {"ph", "f"}, {"gh", "g"}, {"kn", "n"}, {"wr", "r"},
            {"ck", "k"}, {"qu", "k"}, {"x", "ks"}, {"w", "v"},
        };

        foreach (var kv in phoneticMap)
            input = input.Replace(kv.Key, kv.Value);

        return Regex.Replace(input, @"[^a-z]", "");
    }
}