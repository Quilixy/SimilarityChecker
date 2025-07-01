namespace api.Helpers;

public class JaroWinklerDistanceHelper
{
    public static double CalculateJaroWinkler(string s1, string s2)
    {
        if (s1 == s2)
            return 1.0;

        int len1 = s1.Length;
        int len2 = s2.Length;

        if (len1 == 0 || len2 == 0)
            return 0.0;

        int matchDistance = Math.Max(len1, len2) / 2 - 1;

        bool[] s1Matches = new bool[len1];
        bool[] s2Matches = new bool[len2];

        int matches = 0;
        int transpositions = 0;
        
        for (int i = 0; i < len1; i++)
        {
            int start = Math.Max(0, i - matchDistance);
            int end = Math.Min(i + matchDistance + 1, len2);

            for (int j = start; j < end; j++)
            {
                if (s2Matches[j]) continue;
                if (s1[i] != s2[j]) continue;
                s1Matches[i] = true;
                s2Matches[j] = true;
                matches++;
                break;
            }
        }

        if (matches == 0)
            return 0.0;
        
        int k = 0;
        for (int i = 0; i < len1; i++)
        {
            if (!s1Matches[i]) continue;
            while (!s2Matches[k]) k++;
            if (s1[i] != s2[k])
                transpositions++;
            k++;
        }

        transpositions /= 2;
        
        double jaro = (matches / (double)len1 +
                       matches / (double)len2 +
                       (matches - transpositions) / (double)matches) / 3.0;
        
        int prefixLength = 0;
        for (int i = 0; i < Math.Min(4, Math.Min(s1.Length, s2.Length)); i++)
        {
            if (s1[i] == s2[i])
                prefixLength++;
            else
                break;
        }

        double scalingFactor = 0.1;
        return jaro + (prefixLength * scalingFactor * (1 - jaro));
    }
}