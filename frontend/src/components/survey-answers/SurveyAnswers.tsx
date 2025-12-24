import React, { useState, useEffect } from 'react';
import { Box, Tabs, Tab, CircularProgress, Typography } from '@mui/material';
import { useParams } from 'react-router-dom';
import { AnswersByQuestions } from './AnswersByQuestions';
import { AnswersByUsers } from './AnswersByUsers';
import { AnswersSummary } from './AnswersSummary';
import { loadSurveyById, getSurveyResultsBySurveyId, type SurveyResultDto } from '../../services/surveyResultApi';
import type { Survey } from '../../types/Survey';

export const SurveyAnswers: React.FC = () => {
    const { id } = useParams<{ id?: string }>();
    const [activeTab, setActiveTab] = useState(0);
    const [survey, setSurvey] = useState<Survey | null>(null);
    const [surveyResults, setSurveyResults] = useState<SurveyResultDto[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const loadData = async () => {
            if (!id) {
                setLoading(false);
                return;
            }

            try {
                setLoading(true);
                const [surveyData, results] = await Promise.all([
                    loadSurveyById(id),
                    getSurveyResultsBySurveyId(id)
                ]);

                setSurvey(surveyData);
                setSurveyResults(results);
            } catch (error) {
                console.error('Failed to load data:', error);
            } finally {
                setLoading(false);
            }
        };

        loadData();
    }, [id]);

    const handleTabChange = (_event: React.SyntheticEvent, newValue: number) => {
        setActiveTab(newValue);
    };

    if (loading) {
        return (
            <Box sx={{ display: 'flex', justifyContent: 'center', p: 3 }}>
                <CircularProgress />
            </Box>
        );
    }

    if (!survey) {
        return (
            <Box>
                <Typography>Опрос не найден</Typography>
            </Box>
        );
    }

    return (
        <Box>
            <Box sx={{ borderBottom: 1, borderColor: 'divider', mb: 3 }}>
                <Tabs 
                    value={activeTab} 
                    onChange={handleTabChange}
                    sx={{
                        '& .MuiTab-root': {
                            fontSize: '1rem',
                            fontWeight: 'normal',
                            textTransform: 'none',
                        }
                    }}>
                    <Tab label="По вопросам" />
                    <Tab label="По пользователям" />
                </Tabs>
            </Box>

            {activeTab === 0 && (
                <AnswersByQuestions survey={survey} surveyResults={surveyResults} />
            )}

            {activeTab === 1 && (
                <AnswersByUsers survey={survey} surveyResults={surveyResults} />
            )}
        </Box>
    );
};

