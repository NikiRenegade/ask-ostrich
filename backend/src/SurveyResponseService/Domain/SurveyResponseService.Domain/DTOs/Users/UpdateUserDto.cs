﻿namespace SurveyResponseService.Domain.DTOs.Users
{
    public class UpdateUserDto
    {
        public Guid Id { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
    }
}