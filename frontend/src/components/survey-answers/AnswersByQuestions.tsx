import React, { useMemo } from 'react';
import { Box, Accordion, AccordionSummary, AccordionDetails, Typography, Divider } from '@mui/material';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import { type SurveyResultDto } from '../../services/surveyResultApi';
import type { Survey } from '../../types/Survey';

interface AnswerEntry {
    answerText: string;
    userName: string;
    date: string;
    datePassed: string;
}

interface AnswersByQuestionsProps {
    survey: Survey;
    surveyResults: SurveyResultDto[];
}

export const AnswersByQuestions: React.FC<AnswersByQuestionsProps> = ({ survey, surveyResults }) => {
    const answersByQuestion = useMemo(() => {
        const grouped: Record<string, AnswerEntry[]> = {};

        survey.Questions.forEach(question => {
            const entries: AnswerEntry[] = [];

            surveyResults.forEach(result => {
                const answer = result.answers.find(a => a.questionId === question.QuestionId);
                
                if (answer) {
                    let answerText: string;
                    
                    if (question.Options && question.Options.length > 0) {
                        const optionTitles = answer.values.map(value => {
                            const option = question.Options.find(opt => opt.Value === value);
                            return option ? option.Title : value;
                        });
                        answerText = optionTitles.join(', ') || '(Пустой ответ)';
                    } else {
                        answerText = answer.values.join(', ') || '(Пустой ответ)';
                    }

                    entries.push({
                        answerText,
                        userName: result.userName || 'Неизвестный пользователь',
                        date: result.datePassed,
                        datePassed: result.datePassed
                    });
                }
            });

            entries.sort((a, b) => 
                new Date(a.datePassed).getTime() - new Date(b.datePassed).getTime()
            );

            grouped[question.QuestionId] = entries;
        });

        return grouped;
    }, [survey, surveyResults]);

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

    const sortedQuestions = [...survey.Questions].sort((a, b) => a.Order - b.Order);

    return (
        <Box>
            {sortedQuestions.map(question => {
                const answerEntries = answersByQuestion[question.QuestionId] || [];                

                return (
                    <Accordion key={question.QuestionId} sx={{ mb: 2 }}>
                        <AccordionSummary expandIcon={<ExpandMoreIcon />}>
                            <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', width: '100%', pr: 2 }}>
                                <Typography variant="h6">{question.Title}</Typography>
                            </Box>
                        </AccordionSummary>
                        <AccordionDetails>
                            {answerEntries.length === 0 ? (
                                <Typography variant="body2" sx={{ color: 'text.secondary' }}>
                                    Нет ответов на этот вопрос
                                </Typography>
                            ) : (
                                <Box>
                                    {answerEntries.map((entry, index) => (
                                        <Box key={index} sx={{ display: 'flex', gap: 2, alignItems: 'center', py: 1 }}>
                                            <Typography variant="body1" sx={{ fontWeight: 'medium', flex: 1 }}>
                                                {entry.answerText}
                                            </Typography>
                                            <Typography variant="body2" sx={{ color: 'text.secondary' }}>
                                                {entry.userName}
                                            </Typography>
                                            <Divider orientation="vertical" flexItem />
                                            <Typography variant="body2" sx={{ color: 'text.secondary' }}>
                                                {formatDate(entry.date)}
                                            </Typography>
                                        </Box>
                                    ))}
                                </Box>
                            )}
                        </AccordionDetails>
                    </Accordion>
                );
            })}
        </Box>
    );
};

