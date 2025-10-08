﻿namespace SurveyResponseService.Domain.Entities
{
    public class Question
    {
        private readonly List<Option> _options = [];

        public Guid Id { get; set; }
        public QuestionType Type { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Order { get; set; } = 0;
        public string InnerText { get; set; } = string.Empty;
        public IEnumerable<Option> Options => _options.ToList();

        public Question()
        {
            Id = Guid.NewGuid();
        }

        public Question(QuestionType type, string title, int order, string innerText)
        {
            Id = Guid.NewGuid();
            Type = type;
            Title = title;
            Order = order;
            InnerText = innerText;
        }

        public void AddOptions(List<Option> options)
        {
            if (options != null && options.Any())
            {
                _options.AddRange(options);
            }
        }
    }
}
