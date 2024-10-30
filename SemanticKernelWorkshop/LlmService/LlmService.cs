namespace LlmService;

public class LlmService
{
    public static (string deploymentName, string endpoint, string apiKey) LoadSettings()
    {
        var deploymentName = "gpt-4o";
        var endpoint = "https://oai-aitoday-01.openai.azure.com";
        var apiKey = "";

        return (deploymentName, endpoint, apiKey);
    }

    public static (string deploymentName, string endpoint, string apiKey) LoadEmbeddingSettings()
    {
        var deploymentName = "embedding";
        var endpoint = "https://oai-aitoday-01.openai.azure.com";
        var apiKey = "";

        return (deploymentName, endpoint, apiKey);
    }
}
