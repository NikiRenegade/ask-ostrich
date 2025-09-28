using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;
using SurveyManageService.Domain.Entities;

namespace SurveyManageService.Infrastructure.EntityFramework;

public class SurveyManageDbContext : DbContext
{
    public SurveyManageDbContext(DbContextOptions<SurveyManageDbContext> options)
        : base(options)
    {
        Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;
    }

    public DbSet<Survey> Surveys => Set<Survey>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Survey>().ToCollection("surveys");
        modelBuilder.Entity<Survey>().OwnsMany(s => s.Questions, question => question.OwnsMany(q => q.Options));

        modelBuilder.Entity<User>().ToCollection("users");
    }
}
