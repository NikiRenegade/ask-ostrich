import React, { useState } from 'react';
import { TextField, Button, Box, Typography, CircularProgress, ToggleButtonGroup, ToggleButton, Tooltip } from '@mui/material';
import QuestionAnswerIcon from '@mui/icons-material/QuestionAnswer';
import BuildIcon from '@mui/icons-material/Build';
import { generateSurvey, askLLM } from '../../services/aiAssistantApi';
import { AssistentMode, type ChatMessage, type GeneratedSurvey } from '../../models/aiAssistantModels';
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

export const AIAssistant: React.FC<AIAssistantProps> = ({ messages, currentSurveyJson = '{}', onMessagesChange, onSurveyGenerationStarted, onSurveyGenerated, disabled }) => {
    const [prompt, setPrompt] = useState<string>('');
    const [assistentMode, setAssistentMode] = useState<AssistentMode>(AssistentMode.Construct);

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
            const isAskMode = assistentMode === AssistentMode.Ask;
            
            if (!isAskMode) {
                onSurveyGenerationStarted();
            }

            const updatedMessages = [...messages, userMessage];
            let responseMessage: ChatMessage;

            if (isAskMode) {
                const answerText = await askLLM({
                    prompt: userPrompt,
                    currentSurveyJson: currentSurveyJson,
                    type: 1,
                });

                responseMessage = {
                    id: aiMessage.id,
                    isUserMessage: false,
                    content: answerText,
                    isPending: false,
                    isHtml: true,
                };
            } else {
                const response = await generateSurvey({
                    prompt: userPrompt,
                    currentSurveyJson: currentSurveyJson,
                    type: 0,
                });

                const convertedSurvey = convertToSurvey(response, currentSurveyJson);
                onSurveyGenerated(convertedSurvey);

                responseMessage = {
                    id: aiMessage.id,
                    isUserMessage: false,
                    content: 'Опрос успешно обновлен',
                    isPending: false,
                };
            }

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
                                        {m.isHtml && !m.isUserMessage ? (
                                            <Box
                                                component="div"
                                                variant="body2"
                                                sx={{
                                                    '& p': { margin: '0.5em 0' },
                                                    '& ul, & ol': { margin: '0.5em 0', paddingLeft: '1.5em' },
                                                    '& h1, & h2, & h3, & h4, & h5, & h6': { margin: '0.5em 0' },
                                                    '& code': { 
                                                        backgroundColor: 'rgba(0, 0, 0, 0.05)',
                                                        padding: '0.2em 0.4em',
                                                        borderRadius: '3px',
                                                        fontFamily: 'monospace',
                                                    },
                                                    '& pre': {
                                                        backgroundColor: 'rgba(0, 0, 0, 0.05)',
                                                        padding: '0.5em',
                                                        borderRadius: '4px',
                                                        overflow: 'auto',
                                                    },
                                                }}
                                                dangerouslySetInnerHTML={{ __html: m.content }}
                                            />
                                        ) : (
                                            <Typography component="span" variant="body2">
                                                {m.content}
                                            </Typography>
                                        )}
                                    </Box>
                                </Box>
                            </Box>
                        );
                    })}
                </Box>
            )}

            <Box component="form" onSubmit={handleSubmit} sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
                <Box sx={{ display: 'flex', alignItems: 'center', gap: 2, mb: 1 }}>
                    <ToggleButtonGroup
                        value={assistentMode}
                        exclusive
                        onChange={(_, newMode) => {
                            if (newMode !== null) {
                                setAssistentMode(newMode);
                            }
                        }}
                        aria-label="assistent mode"
                        size="small"
                        disabled={disabled}
                    >
                        <ToggleButton value={AssistentMode.Construct} aria-label="construct mode">
                            <Tooltip title="Конструировать опрос">
                                <Box sx={{ display: 'flex', alignItems: 'center', gap: 0.5 }}>
                                    <BuildIcon fontSize="small" />
                                    <Typography variant="body2">Конструктор</Typography>
                                </Box>
                            </Tooltip>
                        </ToggleButton>
                        <ToggleButton value={AssistentMode.Ask} aria-label="ask mode">
                            <Tooltip title="Задать вопрос">
                                <Box sx={{ display: 'flex', alignItems: 'center', gap: 0.5 }}>
                                    <QuestionAnswerIcon fontSize="small" />
                                    <Typography variant="body2">Консультант</Typography>
                                </Box>
                            </Tooltip>
                        </ToggleButton>
                    </ToggleButtonGroup>
                </Box>
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
        AuthorGuid: currentSurvey.AuthorGuid || uuidv4(),
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

function mapQuestionType(type: 0 | 1 | 2): 'Text' | 'SingleChoice' | 'MultipleChoice' {
    switch (type) {
        case 0:
            return 'Text';
        case 1:
            return 'SingleChoice';
        case 2:
            return 'MultipleChoice';
        default:
            return 'Text';
    }
}
