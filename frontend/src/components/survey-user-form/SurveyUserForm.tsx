import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { 
    Box, 
    Button,
    CircularProgress,
    Alert,
    Snackbar
} from '@mui/material';
import type { Survey } from '../../types/Survey.ts';
import { loadSurveyById, submitSurveyResult, getSurveyResultBySurveyIdAndUserId, type SurveyResultResponse } from '../../services/surveyUserFormApi';
import { useAuth } from '../auth/AuthProvider.tsx';
import { SurveyViewer } from '../survey-viewer/SurveyViewer.tsx';
import { SurveyResultsView } from './SurveyResultsView.tsx';

export const SurveyUserForm: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const { user } = useAuth();
    const [survey, setSurvey] = useState<Survey | null>(null);
    const [surveyResult, setSurveyResult] = useState<SurveyResultResponse | null>(null);
    const [loading, setLoading] = useState(true);
    const [submitting, setSubmitting] = useState(false);
    const [answers, setAnswers] = useState<Record<string, string | string[]>>({});
    
    type SnackSeverity = 'error' | 'warning' | 'info' | 'success';
    const [openedSnack, setOpenedSnack] = useState<boolean>(false);
    const [snackMessage, setSnackMessage] = useState<string>();
    const [snackSeverity, setSnackSeverity] = useState<SnackSeverity>();

    const handleCloseSnack = () => {
        setOpenedSnack(false);
    };
    const showError = (msg: string) => {
        setSnackMessage(msg);
        setSnackSeverity("error");
        setOpenedSnack(true);
    };

    useEffect(() => {
        const loadData = async () => {
            if (!id) {
                showError('ID опроса не указан');
                setLoading(false);
                return;
            }

            try {
                setLoading(true);

                let result: SurveyResultResponse | null = null;
                if (user?.id) {
                    result = await getSurveyResultBySurveyIdAndUserId(id, user.id);
                    if (result) {
                        setSurveyResult(result);
                    }
                    else {
                        const loadedSurvey = await loadSurveyById(id);
                        setSurvey(loadedSurvey);
                    }
                }
            } catch (err) {
                console.error('Failed to load data:', err);
                showError(err instanceof Error ? err.message : 'Не удалось загрузить данные');
            } finally {
                setLoading(false);
            }
        };

        loadData();
    }, [id, user]);


    const handleSubmit = async () => {
        if (!survey || !id) return;

        setSubmitting(true);
        try {
            const answersArray = survey.Questions.map(question => {
                const answer = answers[question.QuestionId];
                return {
                    questionId: question.QuestionId,
                    questionTitle: question.Title,
                    values: Array.isArray(answer) ? answer : (answer ? [answer] : []),
                };
            });

            await submitSurveyResult({
                userId: user?.id,
                surveyId: id,
                datePassed: new Date().toISOString(),
                answers: answersArray,
            });
           
            if (user?.id && id) {
                try {
                    const result = await getSurveyResultBySurveyIdAndUserId(id, user.id);
                    if (result) {
                        setSurveyResult(result);
                    }
                } catch (err) {
                    console.error('Failed to load survey result:', err);
                }
            }
        } catch (err) {
            console.error('Failed to submit survey:', err);
            showError('Не удалось отправить ответы');
        } finally {
            setSubmitting(false);
        }
    };

    if (loading) {
        return (
            <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', minHeight: '400px' }}>
                <CircularProgress />
            </Box>
        );
    }

    if (!survey) {
        return null;
    }

    if (surveyResult) {
        return (
            <Box>
                <SurveyResultsView survey={survey} surveyResult={surveyResult} />
            </Box>
        );
    }
    else {
        return (
            <Box>
                <SurveyViewer 
                    survey={survey} 
                    answers={answers}
                    onAnswersChange={setAnswers}
                    disabled={submitting || (openedSnack && snackSeverity === 'success')}
                />
                
                {survey.Questions.length > 0 && (
                    <Box sx={{ display: 'flex', justifyContent: 'center', mt: 2, mb: 3 }}>
                        <Button
                            variant="contained"
                            color="primary"
                            size="large"
                            onClick={handleSubmit}
                            disabled={submitting || (openedSnack && snackSeverity === 'success')}
                            sx={{ minWidth: 200 }}
                        >
                            {submitting ? <CircularProgress size={24} /> : (openedSnack && snackSeverity === 'success') ? 'Отправлено!' : 'Отправить ответы'}
                        </Button>
                    </Box>
                )}

                <Snackbar
                    open={openedSnack}
                    autoHideDuration={3000}
                    onClose={handleCloseSnack}
                    anchorOrigin={{ vertical: 'top', horizontal: 'center' }}
                >
                    <Alert onClose={handleCloseSnack} severity={snackSeverity} variant="filled">
                        {snackMessage}
                    </Alert>
                </Snackbar>
            </Box>
        );
    }
};

