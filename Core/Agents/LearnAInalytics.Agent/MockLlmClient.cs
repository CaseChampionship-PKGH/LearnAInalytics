using LearnAInalytics.Agent.Contracts.Enums;
using LearnAInalytics.Agent.Contracts.Interfaces;
using LearnAInalytics.Agent.Contracts.Models;

namespace LearnAInalytics.Agent;

/// <summary>
/// Заглушка для <see cref="ILlmClient"/>
/// </summary>
public class MockLlmClient : ILlmClient
{
    LlmVariant ILlmClient.LlmVariant => LlmVariant.Russian;

    Task<LlmResponse> ILlmClient.SendRequestAsync(LlmRequest prompt, string targetTest)
    {
        string mockResponse;

        if (targetTest == "analysis")
        {
            mockResponse = @"```json
                            {
                                ""results"": [
                                {
                                    ""userId"": ""20250801007"",
                                    ""similarityPercent"": 85,
                                    ""verdict"": ""correct"",
                                    ""comment"": ""Ответ содержит ключевые понятия, но неполный.""
                                }
                                ]
                            }
                            ```";
        }
        else if (targetTest == "report")
        {
            mockResponse = "Рекомендуется пересмотреть формулировку вопроса и добавить пояснения в материал.";
        }
        else
        {
            // На случай дополнительных запросов
            mockResponse = "{}";
        }

        return Task.FromResult(new LlmResponse()
        {
            RawResponse = mockResponse
        });
    }
}
