import React from 'react';
import { TextField, Typography, Box } from '@mui/material';

interface CodeEditorProps {
    codeText: string;
    onCodeChange: (text: string) => void;
    disabled?: boolean;
    onUserEditingChange?: (isEditing: boolean) => void;
    codeType: "Json" | "Yaml";
}

export const CodeEditor: React.FC<CodeEditorProps> = ({ codeText, onCodeChange: onCodeChange, disabled = false, onUserEditingChange, codeType }) => {
    return (
        <Box>
            <Typography variant="h6" sx={{ mb: 2, color: 'text.secondary' }}>
                {codeType} структура
            </Typography>
            <Typography variant="body2" sx={{ mb: 2, color: 'text.secondary' }}>
                Изменяй {codeType} — изменения появятся в опросе (если {codeType} валидный)
            </Typography>
            <TextField
                fullWidth
                multiline
                rows={25}
                value={codeText}
                onChange={(e) =>{
                    onCodeChange(e.target.value)
                    onUserEditingChange?.(true);
                }}
                onBlur={() => onUserEditingChange?.(false)}
                disabled={disabled}
                sx={{
                    '& .MuiInputBase-input': {
                        fontFamily: 'monospace',
                        fontSize: '0.875rem',
                    }
                }}
            />
        </Box>
    );
};