using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;
using SurveyResponseService.Domain.Entities;

namespace SurveyResponseService.Infrastructure.EntityFramework
{
    public class SurveyResponseDbContext : DbContext
    {
        public SurveyResponseDbContext(DbContextOptions<SurveyResponseDbContext> options)
            : base(options)
        {
            Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;
        }

        public DbSet<Survey> Surveys => Set<Survey>();
        public DbSet<SurveyResult> SurveyResults => Set<SurveyResult>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Survey>().ToCollection("surveys");
            modelBuilder.Entity<Survey>().OwnsMany(s => s.Questions, question => question.OwnsMany(q => q.Options));

            modelBuilder.Entity<SurveyResult>().ToCollection("survey_results");
            modelBuilder.Entity<SurveyResult>().OwnsMany(r => r.Answers, answer => answer.WithOwner());

            modelBuilder.Entity<User>().ToCollection("users");
        }
    }
}
