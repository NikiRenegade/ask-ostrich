using SurveyResponseService.Application.Mappers;
using SurveyResponseService.Domain.DTOs.Survey;
using SurveyResponseService.Domain.DTOs.SurveyResults;
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
            return results.Select(SurveyResultMapper.ToDto).ToList();
        }

        public async Task<SurveyResultDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var result = await _repository.GetByIdAsync(id, cancellationToken);
            return result != null ? SurveyResultMapper.ToDto(result) : null;
        }

        public async Task<SurveyResultCreatedDto> AddAsync(CreateSurveyResultDto request, CancellationToken cancellationToken = default)
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

            var surveyResult = SurveyResultMapper.ToEntity(request);
            surveyResult.UserId = user.Id;

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

        public async Task<IList<SurveyDto>> GetSurveysPassedByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var userReponses = await _repository.GetByUserIdAsync(userId, cancellationToken);
            var surveyIds = userReponses.Select(r => r.SurveyId).Distinct().ToList();
            
            var result = new List<SurveyDto>();
            foreach (var surveyId in surveyIds)
            {
                var survey = await _surveyRepository.GetByIdAsync(surveyId, cancellationToken);
                if (survey != null)
                {
                    result.Add(SurveyMapper.ToDto(survey));
                }
            }

            return result;
        }
    }
}
