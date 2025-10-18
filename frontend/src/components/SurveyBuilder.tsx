import React, { useState } from 'react';
import { v4 as uuidv4 } from 'uuid';
import type { Question } from "../types/Question.ts";
import type { Survey } from '../types/Survey.ts';
import { QuestionEditor } from './QuestionEditor';
import {OrderArrows} from "./OrderArrows.tsx";

export const SurveyBuilder: React.FC = () => {
    const [survey, setSurvey] = useState<Survey>({
        SurveyId: uuidv4(),
        Title: '',
        Description: '',
        IsPublished: false,
        AuthorID: uuidv4(),
        CreatedAt: new Date().toISOString(),
        ShortUrl: '',
        Questions: [],
    });

    const [jsonText, setJsonText] = useState<string>(
        JSON.stringify(survey, null, 2)
    );

    // 🔁 Синхронизировать JSON при изменении survey
    React.useEffect(() => {
        setJsonText(JSON.stringify(survey, null, 2));
    }, [survey]);

    // 💾 Применить изменения из JSON обратно в survey
    const handleJsonChange = (e: React.ChangeEvent<HTMLTextAreaElement>) => {
        const text = e.target.value;
        setJsonText(text);

        try {
            const parsed = JSON.parse(text);
            // Только ключевые поля, чтобы не сломать типизацию
            setSurvey((prev) => ({
                ...prev,
                Title: parsed.Title || prev.Title,
                Description: parsed.Description || prev.Description,
                Questions: Array.isArray(parsed.Questions) ? parsed.Questions : prev.Questions,
            }));
        } catch {
        }
    };

    const addQuestion = () => {
        const newQuestion: Question = {
            QuestionId: uuidv4(),
            Type: 'text',
            Title: '',
            Order: survey.Questions.length + 1,
            InnerText: '',
            Options: [],
        };
        setSurvey({ ...survey, Questions: [...survey.Questions, newQuestion] });
    };

    const updateQuestionById = (id: string, updated: Question) => {
        const newQuestions = survey.Questions.map(q =>
            q.QuestionId === id ? updated : q
        );
        setSurvey({ ...survey, Questions: newQuestions });
    };
    const deleteQuestionById = (id: string) => {
        const newQuestions = survey.Questions.filter(q => q.QuestionId !== id).
                map((q, i) => ({ ...q, Order : i + 1}));
        setSurvey({...survey, Questions: newQuestions});
    };
    const handleSave = () => {
        console.log('Survey JSON:', survey);
        alert('Опрос создан! Посмотри результат в консоли.');
    };

    return (
        <div className="grid grid-cols-1 md:grid-cols-2 gap-6 mt-10 p-6">
            <div className="bg-white shadow rounded-lg p-6">
                <h2 className="text-2xl font-bold mb-4">Создать опрос</h2>

                <input
                    type="text"
                    className="w-full border border-gray-300 rounded px-3 py-2 mb-3"
                    placeholder="Название опроса"
                    value={survey.Title}
                    onChange={(e) => setSurvey({ ...survey, Title: e.target.value })}/>

                <textarea
                    className="w-full border border-gray-300 rounded px-3 py-2 mb-3"
                    placeholder="Описание"
                    value={survey.Description}
                    onChange={(e) => setSurvey({ ...survey, Description: e.target.value })}/>

                {[...survey.Questions].sort((a, b) => a.Order - b.Order).map(q => (
                    <div key={q.QuestionId} className="flex items-center gap-2 mb-2">
                        <OrderArrows item={q}
                                     list={survey.Questions}
                                     setList={(updated) => updateQuestionById(updated.QuestionId, updated)}/>
                    <QuestionEditor
                        question={q}
                        onChange={(updated) => updateQuestionById(updated.QuestionId, updated)}
                        onDelete={() => deleteQuestionById(q.QuestionId)}/>
                    </div>
                ))}

                <div className="flex gap-3 mt-4">
                    <button
                        type="button"
                        className="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600"
                        onClick={addQuestion}>
                        + Добавить вопрос
                    </button>
                    <button
                        type="button"
                        className="px-4 py-2 bg-green-500 text-white rounded hover:bg-green-600"
                        onClick={handleSave}>
                        💾 Сохранить опрос
                    </button>
                </div>
            </div>

            <div className="bg-gray-50 border border-gray-200 rounded-lg p-4">
                <h2 className="text-xl font-semibold mb-2 text-gray-700">JSON структуры</h2>
                <textarea
                    className="w-full h-[700px] font-mono text-sm border border-gray-300 rounded p-2 bg-white"
                    value={jsonText}
                    onChange={handleJsonChange}
                />
                <p className="text-xs text-gray-500 mt-1">
                    Изменяй JSON — изменения появятся в опросе (если JSON валидный)
                </p>
            </div>
        </div>
    );
};