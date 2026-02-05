import React, { useState, useEffect } from 'react';
import { Box, Tabs, Tab, CircularProgress, Typography } from '@mui/material';
import { useParams } from 'react-router-dom';
import { AnswersByQuestions } from './AnswersByQuestions';
import { AnswersByUsers } from './AnswersByUsers';
import { AnswersSummary } from './AnswersSummary';
import { surveyHub } from '../../services/surveyHubClient';
import { type SurveyResultDto, loadSurveyById } from '../../services/surveyResultApi';
import type { Survey } from '../../types/Survey';

export const SurveyAnswers: React.FC = () => {
    const { id } = useParams<{ id?: string }>();
    const [activeTab, setActiveTab] = useState(0);
    const [survey, setSurvey] = useState<Survey | null>(null);
    const [surveyResults, setSurveyResults] = useState<SurveyResultDto[]>([]);
    const [loading, setLoading] = useState(true);
    
    useEffect(() => {
        if (!id) return;
        let isMounted = true;

        const listener = {
            onEvent: (event: any) => {
                if (!isMounted) return;
                if (event.type === "ResultsUpdated") {
                    setSurveyResults(prev => {
                        const updated = [...prev];
                        event.data.forEach((newResult: SurveyResultDto) => {
                            const index = updated.findIndex(r => r.id === newResult.id);
                            if (index >= 0) updated[index] = newResult;
                            else updated.push(newResult);
                        });
                        return updated;
                    });
                }
            }
        };

        const init = async () => {
            try {
                setLoading(true);
                const surveyData = await loadSurveyById(id);
                if (!isMounted) return;
                setSurvey(surveyData);

                await surveyHub.connect();
                surveyHub.subscribe(listener);
                await surveyHub.joinSurvey(id);

            } catch (error) {
                console.error(error);
            } finally {
                if (isMounted) setLoading(false);
            }
        };

        init();

        return () => {
            isMounted = false;
            surveyHub.unsubscribe(listener);
            surveyHub.leaveSurvey(id);
        };
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
                    <Tab label="Сводка" />
                    <Tab label="По вопросам" />
                    <Tab label="По пользователям" />
                </Tabs>
            </Box>

            {activeTab === 0 && (
                <AnswersSummary survey={survey} surveyResults={surveyResults} />
            )}

            {activeTab === 1 && (
                <AnswersByQuestions survey={survey} surveyResults={surveyResults} />
            )}

            {activeTab === 2 && (
                <AnswersByUsers survey={survey} surveyResults={surveyResults} />
            )}
        </Box>
    );
};

