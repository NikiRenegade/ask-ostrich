using SurveyManageService.Application.Mappers;
using SurveyManageService.Domain.DTO;
using SurveyManageService.Domain.Entities;
using SurveyManageService.Domain.Interfaces.Publishers;
using SurveyManageService.Domain.Interfaces.Repositories;
using SurveyManageService.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyManageService.Application.Services
{
    public class SurveyService: ISurveyService
    {
        private readonly ISurveyRepository _repository;
        private readonly IUserRepository _userRepository;
        private readonly ISurveyEventPublisher _surveyEventPublisher;

        public SurveyService(
            ISurveyRepository repository, 
            IUserRepository userRepository,
            ISurveyEventPublisher surveyEventPublisher)
        {
            _repository = repository;
            _userRepository = userRepository;
            _surveyEventPublisher = surveyEventPublisher;
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
            var author = await _userRepository.GetByIdAsync(request.AuthorGuid, cancellationToken) 
                ?? throw new ArgumentException("Author not found", nameof(request.AuthorGuid));

            var survey = SurveyMapper.ToEntity(request, author);
            await _repository.AddAsync(survey, cancellationToken);

            await _surveyEventPublisher.PublishSurveyCreated(survey.ToSurveyCreatedEvent());

            return new SurveyCreatedDto { Id = survey.Id };
        }

        public async Task<bool> UpdateAsync(UpdateSurveyDto request, CancellationToken cancellationToken = default)
        {
            var author = await _userRepository.GetByIdAsync(request.AuthorGuid, cancellationToken)
                ?? throw new ArgumentException("Author not found", nameof(request.AuthorGuid));
            var existingSurvey = await _repository.GetByIdAsync(request.Id, cancellationToken)
                ?? throw new ArgumentException("Survey not found", nameof(request.Id));

            var updatedSurvey = SurveyMapper.ToEntity(request, author);
            bool isUpdated = await _repository.UpdateAsync(updatedSurvey, cancellationToken);

            if (isUpdated)
                await _surveyEventPublisher.PublishSurveyUpdated(
                    updatedSurvey.ToSurveyUpdatedEvent(existingSurvey));

            return isUpdated;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            bool isDeleted = await _repository.DeleteAsync(id, cancellationToken);

            if (isDeleted)
                await _surveyEventPublisher.PublishSurveyDeleted(id);

            return isDeleted;
        }

        public async Task<IList<SurveyShortDto>> GetExistingByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            var surveys = await _repository.GetExistingByUserIdAsync(userId, cancellationToken);
            return surveys.Select(SurveyMapper.ToShortDto).ToList();
        }
    }
}
