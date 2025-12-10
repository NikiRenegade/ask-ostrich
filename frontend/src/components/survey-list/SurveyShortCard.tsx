import React from "react";
import { Paper, Typography, Box, Chip, IconButton, Tooltip } from "@mui/material";
import EditIcon from "@mui/icons-material/Edit";
import DeleteIcon from "@mui/icons-material/Delete";
import LaunchIcon from "@mui/icons-material/Launch";
import type { SurveyShort } from "../../types/SurveyShort";

interface Props {
    survey: SurveyShort;
    onDelete: () => void;
    onEdit: () => void;
}
export const SurveyShortCard: React.FC<Props> = ({ survey, onDelete, onEdit }) => {
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

                <Chip
                    label={survey.isPublished ? "Опубликован" : "Черновик"}
                    color={survey.isPublished ? "success" : "default"}
                    size="small"
                    sx={{ mt: 1 }}
                />
            </Box>

            <Box sx={{ display: "flex", gap: 1 }}>
                <Tooltip title="Пройти опрос">
                    <IconButton color="primary" onClick={handleTakeSurvey}>
                        <LaunchIcon />
                    </IconButton>
                </Tooltip>

                <Tooltip title="Редактировать">
                    <IconButton color="secondary" onClick={onEdit}>
                        <EditIcon />
                    </IconButton>
                </Tooltip>

                <Tooltip title="Удалить">
                    <IconButton color="error" onClick={onDelete}>
                        <DeleteIcon />
                    </IconButton>
                </Tooltip>
            </Box>
        </Paper>
    );
};