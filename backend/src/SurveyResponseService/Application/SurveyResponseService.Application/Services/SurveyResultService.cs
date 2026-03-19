using SurveyResponseService.Application.Helpers;
using SurveyResponseService.Application.Mappers;
using SurveyResponseService.Domain.DTOs.SurveyResults;
using SurveyResponseService.Domain.Entities;
using SurveyResponseService.Domain.Interfaces.Repositories;
using SurveyResponseService.Domain.Interfaces.Services;

namespace SurveyResponseService.Application.Services
{
    public class SurveyResultService : ISurveyResultService
    {
        private readonly ISurveyResultRepository _repository;
        private readonly IUserRepository _userRepository;
        private readonly ISurveyRepository _surveyRepository;

        public SurveyResultService(ISurveyResultRepository repository, IUserRepository userRepository, ISurveyRepository surveyRepository)
        {
            _repository = repository;
            _userRepository = userRepository;
            _surveyRepository = surveyRepository;
        }

        public async Task<IList<SurveyResultDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var results = await _repository.GetAllAsync(cancellationToken);
            
            if (!results.Any())
                return new List<SurveyResultDto>();

            var userIds = results.Select(r => r.UserId).Distinct();
            var surveyIds = results.Select(r => r.SurveyId).Distinct();

            var allUsers = await _userRepository.GetAllAsync(cancellationToken);
            var surveys = new Dictionary<Guid, Survey>();
            foreach (var surveyId in surveyIds)
            {
                var survey = await _surveyRepository.GetByIdAsync(surveyId, cancellationToken);
                if (survey != null)
                    surveys[surveyId] = survey;
            }

            var userDict = allUsers
                .Where(u => userIds.Contains(u.Id))
                .ToDictionary(u => u.Id);

            return results
                .Select(r =>
                {
                    var dto = SurveyResultMapper.ToDto(r);
                    
                    if (surveys.TryGetValue(r.SurveyId, out var survey))
                    {
                        dto.Title = survey.Title;
                        dto.Description = survey.Description;
                        foreach (var answer in dto.Answers)
                            answer.IsCorrect = SurveyResultCalculator.IsAnswerCorrect(survey, r, answer.QuestionId);
                    }
                    
                    if (r.UserId != null && userDict.TryGetValue(r.UserId.Value, out var user))
                    {
                        dto.UserName = user.UserName;
                        dto.Email = user.Email;
                    }
                    else
                    {
                        dto.UserName = r.DisplayName;
                    }
                    
                    return dto;
                })
                .ToList();
        }

        public async Task<SurveyResultDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var result = await _repository.GetByIdAsync(id, cancellationToken);
            if (result == null)
                return null;

            if (result.UserId == null && result.GuestId == null)
            {
                throw new ArgumentException("User not found");
            }
            var dto = SurveyResultMapper.ToDto(result);
            var survey = await _surveyRepository.GetByIdAsync(result.SurveyId, cancellationToken);
            if (result.UserId != null)
            {
                var user = await _userRepository.GetByIdAsync(result.UserId, cancellationToken);
                dto.UserName = user?.UserName ?? string.Empty;
                dto.Email = user?.Email ?? string.Empty;
            }
            if (survey != null)
            {
                dto.Title = survey.Title;
                dto.Description = survey.Description;
                foreach (var answer in dto.Answers)
                    answer.IsCorrect = SurveyResultCalculator.IsAnswerCorrect(survey, result, answer.QuestionId);
            }

