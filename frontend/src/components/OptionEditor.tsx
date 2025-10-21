import React from 'react';
import type {Option} from "../types/Option.ts";
interface OptionEditorProps {
    option: Option;
    onChange: (updated: Option) => void;
    onDelete: () => void;
}

export const OptionEditor: React.FC<OptionEditorProps> = ({ option, onChange, onDelete }) => {
    return (
        <div className="flex items-center gap-2 mb-2">
            <input
                type="text"
                className="flex-1 border border-gray-300 rounded px-2 py-1"
                placeholder="Текст варианта"
                value={option.Title}
                onChange={(e) => onChange({ ...option, Title: e.target.value })}/>
            <input
                type="checkbox"
                checked={option.IsCorrect}
                onChange={(e) => onChange({ ...option, IsCorrect: e.target.checked })}/>
            <button
                type="button"
                className="text-red-500 hover:text-red-700 font-bold"
                onClick={onDelete}>✕</button>
        </div>
    );
};
