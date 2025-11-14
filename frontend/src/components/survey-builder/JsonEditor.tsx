import React from 'react';
import { TextField, Typography, Box } from '@mui/material';

interface JsonEditorProps {
    jsonText: string;
    onJsonChange: (text: string) => void;
    disabled?: boolean;
}

export const JsonEditor: React.FC<JsonEditorProps> = ({ jsonText, onJsonChange, disabled = false }) => {
    return (
        <Box>
            <Typography variant="h6" sx={{ mb: 2, color: 'text.secondary' }}>
                JSON структуры
            </Typography>
            <Typography variant="body2" sx={{ mb: 2, color: 'text.secondary' }}>
                Изменяй JSON — изменения появятся в опросе (если JSON валидный)
            </Typography>
            <TextField
                fullWidth
                multiline
                rows={25}
                value={jsonText}
                onChange={(e) => onJsonChange(e.target.value)}
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