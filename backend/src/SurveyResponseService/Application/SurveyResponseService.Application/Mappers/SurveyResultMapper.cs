using SurveyResponseService.Domain.DTOs.SurveyResults;
using SurveyResponseService.Domain.Entities;

namespace SurveyResponseService.Application.Mappers
{
    public static class SurveyResultMapper
    {
        public static SurveyResultDto ToDto(SurveyResult entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            return new SurveyResultDto
            {
                Id = entity.Id,
                UserId = entity.UserId,
                SurveyId = entity.SurveyId,
                DatePassed = entity.DatePassed,
                Answers = entity.Answers?.Select(ToDto).ToList() ?? new List<AnswerDto>()
            };
        }

        private static AnswerDto ToDto(Answer entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            return new AnswerDto
            {
                QuestionId = entity.QuestionId,
                QuestionTitle = entity.QuestionTitle,
                Values = entity.Values ?? new List<string>()
            };
        }

        public static SurveyResult ToEntity(CreateSurveyResultDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var surveyResult = new SurveyResult
            {
                Id = Guid.NewGuid(),
                UserId = dto.UserId,
                SurveyId = dto.SurveyId,
                DatePassed = dto.DatePassed == default ? DateTime.UtcNow : dto.DatePassed,
            };

            if (dto.Answers != null && dto.Answers.Any())
            {
                surveyResult.AddAnswers(dto.Answers.Select(ToEntity).ToList());
            }

            return surveyResult;
        }

        public static SurveyResult ToEntity(UpdateSurveyResultDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var surveyResult = new SurveyResult
            {
                Id = dto.Id,
                UserId = dto.UserId,
                SurveyId = dto.SurveyId,
                DatePassed = dto.DatePassed,
            };

            if (dto.Answers != null && dto.Answers.Any())
            {
                surveyResult.AddAnswers(dto.Answers.Select(ToEntity).ToList());
            }

            return surveyResult;
        }

        private static Answer ToEntity(AnswerDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            return new Answer
            {
                QuestionId = dto.QuestionId,
                QuestionTitle = dto.QuestionTitle,
                Values = dto.Values ?? new List<string>()
            };
        }
    }
}
