using System.Text.RegularExpressions;

namespace aaura.api.Utils;

public class Extractor
{
    public string ExtractReasoning(string response)
    {
        int startIndex = response.IndexOf("|Reasoning|") + "|Reasoning|".Length;
        if (startIndex == -1) return string.Empty;

        int endIndex = response.IndexOf("|EndOfReasoning|", startIndex);
        if (endIndex == -1) return string.Empty;

        string reasoning = response[startIndex..endIndex].Trim();

        return reasoning;
    }

    public int ExtractMoodScore(string response)
    {
        int startIndex = response.IndexOf("|Score|") + "|Score|".Length;
        if (startIndex == -1) return 0;

        int endIndex = response.IndexOf("|EndOfScore|", startIndex);
        if (endIndex == -1) return 0;

        string scorePart = response[startIndex..endIndex].Trim();

        if (int.TryParse(scorePart, out int score))
        {
            return score;
        }

        return 0;
    }
}