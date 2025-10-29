import React from 'react';

interface JsonEditorProps {
    jsonText: string;
    onJsonChange: (e: React.ChangeEvent<HTMLTextAreaElement>) => void;
}

export const JsonEditor: React.FC<JsonEditorProps> = ({ jsonText, onJsonChange }) => {
    return (
        <div>
            <h2 className="text-xl font-semibold mb-2 text-gray-700">JSON структуры</h2>
            <textarea
                className="w-full h-[700px] font-mono text-sm border border-gray-300 rounded p-2 bg-white"
                value={jsonText}
                onChange={onJsonChange}/>
            <p className="text-xs text-gray-500 mt-1">
                Изменяй JSON — изменения появятся в опросе (если JSON валидный)
            </p>
        </div>
    );
};
