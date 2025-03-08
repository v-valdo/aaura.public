namespace aaura.api.Utils;
public static class Prompts
{
    public static string DailyPrompt(string newsContent)
    {
        return $@"
        I will give you a long list of titles and descriptions of today's news.
        Filter out headlines that don't matter for the lives of normal people (for example sports, fashion, culture...).
        TRY to pick at least 25-60 headlines (but NEVER unimportant ones!).
        Dont pick same news twice.
        Then do a utilitarian analysis of the news while following these instructions:

        For each news item of actually important news:
        - Generate a sentiment score from 0 (very positive) to 100 (very negative).

        Analyze according to this scoring concept example:
        Score 100: Catastrophic Global Events
        Nuclear war, apocalyptic events, global-scale disasters causing a very high number of deaths
        Major events that result in widespread destruction, death, and irreversible damage to societies, economies, and the environment.
        These events deeply affect the global population and future generations.

        Score 90-99: Violent, International or Mass-Casualty Events
        Violent conflicts causing a high number of deaths, international wars reaching new extreme levels
        Events that may not yet reach the catastrophic scale of Score 100 but are still highly significant. These could include large-scale wars, acts of terrorism, or violent conflicts leading to substantial loss of life, destabilizing entire regions and economies.

        Score 80-89: Severe Regional Conflicts, Economic Decline, or Humanitarian Crises
        Violent events causing many deaths, rising international tensions, significant economic or social decline
        These events include large-scale regional wars, economic crises, and social unrest that dramatically impact large groups of people. These events often lead to rising unemployment, lower living standards, and increased poverty for significant portions of populations, particularly vulnerable groups.

        Score 70-79: Major Disruptions with Serious Impact on People's Lives
        Economic recessions, environmental disasters, or large-scale displacement of people
        While not always resulting in mass deaths, these events disrupt the lives of millions. The focus is on events that severely affect everyday life, such as economic crises, climate disasters, or pandemics. There's significant hardship but not necessarily catastrophic levels of violence or loss of life.

        Score 60-69: Substantial Economic or Social Impact
        Political unrest, smaller-scale conflicts, economic downturns affecting many people
        Events in this range cause noticeable economic or social pain for large populations. Unemployment rises, and poverty spreads in affected areas, but the scale is smaller compared to the previous levels.

        Score 50-59: Significant but Recoverable Issues
        Localized disasters, strikes, smaller-scale economic issues, significant protests
        While these events might affect hundreds of thousands of people, they typically don't have long-term global effects. People experience economic hardship or social unrest, but recovery is possible.

        Score 40-49: Minor Setbacks
        Regional strikes, localized economic downturns, or small-scale protests
        Events in this range represent minor economic setbacks or social protests. The effects are localized, and their impact on the broader population is limited.

        Score 30-39: Positive Developments with Short-Term Impact
        Advances in healthcare, education, or infrastructure, but localized and not global
        News that represents positive steps for improving lives but may not yet have broad or immediate global consequences. For instance, a new medical breakthrough or major infrastructural project that benefits a specific region.

        Score 20-29: Small Positive Developments with Broader Impact
        Regional economic growth or improvements in living standards
        While positive, the improvements are more limited in scope and their effect on global well-being is not yet fully realized. This could include localized advancements in health, education, or technology.

        Score 2-20: GREAT Progress Towards Stability in an Unstable Area
        Notable improvements in living standards, economic growth for normal people in struggling regions
        These events mark significant progress for a large number of people, leading to long-term positive effects on quality of life. These could be major economic reforms, stability improvements, or reductions in poverty that will bring lasting benefits.

        Score 1: Official End to War or Drastic Living Standard Improvements
        A war officially ending, dramatic increases in living standards in large populations
        News that represents a significant milestone in peace and stability, such as a war coming to a peaceful resolution or dramatic improvements in the well-being of large populations, improving opportunities for future generations.

        Remember - Base this score on actual impact on people of the world all over.
        And it's ok if no news are 100 or 0 today. It would be very unlikely (but possible) for the average final score to hit 90-100 or 0-40.

        Then give me a final, average whole-number score.
        This final score is the sum of the score of each individual news you analyzed divided by the count of news you analyzed.
        So for example, if you analyzed 5 headlines (remember I want 25-60 at least) that scored 60, 84, 65, 68, and 79, the final score you give to me will be 71.

        Keep in mind, the average score will probably never be 0 because most news are usually bad.
        Don't show me the calculation of the score under score. Only give me the final number.

        DONT use any MD formatting.
        DONT use any other under titles (like title and description).

        Consistency: Provide responses as if the AI temperature is low, prioritizing precision, determinism, and minimal creativity.
        Try to calculate according to the moral concepts of utilitarianism.

        IMPORTANT: Please follow the exact structure below, using **only** the tags as shown, without deviation. The structure is critical to match my requirements.

        |Score|
        <score in whole number>
        |EndOfScore|

        |Reasoning|
        <tell me very briefly the highest scoring and lowest scoring news>
        |EndOfReasoning|

        Here's the news you should analyze:
        {newsContent}

        Once again, dont forget to use the tags |Score|, |EndOfScore|, |Reasoning|, |EndOfReasoning|
        Otherwise my program will crash";
    }
    public static string ImagePrompt(string reasoning)
    {
        return $@"Based on this reasoning regarding today's sentiment in the world: {reasoning}.
        Generate ONE abstract image showing the positive news item on one side, the negative one on the other side.";
    }
}