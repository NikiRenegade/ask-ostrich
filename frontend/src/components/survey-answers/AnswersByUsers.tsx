import React, { useMemo } from 'react';
import { Box, Accordion, AccordionSummary, AccordionDetails, Typography } from '@mui/material';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import { type SurveyResultDto } from '../../services/surveyResultApi';
import type { Survey } from '../../types/Survey';
import { SurveyAnswerItem } from '../survey-answer-item/SurveyAnswerItem';

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
            <Box sx={{ p: 2, textAlign: 'center' }}>
                <Typography variant="body1" sx={{ color: 'text.secondary' }}>
                    Нет ответов от пользователей
                </Typography>
            </Box>
        );
    }

    return (
        <Box>
            {groupedByUser.map((userGroup) => {
                const latestResult = userGroup.results[0];
                const correctAnswers = latestResult.answers.filter(a => a.isCorrect).length;
                const totalQuestions = latestResult.answers.length;
                
                return (
                    <Accordion key={userGroup.userId} sx={{ mb: 2 }}>
                        <AccordionSummary
                            expandIcon={<ExpandMoreIcon />}
                            sx={{
                                '& .MuiAccordionSummary-content': {
                                    alignItems: 'center',
                                }
                            }}
                        >
                            <Box sx={{ flex: 1, display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                                <Box>
                                    <Typography variant="h6" sx={{ fontWeight: 'medium' }}>
                                        {userGroup.userName}
                                    </Typography>
                                    {userGroup.email && (
                                        <Typography variant="body2" sx={{ color: 'text.secondary', mt: 0.5 }}>
                                            {userGroup.email}
                                        </Typography>
                                    )}
                                </Box>
                                <Typography variant="body2" sx={{ color: 'text.secondary', ml: 2 }}>
                                    {totalQuestions > 0 ? `${(correctAnswers / totalQuestions * 100).toFixed()}%` : '0%'} ({correctAnswers} из {totalQuestions})
                                </Typography>
                                
                            </Box>
                        </AccordionSummary>
                    <AccordionDetails>
                        <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
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
                                    
                                    {[...survey.Questions].sort((a, b) => a.Order - b.Order).map((question) => (
                                        <SurveyAnswerItem
                                            key={question.QuestionId}
                                            question={question}
                                            result={result}
                                            answerLabel="Ответ:"
                                            elevation={1}
                                            padding={2}
                                        />
                                    ))}
                                </Box>
                            ))}
                        </Box>
                    </AccordionDetails>
                </Accordion>
                );
            })}
        </Box>
    );
};
