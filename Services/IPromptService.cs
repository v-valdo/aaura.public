namespace aaura.api.Services
{
    public interface IPromptService<T>
    {
        /// <summary>
        /// Generates data using AI prompts (see Utils-> Prompts), returning the type-specific result.
        /// </summary>
        /// <returns>A result of type T.</returns>

        Task<T> GenerateNewDailyData();

        /// <summary>
        /// Caching method that runs the generator
        /// </summary>
        Task<T> GetOrGenerateDailyData();
    }
}