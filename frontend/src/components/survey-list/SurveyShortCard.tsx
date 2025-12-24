import React, { useState } from "react";
import { Paper, Typography, Box, Chip, IconButton, Switch, Tooltip, Snackbar, Alert } from "@mui/material";
import EditIcon from "@mui/icons-material/Edit";
import DeleteIcon from "@mui/icons-material/Delete";
import LaunchIcon from "@mui/icons-material/Launch";
import type { SurveyShort } from "../../types/SurveyShort";
import { publishSurvey } from '../../services/surveyResultApi';

interface Props {
    survey: SurveyShort;
    onDelete: () => void;
    onEdit: () => void;
    onPublishToggle?: (id: string, isPublished: boolean) => void;
}
export const SurveyShortCard: React.FC<Props> = ({ survey, onDelete, onEdit, onPublishToggle }) => {
    const [isPublished, setIsPublished] = useState(survey.isPublished);
    const [isLoading, setIsLoading] = useState(false);
    const [snackbar, setSnackbar] = useState<{
      open: boolean;
      message: string;
      severity: SnackSeverity;
    }>({
      open: false,
      message: '',
      severity: 'info'
    });

    type SnackSeverity = 'error' | 'warning' | 'info' | 'success'

    const handleTakeSurvey = () => {
        const surveyFormUrl = `${window.location.origin}/survey-form/${survey.id}`;
        window.open(surveyFormUrl, '_blank');
    };

    const showSnackbar = (message: string, severity: SnackSeverity) => {
      setSnackbar({
        open: true,
        message,
        severity
      });
    };

    const handleCloseSnackbar = () => {
      setSnackbar(prev => ({ ...prev, open: false }));
    };

    const handlePublishToggle = async (event: React.ChangeEvent<HTMLInputElement>) => {
      const newPublishedState = event.target.checked;
      
      const oldPublishedState = isPublished;
      
      setIsPublished(newPublishedState);
      setIsLoading(true);
      
      try {
        if (onPublishToggle) {
          await onPublishToggle(survey.id, newPublishedState);
        }
        
        survey.isPublished = newPublishedState;
        await publishSurvey(survey);
        
        showSnackbar(
          `Опрос "${survey.title}" ${newPublishedState ? 'опубликован' : 'снят с публикации'}`,
          'success'
        );
        
      } catch (err: unknown) {
        setIsPublished(oldPublishedState);
      
        const errorMessage = err instanceof Error 
          ? err.message 
          : "Не удалось изменить статус публикации";
      
        showSnackbar(errorMessage, 'error');
      
        console.error('Ошибка при изменении статуса публикации:', err);
      } finally {
        setIsLoading(false);
      }
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
                    label={isPublished ? "Опубликован" : "Черновик"}
                    color={isPublished ? "success" : "default"}
                    size="small"
                    sx={{ mt: 1 }}
                />
            </Box>

            <Box sx={{ display: "flex", gap: 1 }}>
                <Tooltip title={isPublished ? "Снять с публикации" : "Опубликовать"}>
                  <Box sx={{ display: 'flex', alignItems: 'center' }}>
                    <Switch
                      checked={isPublished}
                      onChange={handlePublishToggle}
                      sx={{
                          '& .MuiSwitch-switchBase:not(.Mui-checked)': {
                              color: '#9e9e9e',
                          },
                          '& .MuiSwitch-switchBase:not(.Mui-checked) + .MuiSwitch-track': {
                              backgroundColor: '#e0e0e0',
                          },
                      }}
                      inputProps={{ 'aria-label': 'публикация опроса' }}
                    />
                  </Box>
                </Tooltip>
                
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
            <Snackbar
              open={snackbar.open}
              autoHideDuration={6000}
              onClose={handleCloseSnackbar}
              anchorOrigin={{ vertical: 'bottom', horizontal: 'right' }}
            >
              <Alert 
                onClose={handleCloseSnackbar} 
                severity={snackbar.severity}
                sx={{ width: '100%' }}
              >
                {snackbar.message}
              </Alert>
            </Snackbar>
        </Paper>
    );
};