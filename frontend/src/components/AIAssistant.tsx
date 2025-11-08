import React, { useState } from 'react';
import { generateSurvey } from '../services/aiAssistantApi';
import type { GeneratedSurvey } from '../models/aiAssistantModels';
import type { Survey } from '../types/Survey';
import { v4 as uuidv4 } from 'uuid';

interface AIAssistantProps {    
    messages: ChatMessage[];   
    currentSurveyJson?: string;
    onPromptSubmit?: (prompt: string) => void;
    onMessagesChange: (messages: ChatMessage[]) => void;
    onSurveyGenerated?: (survey: Survey) => void;
}

export interface ChatMessage {
    id: string;
    isUserMessage: boolean;
    content: string;
    isPending?: boolean;
}

export const AIAssistant: React.FC<AIAssistantProps> = ({ messages, currentSurveyJson = '{}', onPromptSubmit, onMessagesChange,onSurveyGenerated }) => {
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

        if (onPromptSubmit) {
            onPromptSubmit(userPrompt);
        }

        setPrompt('');

        try {
            const response = await generateSurvey({
                prompt: userPrompt,
                currentSurveyJson: currentSurveyJson,
                type: 0,
            });

            const convertedSurvey = convertToSurvey(response, currentSurveyJson);

            if (onSurveyGenerated) {
                onSurveyGenerated(convertedSurvey);
            }

            const updatedMessages = [...messages, userMessage];
            const responseMessage: ChatMessage = {
                id: aiMessage.id,
                isUserMessage: false,
                content: `Опрос успешно сгенерирован!\n\nНазвание: ${response.title}\nОписание: ${response.description}\nВопросов: ${response.questions.length}`,
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
        }
    };

    return (
        <div className="space-y-4">
            <div>
                <h3 className="text-lg font-semibold text-gray-800 mb-2">
                    ИИ Ассистент
                </h3>
                {messages.length === 0 && (<p className="text-sm text-gray-600 mb-4">
                    Опишите, какие изменения необходимо внести в текущий опрос.
                </p>)}
            </div>

            {messages.length > 0 && (
                <div className="space-y-3">
                    {messages.map((m) => {
                        return (
                        <div
                            key={m.id}
                            className={
                                m.isUserMessage
                                    ? 'flex justify-start'
                                    : 'flex justify-end'
                            }
                        >
                            <div
                                className={
                                    (m.isUserMessage
                                        ? 'bg-blue-600 text-white'
                                        : 'bg-gray-100 text-gray-800') +
                                    ' max-w-[85%] rounded-lg px-3 py-2 text-sm shadow'
                                }
                            >
                                <div className="flex items-center gap-2">
                                    {!m.isUserMessage && (
                                        <span className="inline-flex items-center justify-center">
                                            {m.isPending === true && (
                                                <svg
                                                    className="h-4 w-4 animate-spin text-gray-500"
                                                    viewBox="0 0 24 24"
                                                    fill="none"
                                                    xmlns="http://www.w3.org/2000/svg"
                                                >
                                                    <circle
                                                        className="opacity-25"
                                                        cx="12"
                                                        cy="12"
                                                        r="10"
                                                        stroke="currentColor"
                                                        strokeWidth="4"
                                                    />
                                                    <path
                                                        className="opacity-75"
                                                        fill="currentColor"
                                                        d="M4 12a8 8 0 018-8v4a4 4 0 00-4 4H4z"
                                                    />
                                                </svg>
                                            )}
                                        </span>
                                    )}
                                    <span>{m.content}</span>
                                </div>
                            </div>
                        </div>
                        );
                    })}
                </div>
            )}

            <form onSubmit={handleSubmit} className="space-y-4">
                <div>
                    <label htmlFor="ai-prompt" className="block text-sm font-medium text-gray-700 mb-2">
                        Ваш запрос:
                    </label>
                    <textarea
                        id="ai-prompt"
                        className="w-full h-32 border border-gray-300 rounded-lg px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent resize-none"
                        placeholder={messages.length === 0 ? "Например: Создай опрос пользовательской удовлетворенности сайтом с 5 вопросами разного типа..." : ""}
                        value={prompt}
                        onChange={(e) => setPrompt(e.target.value)}
                    />
                </div>

                <div className="flex gap-3">
                    <button
                        type="submit"
                        className="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2 transition-colors"
                        disabled={!prompt.trim()}
                    >
                        ✨ Отправить
                    </button>
                    <button
                        type="button"
                        onClick={() => onMessagesChange([])}
                        className="px-4 py-2 bg-gray-200 text-gray-700 rounded hover:bg-gray-300 focus:outline-none focus:ring-2 focus:ring-gray-500 focus:ring-offset-2 transition-colors"
                    >
                        🧹 Очистить диалог
                    </button>
                </div>
            </form>
        </div>
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
