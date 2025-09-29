﻿using SurveyManageService.Application.Mappers;
using SurveyManageService.Domain.DTO;
using SurveyManageService.Domain.Entities;
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

        public SurveyService(ISurveyRepository repository, IUserRepository userRepository)
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

        public async Task UpdateAsync(UpdateSurveyDto request, CancellationToken cancellationToken = default)
        {
            var existingSurvey = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (existingSurvey == null)
            {
                throw new ArgumentException("Survey not found", nameof(request.Id));
            }

            var author = await _userRepository.GetByIdAsync(request.AuthorGuid, cancellationToken);
            if (author == null)
            {
                throw new ArgumentException("Author not found", nameof(request.AuthorGuid));
            }

            var updatedSurvey = SurveyMapper.ToEntity(request, author);
            await _repository.UpdateAsync(updatedSurvey, cancellationToken);
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _repository.DeleteAsync(id, cancellationToken);
        }
    }
}
