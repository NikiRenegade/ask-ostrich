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
        <Box sx={{ height: '100%', display: 'flex', flexDirection: 'column', minHeight: 0 }}>
            <Typography variant="h6" sx={{ mb: 1, color: 'text.secondary', flexShrink: 0 }}>
                {codeType} структура
            </Typography>
            <Typography variant="body2" sx={{ mb: 1, color: 'text.secondary', flexShrink: 0 }}>
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
                    flex: 1,
                    minHeight: 0,
                    display: 'flex',
                    flexDirection: 'column',
                    '& .MuiInputBase-root': {
                        flex: 1,
                        minHeight: 0,
                        display: 'flex',
                        flexDirection: 'column',
                    },
                    '& .MuiInputBase-input': {
                        flex: 1,
                        minHeight: 0,
                        fontFamily: 'monospace',
                        fontSize: '0.875rem'
                    },
                }}
            />
        </Box>
    );
};