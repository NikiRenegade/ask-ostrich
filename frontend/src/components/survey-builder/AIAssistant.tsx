import React, { useState } from 'react';
import { TextField, Button, Box, Typography, CircularProgress } from '@mui/material';
import { generateSurvey } from '../../services/aiAssistantApi';
import type { GeneratedSurvey } from '../../models/aiAssistantModels';
import type { Survey } from '../../types/Survey';
import { v4 as uuidv4 } from 'uuid';

interface AIAssistantProps {    
    messages: ChatMessage[];   
    currentSurveyJson?: string;    
    onMessagesChange: (messages: ChatMessage[]) => void;
    onSurveyGenerationStarted: () => void;
    onSurveyGenerated: (survey: Survey | null) => void;
    disabled?: boolean;
}

export interface ChatMessage {
    id: string;
    isUserMessage: boolean;
    content: string;
    isPending?: boolean;
}

export const AIAssistant: React.FC<AIAssistantProps> = ({ messages, currentSurveyJson = '{}', onMessagesChange, onSurveyGenerationStarted, onSurveyGenerated, disabled }) => {
    const [prompt, setPrompt] = useState<string>('');    

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        const userPrompt = prompt.trim();
        if (!userPrompt) return;

        const userMessage: ChatMessage = {
            id: crypto.randomUUID(),
            isUserMessage: true,
            content: userPrompt,
        };

        const aiMessage: ChatMessage = {
            id: crypto.randomUUID(),
            isUserMessage: false,
            content: 'Запрос обрабатывается...',
            isPending: true,
        };

        onMessagesChange([...messages, userMessage, aiMessage]);        
        setPrompt('');

        try {
            onSurveyGenerationStarted();            

            const response = await generateSurvey({
                prompt: userPrompt,
                currentSurveyJson: currentSurveyJson,
                type: 0,
            });

            const convertedSurvey = convertToSurvey(response, currentSurveyJson);
            onSurveyGenerated(convertedSurvey);

            const updatedMessages = [...messages, userMessage];
            const responseMessage: ChatMessage = {
                id: aiMessage.id,
                isUserMessage: false,
                content: `Опрос успешно сгенерирован.\n\nНазвание: ${response.title}\nОписание: ${response.description}\nВопросов: ${response.questions.length}`,
                isPending: false,
            };
            onMessagesChange([...updatedMessages, responseMessage]);
        } catch (error) {
            const updatedMessages = [...messages, userMessage];
            const errorMessage: ChatMessage = {
                id: aiMessage.id,
                isUserMessage: false,
                content: 'Ошибка получения ответа от ИИ-ассистента',
                isPending: false,
            };
            console.error('Ошибка получения ответа от ИИ-ассистента:', error);
            onMessagesChange([...updatedMessages, errorMessage]);
        } finally {
            onSurveyGenerated(null);
        }
    };

    return (
        <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
            <Box>
                <Typography variant="h6" sx={{ mb: 1 }}>
                    ИИ Ассистент
                </Typography>
                {messages.length === 0 && (
                    <Typography variant="body2" sx={{ mb: 2, color: 'text.secondary' }}>
                        Опишите, какие изменения необходимо внести в текущий опрос.
                    </Typography>
                )}
            </Box>

            {messages.length > 0 && (
                <Box sx={{ display: 'flex', flexDirection: 'column', gap: 1.5 }}>
                    {messages.map((m) => {
                        return (
                            <Box
                                key={m.id}
                                sx={{
                                    display: 'flex',
                                    justifyContent: m.isUserMessage ? 'flex-start' : 'flex-end',
                                }}
                            >
                                <Box
                                    sx={{
                                        maxWidth: '85%',
                                        borderRadius: 1,
                                        px: 1.5,
                                        py: 1,
                                        fontSize: '0.875rem',
                                        boxShadow: 1,
                                        bgcolor: m.isUserMessage ? 'primary.main' : 'grey.100',
                                        color: m.isUserMessage ? 'primary.contrastText' : 'text.primary',
                                    }}
                                >
                                    <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                                        {!m.isUserMessage && m.isPending === true && (
                                            <CircularProgress size={16} />
                                        )}
                                        <Typography component="span" variant="body2">
                                            {m.content}
                                        </Typography>
                                    </Box>
                                </Box>
                            </Box>
                        );
                    })}
                </Box>
            )}

            <Box component="form" onSubmit={handleSubmit} sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
                <TextField
                    id="ai-prompt"
                    fullWidth
                    multiline
                    rows={4}
                    label="Ваш запрос:"
                    placeholder={messages.length === 0 ? "Например: Создай опрос пользовательской удовлетворенности сайтом с 5 вопросами разного типа..." : ""}
                    value={prompt}
                    onChange={(e) => setPrompt(e.target.value)}
                    disabled={disabled}
                />

                <Box sx={{ display: 'flex', gap: 2 }}>
                    <Button
                        type="submit"
                        variant="outlined"
                        color="primary"
                        startIcon={<span>✨</span>}
                        disabled={!prompt.trim() || disabled}>
                        Отправить
                    </Button>
                    <Button
                        type="button"
                        variant="outlined"
                        color="secondary"
                        startIcon={<span>🧹</span>}
                        onClick={() => onMessagesChange([])}
                        disabled={messages.length === 0 || disabled}>
                        Очистить диалог
                    </Button>
                </Box>
            </Box>
        </Box>
    );
};

function convertToSurvey(generated: GeneratedSurvey, currentSurveyJson: string): Survey {
    let currentSurvey: Partial<Survey> = {};
    try {
        currentSurvey = JSON.parse(currentSurveyJson || '{}');
    } catch {
    }

    return {
        SurveyId: currentSurvey.SurveyId || uuidv4(),
        Title: generated.title || currentSurvey.Title || '',
        Description: generated.description || currentSurvey.Description || '',
        IsPublished: currentSurvey.IsPublished || false,
        AuthorID: currentSurvey.AuthorID || uuidv4(),
        CreatedAt: currentSurvey.CreatedAt || new Date().toISOString(),
        ShortUrl: currentSurvey.ShortUrl || '',
        Questions: generated.questions.map((q, index) => ({
            QuestionId: uuidv4(),
            Type: mapQuestionType(q.type),
            Title: q.title,
            Order: q.order || index + 1,
            InnerText: q.innerText || '',
            Options: q.options.map((opt, optIndex) => ({
                Title: opt.title,
                Value: opt.value,
                Order: opt.order || optIndex + 1,
                IsCorrect: opt.isCorrect || false,
            })),
        })),
    };
}

function mapQuestionType(type: 0 | 1 | 2): 'text' | 'singleChoice' | 'multipleChoice' {
    switch (type) {
        case 0:
            return 'text';
        case 1:
            return 'singleChoice';
        case 2:
            return 'multipleChoice';
        default:
            return 'text';
    }
}
