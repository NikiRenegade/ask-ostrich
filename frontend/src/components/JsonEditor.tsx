import React from 'react';

interface JsonEditorProps {
    jsonText: string;
    onJsonChange: (e: React.ChangeEvent<HTMLTextAreaElement>) => void;
    disabled?: boolean;
}

export const JsonEditor: React.FC<JsonEditorProps> = ({ jsonText, onJsonChange, disabled = false }) => {
    return (
        <div>
            <h3 className="text-lg font-semibold text-gray-700 mb-2">JSON структуры</h3>
            <textarea
                className="w-full h-[600px] font-mono text-sm border border-gray-300 rounded p-2 bg-white disabled:opacity-50 disabled:cursor-not-allowed"
                value={jsonText}
                onChange={onJsonChange}
                disabled={disabled}
            />
            <p className="text-xs text-gray-500 mt-1">
                Изменяй JSON — изменения появятся в опросе (если JSON валидный)
            </p>
        </div>
    );
};