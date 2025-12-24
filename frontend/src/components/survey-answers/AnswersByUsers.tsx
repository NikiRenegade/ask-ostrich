import React, { useMemo } from 'react';
import { Box, Accordion, AccordionSummary, AccordionDetails, Typography, Paper, Chip } from '@mui/material';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import { type SurveyResultDto } from '../../services/surveyResultApi';
import type { Survey } from '../../types/Survey';

interface AnswersByUsersProps {
    survey: Survey;
    surveyResults: SurveyResultDto[];
}

interface UserResultGroup {
    userId: string;
    userName: string;
    email: string;
    results: SurveyResultDto[];
}

export const AnswersByUsers: React.FC<AnswersByUsersProps> = ({ survey, surveyResults }) => {
    const groupedByUser = useMemo(() => {
        const userMap = new Map<string, UserResultGroup>();

        surveyResults.forEach(result => {
            const key = result.userId;
            if (!userMap.has(key)) {
                userMap.set(key, {
                    userId: result.userId,
                    userName: result.userName || 'Неизвестный пользователь',
                    email: result.email || '',
                    results: []
                });
            }
            userMap.get(key)!.results.push(result);
        });

        userMap.forEach(group => {
            group.results.sort((a, b) => 
                new Date(b.datePassed).getTime() - new Date(a.datePassed).getTime()
            );
        });

        return Array.from(userMap.values()).sort((a, b) => 
            a.userName.localeCompare(b.userName, 'ru', { sensitivity: 'base' })
        );
    }, [surveyResults]);

    const getUserAnswerText = (questionId: string, result: SurveyResultDto): string => {
        const userAnswer = result.answers.find(a => a.questionId === questionId);
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

    const formatDate = (dateString: string): string => {
        const date = new Date(dateString);
        return date.toLocaleString('ru-RU', {
            year: 'numeric',
            month: 'long',
            day: 'numeric',
            hour: '2-digit',
            minute: '2-digit'
        });
    };

    if (groupedByUser.length === 0) {
        return (
            <Box sx={{ p: 3, textAlign: 'center' }}>
                <Typography variant="body1" sx={{ color: 'text.secondary' }}>
                    Нет ответов от пользователей
                </Typography>
            </Box>
        );
    }

    return (
        <Box>
            {groupedByUser.map((userGroup) => (
                <Accordion key={userGroup.userId} sx={{ mb: 2 }}>
                    <AccordionSummary
                        expandIcon={<ExpandMoreIcon />}
                        sx={{
                            '& .MuiAccordionSummary-content': {
                                alignItems: 'center',
                            }
                        }}
                    >
                        <Box sx={{ flex: 1 }}>
                            <Typography variant="h6" sx={{ fontWeight: 'medium' }}>
                                {userGroup.userName}
                            </Typography>
                            {userGroup.email && (
                                <Typography variant="body2" sx={{ color: 'text.secondary', mt: 0.5 }}>
                                    {userGroup.email}
                                </Typography>
                            )}
                        </Box>
                    </AccordionSummary>
                    <AccordionDetails>
                        <Box sx={{ display: 'flex', flexDirection: 'column', gap: 3 }}>
                            {userGroup.results.map((result, resultIndex) => (
                                <Box key={result.id || resultIndex}>
                                    <Typography 
                                        variant="subtitle1" 
                                        sx={{ 
                                            mb: 2, 
                                            fontWeight: 'medium',
                                            color: 'text.secondary',
                                            pb: 1,
                                            borderBottom: '1px solid',
                                            borderColor: 'divider'
                                        }}
                                    >
                                        Дата прохождения: {formatDate(result.datePassed)}
                                    </Typography>
                                    
                                    {[...survey.Questions].sort((a, b) => a.Order - b.Order).map((question) => {
                                        const hasCorrectOptions = question.Options.some(opt => opt.IsCorrect);
                                        const userAnswer = result.answers.find(a => a.questionId === question.QuestionId);
                                        const isCorrect = hasCorrectOptions && userAnswer ? userAnswer.isCorrect : null;
                                        const userAnswerText = getUserAnswerText(question.QuestionId, result);
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
                                                elevation={1}
                                                sx={{
                                                    p: 2,
                                                    mb: 2,
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
                                                        Ответ:
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
                            ))}
                        </Box>
                    </AccordionDetails>
                </Accordion>
            ))}
        </Box>
    );
};
