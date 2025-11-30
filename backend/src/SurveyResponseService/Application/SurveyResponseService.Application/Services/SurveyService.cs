using SurveyResponseService.Application.Mappers;
using SurveyResponseService.Domain.DTOs.Survey;
using SurveyResponseService.Domain.Interfaces.Repositories;
using SurveyResponseService.Domain.Interfaces.Services;

namespace SurveyResponseService.Application.Services
{
    public class SurveyService : ISurveyService
    {
        private readonly ISurveyRepository _repository;
        private readonly IUserRepository _userRepository;

        public SurveyService(
            ISurveyRepository repository, 
            IUserRepository userRepository)
        {
            _repository = repository;
            _userRepository = userRepository;
        }

        public async Task<IList<SurveyDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var surveys = await _repository.GetAllAsync(cancellationToken);
            return surveys.Select(SurveyMapper.ToDto).ToList();
        }

        public async Task<SurveyDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var survey = await _repository.GetByIdAsync(id, cancellationToken);
            return survey != null ? SurveyMapper.ToDto(survey) : null;
        }

        public async Task<SurveyCreatedDto> AddAsync(CreateSurveyDto request, CancellationToken cancellationToken = default)
        {
            var author = await _userRepository.GetByIdAsync(request.AuthorGuid, cancellationToken);
            if (author == null)
            {
                throw new ArgumentException("Author not found", nameof(request.AuthorGuid));
            }

            var survey = SurveyMapper.ToEntity(request, author);
            await _repository.AddAsync(survey, cancellationToken);

            return new SurveyCreatedDto { Id = survey.Id };
        }

        public async Task<bool> UpdateAsync(UpdateSurveyDto request, CancellationToken cancellationToken = default)
        {
            var author = await _userRepository.GetByIdAsync(request.AuthorGuid, cancellationToken)
                ?? throw new ArgumentException("Author not found", nameof(request.AuthorGuid));

            var updatedSurvey = SurveyMapper.ToEntity(request, author);
            return await _repository.UpdateAsync(updatedSurvey, cancellationToken);
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _repository.DeleteAsync(id, cancellationToken);
        }
    }
}
