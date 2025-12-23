import React, { useMemo } from 'react';
import { Box, Accordion, AccordionSummary, AccordionDetails, Typography } from '@mui/material';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import { type SurveyResultDto } from '../../services/surveyResultApi';
import type { Survey } from '../../types/Survey';

interface AnswerGroup {
    answerText: string;
    count: number;
}

interface AnswersByQuestionsProps {
    survey: Survey;
    surveyResults: SurveyResultDto[];
}

export const AnswersByQuestions: React.FC<AnswersByQuestionsProps> = ({ survey, surveyResults }) => {
    const answersByQuestion = useMemo(() => {
        const grouped: Record<string, AnswerGroup[]> = {};

        survey.Questions.forEach(question => {
            const questionAnswers = surveyResults
                .flatMap(result => result.answers)
                .filter(answer => answer.questionId === question.QuestionId);

            const answerMap = new Map<string, number>();

            questionAnswers.forEach(answer => {
                let answerKey: string;
                
                if (question.Options && question.Options.length > 0) {
                    const optionTitles = answer.values.map(value => {
                        const option = question.Options.find(opt => opt.Value === value);
                        return option ? option.Title : value;
                    });
                    answerKey = optionTitles.join(', ');
                } else {
                    answerKey = answer.values.join(', ');
                }
                
                answerMap.set(answerKey, (answerMap.get(answerKey) || 0) + 1);
            });

            grouped[question.QuestionId] = Array.from(answerMap.entries()).map(([answerText, count]) => ({
                answerText,
                count
            }));
        });

        return grouped;
    }, [survey, surveyResults]);

    const sortedQuestions = [...survey.Questions].sort((a, b) => a.Order - b.Order);

    return (
        <Box>
            {sortedQuestions.map(question => {
                const answerGroups = answersByQuestion[question.QuestionId] || [];
                const totalAnswers = answerGroups.reduce((sum, group) => sum + group.count, 0);

                return (
                    <Accordion key={question.QuestionId} sx={{ mb: 2 }}>
                        <AccordionSummary expandIcon={<ExpandMoreIcon />}>
                            <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', width: '100%', pr: 2 }}>
                                <Typography variant="h6">{question.Title}</Typography>
                                <Typography variant="body2" sx={{ color: 'text.secondary', ml: 2 }}>
                                    Ответов: {totalAnswers}
                                </Typography>
                            </Box>
                        </AccordionSummary>
                        <AccordionDetails>
                            {answerGroups.length === 0 ? (
                                <Typography variant="body2" sx={{ color: 'text.secondary' }}>
                                    Нет ответов на этот вопрос
                                </Typography>
                            ) : (
                                <Box>
                                    {answerGroups.map((group, index) => (
                                        <Box key={index} sx={{ mb: 1, p: 1, bgcolor: 'background.default', borderRadius: 1 }}>
                                            <Typography variant="body1">
                                                {group.answerText || '(Пустой ответ)'}
                                            </Typography>
                                            <Typography variant="body2" sx={{ color: 'text.secondary', mt: 0.5 }}>
                                                Количество: {group.count}
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

