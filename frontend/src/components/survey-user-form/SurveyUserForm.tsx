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
import api from '../../services/axios';
import { useAuth } from '../auth/AuthProvider.tsx';
import { SurveyViewer } from '../survey-viewer/SurveyViewer.tsx';

export const SurveyUserForm: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const { user } = useAuth();
    const [survey, setSurvey] = useState<Survey | null>(null);
    const [loading, setLoading] = useState(true);
    const [submitting, setSubmitting] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [success, setSuccess] = useState(false);
    const [answers, setAnswers] = useState<Record<string, string | string[]>>({});

    useEffect(() => {
        const loadSurvey = async () => {
            if (!id) {
                setError('ID опроса не указан');
                setLoading(false);
                return;
            }

            try {
                setLoading(true);
                const res = await api.get(`/survey-response/api/survey/${id}`);
                const loadedSurvey = res.data;

                setSurvey({
                    SurveyId: loadedSurvey.id || id,
                    Title: loadedSurvey.title || '',
                    Description: loadedSurvey.description || '',
                    IsPublished: loadedSurvey.isPublished !== undefined ? loadedSurvey.isPublished : false,
                    AuthorGuid: loadedSurvey.authorGuid || '',
                    CreatedAt: loadedSurvey.createdAt || new Date().toISOString(),
                    ShortUrl: loadedSurvey.shortUrl || '',
                    Questions: (loadedSurvey.questions || []).map((q: any) => ({
                        QuestionId: q.questionId || '',
                        Type: (q.type || 'Text') as 'Text' | 'SingleChoice' | 'MultipleChoice',
                        Title: q.title || '',
                        Order: q.order || 1,
                        InnerText: q.innerText || '',
                        Options: (q.options || []).map((opt: any) => ({
                            Title: opt.title || '',
                            Value: opt.value || '',
                            IsCorrect: opt.isCorrect !== undefined ? opt.isCorrect : false,
                            Order: opt.order || 1,
                        })),
                    })),
                });
                setError(null);
            } catch (err: unknown) {
                console.error('Failed to load survey:', err);
                setError(err instanceof Error ? err.message : 'Не удалось загрузить опрос');
            } finally {
                setLoading(false);
            }
        };

        loadSurvey();
    }, [id]);


    const handleSubmit = async () => {
        if (!survey || !id) return;

        setSubmitting(true);
        try {
            const answersArray = survey.Questions.map(question => {
                const answer = answers[question.QuestionId];
                return {
                    QuestionId: question.QuestionId,
                    QuestionTitle: question.Title,
                    Values: Array.isArray(answer) ? answer : (answer ? [answer] : []),
                };
            });

            await api.post('/survey-response/api/SurveyResult', {
                userId: user?.id, 
                surveyId: id,
                datePassed: new Date().toISOString(),
                answers: answersArray.map(a => ({
                    questionId: a.QuestionId,
                    questionTitle: a.QuestionTitle,
                    values: a.Values,
                })),
            });

            setSuccess(true);
        } catch (err: unknown) {
            console.error('Failed to submit survey:', err);
            setError(err instanceof Error ? err.message : 'Не удалось отправить ответы');
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

    if (error && !survey) {
        return (
            <Box sx={{ p: 3 }}>
                <Alert severity="error">{error}</Alert>
            </Box>
        );
    }

    if (!survey) {
        return (
            <Box sx={{ p: 3 }}>
                <Alert severity="warning">Опрос не найден</Alert>
            </Box>
        );
    }

    return (
        <Box>
            {error && (
                <Box sx={{ p: 3 }}>
                    <Alert severity="error" onClose={() => setError(null)}>
                        {error}
                    </Alert>
                </Box>
            )}

            {survey && (
                <>
                    <SurveyViewer 
                        survey={survey} 
                        answers={answers}
                        onAnswersChange={setAnswers}
                        disabled={submitting || success}
                    />
                    
                    {survey.Questions.length > 0 && (
                        <Box sx={{ display: 'flex', justifyContent: 'center', mt: 2, mb: 3 }}>
                            <Button
                                variant="contained"
                                color="primary"
                                size="large"
                                onClick={handleSubmit}
                                disabled={submitting || success}
                                sx={{ minWidth: 200 }}
                            >
                                {submitting ? <CircularProgress size={24} /> : success ? 'Отправлено!' : 'Отправить ответы'}
                            </Button>
                        </Box>
                    )}
                </>
            )}

            <Snackbar
                open={success}
                autoHideDuration={2000}
                onClose={() => setSuccess(false)}
                anchorOrigin={{ vertical: 'top', horizontal: 'center' }}
            >
                <Alert severity="success" variant="filled">
                    Ваши ответы успешно отправлены!
                </Alert>
            </Snackbar>
        </Box>
    );
};

