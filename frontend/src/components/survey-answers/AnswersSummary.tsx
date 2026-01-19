import React, { useMemo } from 'react';
import { Box, Typography, Paper } from '@mui/material';
import { BarChart, Bar, XAxis, YAxis, CartesianGrid, ResponsiveContainer, LabelList, Cell } from 'recharts';
import { type SurveyResultDto } from '../../services/surveyResultApi';
import type { Survey } from '../../types/Survey';

interface AnswersSummaryProps {
    survey: Survey;
    surveyResults: SurveyResultDto[];
}

export const AnswersSummary: React.FC<AnswersSummaryProps> = ({ survey, surveyResults }) => {
    const overallStats = useMemo(() => {
        const correctAnswersCount = new Map<number, number>();
        const totalQuestions = survey.Questions.length;

        surveyResults.forEach(result => {
            const correctCount = result.answers.filter(answer => answer.isCorrect).length;
            correctAnswersCount.set(correctCount, (correctAnswersCount.get(correctCount) || 0) + 1);
        });

        const data = [];
        let maxCount = 0;
        for (let i = 0; i <= totalQuestions; i++) {
            const count = correctAnswersCount.get(i) || 0;
            maxCount = Math.max(maxCount, count);
            data.push({
                name: i.toString(),
                value: count,
                fill: '#2196f3'
            });
        }

        return { data, maxCount };
    }, [surveyResults, survey]);

    const questionCharts = useMemo(() => {
        const sortedQuestions = [...survey.Questions].sort((a, b) => a.Order - b.Order);
        
        return sortedQuestions.map(question => {
            const questionAnswers = surveyResults
                .flatMap(result => result.answers)
                .filter(answer => answer.questionId === question.QuestionId);

            if (question.Type === 'Text') {
                const textAnswerCounts = new Map<string, number>();
                
                questionAnswers.forEach(answer => {
                    const answerText = answer.values.join(', ') || '(Пустой ответ)';
                    textAnswerCounts.set(answerText, (textAnswerCounts.get(answerText) || 0) + 1);
                });

                const data = Array.from(textAnswerCounts.entries())
                    .map(([text, count]) => ({
                        name: text,
                        value: count,
                        fill: '#2196f3'
                    }))
                    .sort((a, b) => b.value - a.value);

                const maxCount = Math.max(...data.map(d => d.value), 0);

                return {
                    question,
                    data,
                    maxCount
                };
            } else {
                const optionCounts = new Map<string, { title: string; count: number; isCorrect: boolean }>();

                question.Options?.forEach(option => {
                    optionCounts.set(option.Value, {
                        title: option.Title,
                        count: 0,
                        isCorrect: option.IsCorrect
                    });
                });

                questionAnswers.forEach(answer => {
                    answer.values.forEach(value => {
                        const option = question.Options?.find(opt => opt.Value === value);
                        if (option) {
                            const existing = optionCounts.get(value);
                            if (existing) {
                                existing.count++;
                            }
                        }
                    });
                });

                const data = Array.from(optionCounts.values())
                    .map(option => ({
                        name: option.title,
                        value: option.count,
                        isCorrect: option.isCorrect,
                        fill: option.isCorrect ? '#4caf50' : '#2196f3'
                    }))
                    .sort((a, b) => b.value - a.value);

                const maxCount = Math.max(...data.map(d => d.value), 0);

                return {
                    question,
                    data,
                    maxCount
                };
            }
        });
    }, [survey, surveyResults]);

    if (surveyResults.length === 0) {
        return (
            <Box sx={{ p: 3, textAlign: 'center' }}>
                <Typography variant="body1" sx={{ color: 'text.secondary' }}>
                    Нет ответов для отображения
                </Typography>
            </Box>
        );
    }

    return (
        <Box sx={{ p: 3 }}>
            <Paper elevation={2} sx={{ p: 3, mb: 4 }}>
                <Typography variant="h5" sx={{ mb: 3, fontWeight: 'bold' }}>
                    Общая статистика
                </Typography>
                <ResponsiveContainer width="100%" height={300}>
                    <BarChart data={overallStats.data} margin={{ top: 20, right: 30, left: 20, bottom: 5 }}>
                        <CartesianGrid strokeDasharray="3 3" />
                        <XAxis 
                            dataKey="name" 
                            label={{ value: 'Количество правильных ответов', position: 'insideBottom', offset: -5 }}
                        />
                        <YAxis 
                            label={{ value: 'Количество ответов', angle: -90, position: 'insideLeft' }}
                            domain={[0, overallStats.maxCount]}
                            allowDecimals={false}
                            ticks={Array.from({ length: overallStats.maxCount + 1 }, (_, i) => i)}
                        />
                        <Bar dataKey="value" radius={[4, 4, 0, 0]}>
                            {overallStats.data.map((entry, index) => (
                                <Cell key={`cell-${index}`} fill={entry.fill} />
                            ))}
                            <LabelList dataKey="value" position="top" />
                        </Bar>
                    </BarChart>
                </ResponsiveContainer>
            </Paper>

            <Box>
                <Typography variant="h5" sx={{ mb: 3, fontWeight: 'bold' }}>
                    Статистика по вопросам
                </Typography>
                {questionCharts.map((chartData) => (
                    <Paper key={chartData.question.QuestionId} elevation={2} sx={{ p: 3, mb: 4 }}>
                        <Typography variant="h6" sx={{ mb: 3, fontWeight: 'bold' }}>
                            {chartData.question.Title}
                        </Typography>
                        {chartData.data.length === 0 ? (
                            <Typography variant="body2" sx={{ color: 'text.secondary', textAlign: 'center', py: 3 }}>
                                Нет ответов на этот вопрос
                            </Typography>
                        ) : (
                            <ResponsiveContainer width="100%" height={Math.max(300, chartData.data.length * 50)}>
                                <BarChart 
                                    layout="vertical" 
                                    data={chartData.data} 
                                    margin={{ top: 5, right: 30, left: 100, bottom: 5 }}
                                >
                                    <CartesianGrid strokeDasharray="3 3" />
                                    <XAxis 
                                        type="number" 
                                        domain={[0, chartData.maxCount]}
                                        allowDecimals={false}
                                        ticks={Array.from({ length: chartData.maxCount + 1 }, (_, i) => i)}
                                        label={{ value: 'Количество ответов', position: 'insideBottom', offset: -5 }}
                                    />
                                    <YAxis 
                                        dataKey="name" 
                                        type="category" 
                                        width={100}
                                        tick={{ fontSize: 12 }}
                                    />
                                    <Bar dataKey="value" radius={[0, 4, 4, 0]}>
                                        {chartData.data.map((entry, idx) => (
                                            <Cell key={`cell-${idx}`} fill={entry.fill} />
                                        ))}
                                        <LabelList dataKey="value" position="right" />
                                    </Bar>
                                </BarChart>
                            </ResponsiveContainer>
                        )}
                    </Paper>
                ))}
            </Box>
        </Box>
    );
};
