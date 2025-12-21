import React from 'react';
import { v4 as uuidv4 } from 'uuid';
import { TextField, Button, Box, Paper, Typography, Select, MenuItem, FormControl, InputLabel, IconButton } from '@mui/material';
import DeleteIcon from '@mui/icons-material/Delete';
import type {Option} from "../../types/Option.ts";
import type {Question} from "../../types/Question.ts";
import type { QuestionType } from "../../types/QuestionType.ts";
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

        const newOptions = question.Options.map(option => {
            if (option.Value === value) {
                return { ...option, ...updated };
            }
            if (question.Type === 'SingleChoice' && updated.IsCorrect) {
                return { ...option, IsCorrect: false };
            }

            return option;
        });
        onChange({ ...question, Options: newOptions });
    };
    const deleteOptionByValue =  (value: string) => {
        const newOptions = question.Options.filter(o => o.Value !== value)
            .map((o, i) => ({...o, Order: i + 1}));
        onChange({ ...question, Options: newOptions });
    };

    return (
        <Paper sx={{ p: 2, mb: 2, width: '100%' }}>
            <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 2 }}>
                <Typography variant="h6" component="h3">
                    Вопрос #{question.Order}
                </Typography>
                <IconButton
                    color="error"
                    onClick={onDelete}
                    size="small">
                    <DeleteIcon />
                </IconButton>
            </Box>

            <TextField
                fullWidth
                label="Заголовок вопроса"
                placeholder="Заголовок вопроса"
                value={question.Title}
                onChange={(e) => onChange({ ...question, Title: e.target.value })}
                sx={{ mb: 2 }}
            />

            <FormControl fullWidth sx={{ mb: 2 }}>
                <InputLabel>Тип вопроса</InputLabel>
                <Select
                    value={question.Type}
                    label="Тип вопроса"
                    onChange={(e) =>
                        onChange({
                            ...question,
                            Type: e.target.value as QuestionType,
                            Options: e.target.value === 'Text' ? [] : question.Options
                        })}>
                    <MenuItem value="Text">Текстовый ответ</MenuItem>
                    <MenuItem value="SingleChoice">Один выбор</MenuItem>
                    <MenuItem value="MultipleChoice">Множественный выбор</MenuItem>
                </Select>
            </FormControl>

            <TextField
                fullWidth
                multiline
                rows={2}
                label="Описание вопроса (InnerText)"
                placeholder="Описание вопроса (InnerText)"
                value={question.InnerText}
                onChange={(e) => onChange({ ...question, InnerText: e.target.value })}
                sx={{ mb: 2 }}
            />

            {(question.Type === 'SingleChoice' || question.Type === 'MultipleChoice') && (
                <Box>
                    <Typography variant="body2" sx={{ fontWeight: 'medium', mb: 1 }}>
                        Варианты:
                    </Typography>
                    {[...question.Options].sort((a, b) => a.Order - b.Order).map(o => (
                        <Box key={o.Value} sx={{ display: 'flex', alignItems: 'center', gap: 1, mb: 2 }}>
                            <OrderArrows item={o}
                                     list={question.Options}
                                     setList={(updated) => updateOptionByValue(o.Value, updated)}/>
                            <OptionEditor
                                option={o}
                                onChange={(updated) => updateOptionByValue(o.Value, updated)}
                                onDelete={() => deleteOptionByValue(o.Value)}/>
                        </Box>
                    ))}
                    <Button
                        variant="contained"
                        color="primary"
                        size="small"
                        onClick={addOption}
                        sx={{ mt: 1 }}>
                        + Добавить вариант
                    </Button>
                </Box>
            )}
        </Paper>
    );
};
