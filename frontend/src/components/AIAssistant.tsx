import React, { useState } from 'react';

interface AIAssistantProps {
    onPromptSubmit?: (prompt: string) => void;
}

export const AIAssistant: React.FC<AIAssistantProps> = ({ onPromptSubmit }) => {
    const [prompt, setPrompt] = useState<string>('');

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        if (prompt.trim() && onPromptSubmit) {
            onPromptSubmit(prompt.trim());
        }
    };

    return (
        <div className="space-y-4">
            <div>
                <h3 className="text-lg font-semibold text-gray-800 mb-2">
                    ИИ Ассистент
                </h3>
                <p className="text-sm text-gray-600 mb-4">
                    Опишите, какие изменения необходимо внести в текущий опрос.
                </p>
            </div>

            <form onSubmit={handleSubmit} className="space-y-4">
                <div>
                    <label htmlFor="ai-prompt" className="block text-sm font-medium text-gray-700 mb-2">
                        Ваш запрос:
                    </label>
                    <textarea
                        id="ai-prompt"
                        className="w-full h-32 border border-gray-300 rounded-lg px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent resize-none"
                        placeholder="Например: Создай опрос пользовательской удовлетворенности сайтом с 5 вопросами разного типа..."
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
                        onClick={() => setPrompt('')}
                        className="px-4 py-2 bg-gray-200 text-gray-700 rounded hover:bg-gray-300 focus:outline-none focus:ring-2 focus:ring-gray-500 focus:ring-offset-2 transition-colors"
                    >
                        Очистить
                    </button>
                </div>
            </form>
        </div>
    );
};
