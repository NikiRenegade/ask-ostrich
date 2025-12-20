import React from 'react';
import { Box } from '@mui/material';
import { type SurveyResultDto } from '../../services/surveyResultApi';
import type { Survey } from '../../types/Survey';

interface AnswersByUsersProps {
    survey: Survey;
    surveyResults: SurveyResultDto[];
}

export const AnswersByUsers: React.FC<AnswersByUsersProps> = ({ survey, surveyResults }) => {
    return (
        <Box>
        </Box>
    );
};

