import React from 'react';
import { Box, Typography, Paper, Chip } from '@mui/material';
import type { Question } from '../../types/Question';
import type { SurveyResultDto } from '../../services/surveyResultApi';

interface SurveyAnswerItemProps {
    question: Question;
    result: SurveyResultDto;
    answerLabel?: string;
    elevation?: number;
    maxWidth?: number | string;
    padding?: number;
}

export const SurveyAnswerItem: React.FC<SurveyAnswerItemProps> = ({
    question,
    result,
    answerLabel = 'Ответ:',
    elevation = 1,
    maxWidth,
    padding = 2
}) => {
    const hasCorrectOptions = question.Options.some(opt => opt.IsCorrect);
    const userAnswer = result.answers.find(a => a.questionId === question.QuestionId);
    const isCorrect = hasCorrectOptions && userAnswer ? userAnswer.isCorrect : null;

    const getUserAnswerText = (): string => {
        if (!userAnswer || !userAnswer.values || userAnswer.values.length === 0) {
            return 'Нет ответа';
        }

        const answerTexts = userAnswer.values.map(value => {
            const option = question.Options.find(opt => opt.Value === value);
            return option ? option.Title : value;
        });

        return answerTexts.join(', ');
    };

    const getCorrectAnswerText = (): string => {
        if (!hasCorrectOptions) {
            return '';
        }

        const correctOptions = question.Options.filter(opt => opt.IsCorrect);
        return correctOptions.map(opt => opt.Title).join(', ');
    };

    const userAnswerText = getUserAnswerText();
    const correctAnswerText = getCorrectAnswerText();

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
            elevation={elevation}
            sx={{
                p: padding,
                mb: 2,
                ...(maxWidth && { maxWidth, width: '100%' }),
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
                    {answerLabel}
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
};

