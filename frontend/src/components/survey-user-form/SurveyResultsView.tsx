import React from 'react';
import { Box, Typography } from '@mui/material';
import type { Survey } from '../../types/Survey';
import type { SurveyResultDto } from '../../services/surveyResultApi';
import { SurveyAnswerItem } from '../survey-answer-item/SurveyAnswerItem';

interface Props {
    survey: Survey;
    surveyResult: SurveyResultDto;
}

export const SurveyResultsView: React.FC<Props> = ({ survey, surveyResult }) => {
    const correctAnswers = surveyResult.answers.filter(a => a.isCorrect).length;
    const totalQuestions = survey.Questions.length;
    const score = {
        correct: correctAnswers,
        total: totalQuestions,
        percentage: totalQuestions > 0 ? (correctAnswers / totalQuestions * 100) : 0
    };



    return (
        <Box sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center', p: 2, gap: 2 }}>
            <Box sx={{ maxWidth: 900, width: '100%' }}>
                <Typography variant="h3" component="h1" sx={{ mb: 2, textAlign: 'center', fontWeight: 'bold' }}>
                    {survey.Title || 'Без названия'}
                </Typography>
                {survey.Description && (
                    <Typography variant="body1" sx={{ mb: 2, textAlign: 'center', color: 'text.secondary' }}>
                        {survey.Description}
                    </Typography>
                )}

                <Box sx={{ mb: 2, textAlign: 'center' }}>
                    <Typography variant="h5" sx={{ mb: 1, fontWeight: 'bold' }}>
                        Результат: {score.percentage.toFixed(0)}%
                    </Typography>
                    <Typography variant="body2" sx={{ color: 'text.secondary' }}>
                        Ответы: {score.correct} из {score.total}
                    </Typography>
                </Box>
            </Box>

            {[...survey.Questions].sort((a, b) => a.Order - b.Order).map((question) => (
                <SurveyAnswerItem
                    key={question.QuestionId}
                    question={question}
                    result={surveyResult}
                    answerLabel="Ваш ответ:"
                    elevation={2}
                    maxWidth={800}
                    padding={2}
                />
            ))}
        </Box>
    );
};

