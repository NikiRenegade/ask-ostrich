import React, { useState } from 'react';
import { Box, Paper, Typography, TextField, Radio, RadioGroup, FormControlLabel, Checkbox, FormControl, FormGroup } from '@mui/material';
import type { Survey } from '../../types/Survey.ts';

interface SurveyViewerProps {
    survey: Survey;
}

export const SurveyViewer: React.FC<SurveyViewerProps> = ({ survey }) => {
    const [answers, setAnswers] = useState<Record<string, string | string[]>>({});    

    const handleTextChange = (questionId: string, value: string) => {
        setAnswers(prev => ({ ...prev, [questionId]: value }));
    };

    const handleSingleChoiceChange = (questionId: string, value: string) => {
        setAnswers(prev => ({ ...prev, [questionId]: value }));
    };

    const handleMultipleChoiceChange = (questionId: string, optionValue: string, checked: boolean) => {
        setAnswers(prev => {
            const current = (prev[questionId] as string[]) || [];
            if (checked) {
                return { ...prev, [questionId]: [...current, optionValue] };
            } else {
                return { ...prev, [questionId]: current.filter(v => v !== optionValue) };
            }
        });
    };

    return (
        <Box sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center', p: 3, gap: 3 }}>
            <Box sx={{ maxWidth: 900, width: '100%' }}>
                <Typography variant="h3" component="h1" sx={{ mb: 2, textAlign: 'center', fontWeight: 'bold' }}>
                    {survey.Title || 'Без названия'}
                </Typography>
                {survey.Description && (
                    <Typography variant="body1" sx={{ mb: 4, textAlign: 'center', color: 'text.secondary' }}>
                        {survey.Description}
                    </Typography>
                )}
            </Box>

            {[...survey.Questions].sort((a, b) => a.Order - b.Order).map((question) => (
                <Paper
                    key={question.QuestionId}
                    elevation={2}
                    sx={{
                        p: 3,
                        maxWidth: 800,
                        width: '100%',
                    }}
                >
                    <Typography variant="h6" component="h3" sx={{ mb: 1, fontWeight: 'medium' }}>
                        {question.Title || `Вопрос ${question.Order}`}
                    </Typography>
                    {question.InnerText && (
                        <Typography variant="body2" sx={{ mb: 2, color: 'text.secondary' }}>
                            {question.InnerText}
                        </Typography>
                    )}

                    {question.Type === 'Text' && (
                        <TextField
                            fullWidth
                            multiline
                            rows={3}
                            placeholder="Введите ваш ответ..."
                            value={answers[question.QuestionId] || ''}
                            onChange={(e) => handleTextChange(question.QuestionId, e.target.value)}
                        />
                    )}

                    {question.Type === 'SingleChoice' && (
                        <FormControl component="fieldset" fullWidth>
                            <RadioGroup
                                value={answers[question.QuestionId] || ''}
                                onChange={(e) => handleSingleChoiceChange(question.QuestionId, e.target.value)}
                            >
                                {[...question.Options].sort((a, b) => a.Order - b.Order).map((option) => (
                                    <FormControlLabel
                                        key={option.Value}
                                        value={option.Value}
                                        control={<Radio />}
                                        label={option.Title}
                                    />
                                ))}
                            </RadioGroup>
                        </FormControl>
                    )}

                    {question.Type === 'MultipleChoice' && (
                        <FormControl component="fieldset" fullWidth>
                            <FormGroup>
                                {[...question.Options].sort((a, b) => a.Order - b.Order).map((option) => (
                                    <FormControlLabel
                                        key={option.Value}
                                        control={
                                            <Checkbox
                                                checked={(answers[question.QuestionId] as string[] || []).includes(option.Value)}
                                                onChange={(e) => handleMultipleChoiceChange(question.QuestionId, option.Value, e.target.checked)}
                                            />
                                        }
                                        label={option.Title}
                                    />
                                ))}
                            </FormGroup>
                        </FormControl>
                    )}
                </Paper>
            ))}

            {survey.Questions.length === 0 && (
                <Paper sx={{ p: 3, maxWidth: 800, width: '100%', textAlign: 'center' }}>
                    <Typography color="text.secondary">В опросе пока нет вопросов</Typography>
                </Paper>
            )}
        </Box>
    );
};

