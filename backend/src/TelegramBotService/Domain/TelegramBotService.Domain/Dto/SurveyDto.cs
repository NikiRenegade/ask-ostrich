namespace TelegramBotService.Domain.Dto;

 public record SurveyDto(
     Guid Id ,
     string Title ,
     string Description ,
     bool IsPublished ,
     List<QuestionDto> Questions
);