import React from 'react';
import { TextField, Checkbox, IconButton, Box } from '@mui/material';
import DeleteIcon from '@mui/icons-material/Delete';
import type {Option} from "../../types/Option.ts";

interface OptionEditorProps {
    option: Option;
    onChange: (updated: Option) => void;
    onDelete: () => void;
}

export const OptionEditor: React.FC<OptionEditorProps> = ({ option, onChange, onDelete }) => {
    return (
        <Box sx={{ display: 'flex', alignItems: 'center', gap: 1, flex: 1 }}>
            <TextField
                fullWidth
                size="small"
                placeholder="Текст варианта"
                value={option.Title}
                onChange={(e) => onChange({ ...option, Title: e.target.value })}
            />
            <Checkbox
                checked={option.IsCorrect}
                onChange={(e) => onChange({ ...option, IsCorrect: e.target.checked })}
            />
            <IconButton
                color="error"
                onClick={onDelete}
                size="small">
                <DeleteIcon />
            </IconButton>
        </Box>
    );
};
