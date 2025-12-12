import React from 'react';
import { Box, Typography, Paper, Chip } from '@mui/material';
import type { Survey } from '../../types/Survey';
import type { SurveyResultResponse } from '../../services/surveyUserFormApi';

interface Props {
    survey: Survey;
    surveyResult: SurveyResultResponse;
}

export const SurveyResultsView: React.FC<Props> = ({ survey, surveyResult }) => {
    const correctAnswers = surveyResult.answers.filter(a => a.isCorrect).length;
    const score = {
        correct: correctAnswers,
        total: surveyResult.totalQuestions,
        percentage: surveyResult.totalQuestions > 0 ? (correctAnswers / surveyResult.totalQuestions * 100) : 0
    };


    const getUserAnswerText = (questionId: string): string => {
        const userAnswer = surveyResult.answers.find(a => a.questionId === questionId);
        if (!userAnswer || !userAnswer.values || userAnswer.values.length === 0) {
            return 'Нет ответа';
        }

        const question = survey.Questions.find(q => q.QuestionId === questionId);
        if (!question) return userAnswer.values.join(', ');

        const answerTexts = userAnswer.values.map(value => {
            const option = question.Options.find(opt => opt.Value === value);
            return option ? option.Title : value;
        });

        return answerTexts.join(', ');
    };

    const getCorrectAnswerText = (questionId: string): string => {
        const question = survey.Questions.find(q => q.QuestionId === questionId);
        if (!question) return '';

        const hasCorrectOptions = question.Options.some(opt => opt.IsCorrect);
        if (!hasCorrectOptions) {
            return '';
        }

        const correctOptions = question.Options.filter(opt => opt.IsCorrect);
        return correctOptions.map(opt => opt.Title).join(', ');
    };

    return (
        <Box sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center', p: 3, gap: 3 }}>
            <Box sx={{ maxWidth: 900, width: '100%' }}>
                <Typography variant="h3" component="h1" sx={{ mb: 2, textAlign: 'center', fontWeight: 'bold' }}>
                    {survey.Title || 'Без названия'}
                </Typography>
                {survey.Description && (
                    <Typography variant="body1" sx={{ mb: 4, textAlign: 'center', color: 'text.secondary' }}>
                        {survey.Description}
                    </Typography>
                )}

                <Box sx={{ mb: 4, textAlign: 'center' }}>
                    <Typography variant="h5" sx={{ mb: 1, fontWeight: 'bold' }}>
                        Результат: {score.percentage.toFixed(0)}%
                    </Typography>
                    <Typography variant="body2" sx={{ color: 'text.secondary' }}>
                        Ответы: {score.correct} из {score.total}
                    </Typography>
                </Box>
            </Box>

            {[...survey.Questions].sort((a, b) => a.Order - b.Order).map((question) => {
                const hasCorrectOptions = question.Options.some(opt => opt.IsCorrect);
                const userAnswer = surveyResult.answers.find(a => a.questionId === question.QuestionId);
                const isCorrect = hasCorrectOptions && userAnswer ? userAnswer.isCorrect : null;
                const userAnswerText = getUserAnswerText(question.QuestionId);
                const correctAnswerText = hasCorrectOptions ? getCorrectAnswerText(question.QuestionId) : '';

                const borderColor = hasCorrectOptions 
                    ? (isCorrect ? '#4caf50' : '#ff9800')
                    : '#2196f3'; 
                const backgroundColor = hasCorrectOptions
                    ? (isCorrect ? 'rgba(76, 175, 80, 0.05)' : 'rgba(255, 152, 0, 0.05)')
                    : 'rgba(33, 150, 243, 0.05)';
                const answerBackgroundColor = hasCorrectOptions
                    ? (isCorrect ? 'rgba(76, 175, 80, 0.1)' : 'rgba(255, 152, 0, 0.1)')
                    : 'rgba(33, 150, 243, 0.1)'; 
                const answerTextColor = hasCorrectOptions
                    ? (isCorrect ? '#2e7d32' : '#e65100')
                    : '#1565c0'; 

                return (
                    <Paper
                        key={question.QuestionId}
                        elevation={2}
                        sx={{
                            p: 3,
                            maxWidth: 800,
                            width: '100%',
                            borderLeft: `4px solid ${borderColor}`,
                            backgroundColor: backgroundColor,
                        }}
                    >
                        <Box sx={{ display: 'flex', alignItems: 'center', gap: 1, mb: 1 }}>
                            <Typography variant="h6" component="h3" sx={{ fontWeight: 'medium', flex: 1 }}>
                                {question.Title || `Вопрос ${question.Order}`}
                            </Typography>
                            {hasCorrectOptions && (
                                <Chip
                                    label={isCorrect ? 'Правильно' : 'Неправильно'}
                                    color={isCorrect ? 'success' : 'warning'}
                                    size="small"
                                />
                            )}
                        </Box>
                        {question.InnerText && (
                            <Typography variant="body2" sx={{ mb: 2, color: 'text.secondary' }}>
                                {question.InnerText}
                            </Typography>
                        )}

                        <Box sx={{ mt: 2 }}>
                            <Typography variant="body2" sx={{ mb: 1, fontWeight: 'medium' }}>
                                Ваш ответ:
                            </Typography>
                            <Typography
                                variant="body1"
                                sx={{
                                    mb: 2,
                                    p: 1,
                                    backgroundColor: answerBackgroundColor,
                                    borderRadius: 1,
                                    color: answerTextColor,
                                }}
                            >
                                {userAnswerText}
                            </Typography>

                            {hasCorrectOptions && !isCorrect && correctAnswerText && (
                                <>
                                    <Typography variant="body2" sx={{ mb: 1, fontWeight: 'medium' }}>
                                        Правильный ответ:
                                    </Typography>
                                    <Typography
                                        variant="body1"
                                        sx={{
                                            p: 1,
                                            backgroundColor: 'rgba(76, 175, 80, 0.1)',
                                            borderRadius: 1,
                                            color: '#2e7d32',
                                        }}
                                    >
                                        {correctAnswerText}
                                    </Typography>
                                </>
                            )}
                        </Box>
                    </Paper>
                );
            })}
        </Box>
    );
};

