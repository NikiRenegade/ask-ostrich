import React from "react";
import { Paper, Typography, Box, IconButton, Tooltip } from "@mui/material";
import LaunchIcon from "@mui/icons-material/Launch";
import type { SurveyShort } from "../../types/SurveyShort";

interface Props {
    survey: SurveyShort;
}
export const PassedSurveyCard: React.FC<Props> = ({ survey }) => {
    const handleTakeSurvey = () => {
        const surveyFormUrl = `${window.location.origin}/survey-form/${survey.id}`;
        window.open(surveyFormUrl, '_blank');
    };

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
                    Вопросов: {survey.questionCount}
                </Typography>

                <Typography variant="caption" sx={{ display: "block", opacity: 0.7 }}>
                    Создан: {new Date(survey.createdAt).toLocaleString()}
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

