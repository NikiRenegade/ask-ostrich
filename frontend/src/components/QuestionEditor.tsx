import React from 'react';
import { v4 as uuidv4 } from 'uuid';
import type {Option} from "../types/Option.ts";
import type {Question} from "../types/Question.ts";
import type { QuestionType } from "../types/QuestionType.ts";
import { OptionEditor } from './OptionEditor';
import {OrderArrows} from "./OrderArrows.tsx";

interface QuestionEditorProps {
    question: Question;
    onChange: (updated: Question) => void;
    onDelete: () => void;
}

export const QuestionEditor: React.FC<QuestionEditorProps> = ({ question, onChange, onDelete }) => {
    const addOption = () => {
        const newOption: Option = {
            Title: '',
            Value: uuidv4(),
            Order: question.Options.length + 1,
            IsCorrect: false,
        };
        onChange({ ...question, Options: [...question.Options, newOption] });
    };

    const updateOptionByValue =  (value: string, updated: Option) => {
        const newOptions = question.Options.map(o =>
            o.Value === value ? updated : o
        );
        onChange({ ...question, Options: newOptions });
    };
    const deleteOptionByValue =  (value: string) => {
        const newOptions = question.Options.filter(o => o.Value !== value)
            .map((o, i) => ({...o, Order: i + 1}));
        onChange({ ...question, Options: newOptions });
    };

    return (
        <div className="border border-gray-300 rounded-lg p-4 bg-gray-50 mb-4">
            <div className="flex justify-between items-center mb-3">
                <h3 className="text-lg font-semibold">Вопрос</h3>
                <button
                    type="button"
                    className="text-red-500 font-bold text-xl"
                    onClick={onDelete}
                >
                    ✕
                </button>
            </div>

            <input
                type="text"
                className="w-full border border-gray-300 rounded px-3 py-2 mb-2"
                placeholder="Заголовок вопроса"
                value={question.Title}
                onChange={(e) => onChange({ ...question, Title: e.target.value })}
            />

            <select
                className="border border-gray-300 rounded px-2 py-1 mb-2"
                value={question.Type}
                onChange={(e) =>
                    onChange({
                        ...question,
                        Type: e.target.value as QuestionType,
                        Options: e.target.value === 'text' ? [] : question.Options,
                    })
                }
            >
                <option value="text">Текстовый ответ</option>
                <option value="singleChoice">Один выбор</option>
                <option value="multipleChoice">Множественный выбор</option>
            </select>

            <textarea
                className="w-full border border-gray-300 rounded px-3 py-2 mb-2"
                placeholder="Описание вопроса (InnerText)"
                value={question.InnerText}
                onChange={(e) => onChange({ ...question, InnerText: e.target.value })}
            />

            {(question.Type === 'singleChoice' || question.Type === 'multipleChoice') && (
                <div>
                    <p className="font-medium mb-1">Варианты:</p>
                    {[...question.Options].sort((a, b) => a.Order - b.Order).map(o => (
                        <div key={o.Value} className="flex items-center gap-2 mb-2">
                            <OrderArrows item={o}
                                     list={question.Options}
                                     setList={(updated) => updateOptionByValue(o.Value, updated)}/>
                            <OptionEditor
                            option={o}
                            onChange={(updated) => updateOptionByValue(o.Value, updated)}
                            onDelete={() => deleteOptionByValue(o.Value)}
                            />

                        </div>

                    ))}
                    <button
                        type="button"
                        className="mt-2 px-3 py-1 bg-blue-500 text-white rounded hover:bg-blue-600"
                        onClick={addOption}
                    >
                        + Добавить вариант
                    </button>
                </div>
            )}
        </div>
    );
};
