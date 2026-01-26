using System.Text.Json.Serialization;

namespace TelegramBotService.Domain.Dto;

public record QuestionDto(
    Guid Id,
    QuestionType Type,
    string Title,
    int Order,
    string InnerText,
    List<OptionDto> Options);

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum QuestionType
{
    Text = 0,
    SingleChoice,
    MultipleChoice
}