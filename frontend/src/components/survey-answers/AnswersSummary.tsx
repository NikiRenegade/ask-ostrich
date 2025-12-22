import React from 'react';
import { Box } from '@mui/material';
import { type SurveyResultDto } from '../../services/surveyResultApi';
import type { Survey } from '../../types/Survey';

interface AnswersSummaryProps {
    survey: Survey;
    surveyResults: SurveyResultDto[];
}

export const AnswersSummary: React.FC<AnswersSummaryProps> = ({ survey, surveyResults }) => {
    return (
        <Box>
        </Box>
    );
};