            return dto;
        }

        public async Task<SurveyResultCreatedDto> AddAsync(CreateSurveyResultDto request, CancellationToken cancellationToken = default)
        {
            if (request.UserId == null && request.GuestId == null)
            {
                throw new ArgumentException("UserId or GuestId required");
            }

            var survey = await _surveyRepository.GetByIdAsync(request.SurveyId, cancellationToken);
            if (survey == null)
            {
                throw new ArgumentException("Survey not found");
            }
            var surveyResult = SurveyResultMapper.ToEntity(request);
            
            if (request.UserId != null)
            {
                var user = await _userRepository.GetByIdAsync(request.UserId.Value, cancellationToken);
                if (user == null)
                    throw new ArgumentException("User not found");

                surveyResult.UserId = user.Id;
            }
            else
            {
                surveyResult.GuestId = request.GuestId;
                surveyResult.DisplayName = request.DisplayName;
            }

            await _repository.AddAsync(surveyResult, cancellationToken);

            return new SurveyResultCreatedDto { Id = surveyResult.Id };
        }

        public async Task<bool> UpdateAsync(UpdateSurveyResultDto request, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (user == null)
            {
                throw new ArgumentException("User not found", nameof(request.UserId));
            }

            var survey = await _surveyRepository.GetByIdAsync(request.SurveyId, cancellationToken);
            if (survey == null)
            {
                throw new ArgumentException("Survey not found", nameof(request.SurveyId));
            }

            var updatedSurveyResult = SurveyResultMapper.ToEntity(request);
            updatedSurveyResult.UserId = user.Id;

            return await _repository.UpdateAsync(updatedSurveyResult, cancellationToken);
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _repository.DeleteAsync(id, cancellationToken);
        }

        public async Task<IList<SurveyResultDto>> GetPassedSurveysByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var latestResults = (await _repository.GetByUserIdAsync(userId, cancellationToken))
                .GroupBy(r => r.SurveyId)
                .Select(g => g.OrderByDescending(r => r.DatePassed).First())
                .ToList();

            if (!latestResults.Any())
            {
                return new List<SurveyResultDto>();
            }

            var surveyIds = latestResults.Select(r => r.SurveyId).Distinct();
            var surveys = new Dictionary<Guid, Survey>();
            foreach (var surveyId in surveyIds)
            {
                var survey = await _surveyRepository.GetByIdAsync(surveyId, cancellationToken);
                if (survey != null)
                    surveys[surveyId] = survey;
            }

            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

            return latestResults
                .Where(r => surveys.ContainsKey(r.SurveyId))
                .Select(r =>
                {
                    var survey = surveys[r.SurveyId];
                    var dto = SurveyResultMapper.ToDto(r);
                    dto.Title = survey.Title;
                    dto.Description = survey.Description;
                    dto.UserName = user?.UserName ?? string.Empty;
                    dto.Email = user?.Email ?? string.Empty;
                    
                    foreach (var answer in dto.Answers)
                        answer.IsCorrect = SurveyResultCalculator.IsAnswerCorrect(survey, r, answer.QuestionId);
                    
                    return dto;
                })
                .ToList();
        }

        public async Task<SurveyResultDto?> GetLatestBySurveyIdAndUserIdAsync(Guid surveyId, Guid userId, CancellationToken cancellationToken = default)
        {
            var surveyResult = (await _repository.GetByUserIdAsync(userId, cancellationToken))
                .Where(r => r.SurveyId == surveyId)
                .OrderByDescending(r => r.DatePassed)
                .FirstOrDefault();

            if (surveyResult == null)
            {
                return null;
            }

            var survey = await _surveyRepository.GetByIdAsync(surveyId, cancellationToken);
            if (survey == null)
            {
                return null;
            }

            var dto = SurveyResultMapper.ToDto(surveyResult);
            dto.Title = survey.Title;
            dto.Description = survey.Description;

            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
            dto.UserName = user?.UserName ?? string.Empty;
            dto.Email = user?.Email ?? string.Empty;

            foreach (var answer in dto.Answers)
            {
                answer.IsCorrect = SurveyResultCalculator.IsAnswerCorrect(survey, surveyResult, answer.QuestionId);
            }
            return dto;
        }

        public async Task<IList<SurveyResultDto>> GetBySurveyIdAsync(Guid surveyId, CancellationToken cancellationToken = default)
        {
            var results = await _repository.GetBySurveyIdAsync(surveyId, cancellationToken);
            if (!results.Any())
                return new List<SurveyResultDto>();

            var survey = await _surveyRepository.GetByIdAsync(surveyId, cancellationToken);
            var userIds = results.Select(r => r.UserId).Distinct();
            var allUsers = await _userRepository.GetAllAsync(cancellationToken);
            var userDict = allUsers
                .Where(u => userIds.Contains(u.Id))
                .ToDictionary(u => u.Id);

            return results
                .Select(r =>
                {
                    var dto = SurveyResultMapper.ToDto(r);
                    if (survey != null)
                    {
                        dto.Title = survey.Title;
                        dto.Description = survey.Description;
                        foreach (var answer in dto.Answers)
                            answer.IsCorrect = SurveyResultCalculator.IsAnswerCorrect(survey, r, answer.QuestionId);
                    }
                    
                    if (r.UserId != null && userDict.TryGetValue(r.UserId.Value, out var user))
                    {
                        dto.UserName = user.UserName;
                        dto.Email = user.Email;
                    }
                    else
                    {
                        dto.UserId = r.GuestId;
                        dto.UserName = r.DisplayName;
                    }
                    
                    return dto;
                })
                .ToList();
        }
    }
}
