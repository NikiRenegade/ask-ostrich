import React from "react";
import { Paper, Typography, Box, IconButton, Tooltip } from "@mui/material";
import LaunchIcon from "@mui/icons-material/Launch";
import type { SurveyResultDto } from "../../services/surveyResultApi";

interface Props {
    survey: SurveyResultDto;
}
export const PassedSurveyCard: React.FC<Props> = ({ survey }) => {
    const handleTakeSurvey = () => {
        const surveyFormUrl = `${window.location.origin}/survey-form/${survey.surveyId}`;
        window.open(surveyFormUrl, '_blank');
    };

    const correctAnswers = survey.answers.filter(a => a.isCorrect).length;
    const totalQuestions = survey.answers.length;

    return (
        <Paper sx={{ p: 2, mb: 2, display: "flex", justifyContent: "space-between", alignItems: "center" }}>
            <Box>
                <Typography variant="h6" sx={{ fontWeight: "bold" }}>
                    {survey.title}
                </Typography>
                <Typography variant="body2" sx={{ mb: 1 }}>
                    {survey.description}
                </Typography>

                <Typography variant="caption" sx={{ display: "block", opacity: 0.7 }}>
                    Пройден: {new Date(survey.datePassed).toLocaleString()}
                </Typography>

                <Typography variant="caption" sx={{ display: "block", opacity: 0.7 }}>
                    Результат: {totalQuestions > 0 ? `${(correctAnswers / totalQuestions * 100).toFixed()}%` : '0%'} ({correctAnswers} из {totalQuestions})
                </Typography>
            </Box>

            <Box sx={{ display: "flex", gap: 1 }}>
                <Tooltip title="Пройти опрос">
                    <IconButton color="primary" onClick={handleTakeSurvey}>
                        <LaunchIcon />
                    </IconButton>
                </Tooltip>
            </Box>
        </Paper>
    );
};

