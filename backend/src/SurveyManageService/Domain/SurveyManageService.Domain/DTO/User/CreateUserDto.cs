﻿namespace SurveyManageService.Domain.DTO;

public class CreateUserDto 
{
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
}