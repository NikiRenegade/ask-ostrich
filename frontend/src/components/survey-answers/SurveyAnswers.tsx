import React, { useState } from 'react';
import { Box, Tabs, Tab } from '@mui/material';
import { AnswersByQuestions } from './AnswersByQuestions';
import { AnswersByUsers } from './AnswersByUsers';
import { AnswersSummary } from './AnswersSummary';

export const SurveyAnswers: React.FC = () => {
    const [activeTab, setActiveTab] = useState(0);

    const handleTabChange = (_event: React.SyntheticEvent, newValue: number) => {
        setActiveTab(newValue);
    };

    return (
        <Box>
            <Box sx={{ borderBottom: 1, borderColor: 'divider', mb: 3 }}>
                <Tabs 
                    value={activeTab} 
                    onChange={handleTabChange}
                    sx={{
                        '& .MuiTab-root': {
                            fontSize: '1rem',
                            fontWeight: 'normal',
                            textTransform: 'none',
                        }
                    }}>
                    <Tab label="По вопросам" />
                    <Tab label="По пользователям" />
                    <Tab label="Сводка" />
                </Tabs>
            </Box>

            {activeTab === 0 && (
                <AnswersByQuestions />
            )}

            {activeTab === 1 && (
                <AnswersByUsers />
            )}

            {activeTab === 2 && (
                <AnswersSummary />
            )}
        </Box>
    );
};

