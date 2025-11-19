import React from 'react';
import { TextField, Typography, Box } from '@mui/material';

interface YamlEditorProps {
    yamlText: string;
    onYamlChange: (text: string) => void;
    disabled?: boolean;
    onUserEditingChange?: (isEditing: boolean) => void;
}

export const YamlEditor: React.FC<YamlEditorProps> = ({ yamlText, onYamlChange: onYamlChange, disabled = false, onUserEditingChange }) => {
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
                value={yamlText}
                onChange={(e) =>{
                    onYamlChange(e.target.value)
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