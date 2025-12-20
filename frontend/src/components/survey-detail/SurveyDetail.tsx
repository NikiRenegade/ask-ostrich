import React, { useState } from 'react';
import { Box, Tabs, Tab } from '@mui/material';
import { SurveyBuilder } from '../survey-builder/SurveyBuilder';
import { SurveyAnswers } from '../survey-answers/SurveyAnswers';

export const SurveyDetail: React.FC = () => {
    const [activeTab, setActiveTab] = useState(0);

    const handleTabChange = (_event: React.SyntheticEvent, newValue: number) => {
        setActiveTab(newValue);
    };

    return (
        <Box sx={{ position: 'relative' }}>
            <Box sx={{ borderBottom: 1, borderColor: 'divider', mb: 3 }}>
                <Tabs 
                    value={activeTab} 
                    onChange={handleTabChange}
                    sx={{
                        '& .MuiTab-root': {
                            fontSize: '1.5rem',
                            fontWeight: 'bold',
                            minHeight: '64px',
                            textTransform: 'none',
                        }
                    }}>
                    <Tab label="Вопросы" />
                    <Tab label="Ответы" />
                </Tabs>
            </Box>

            {activeTab === 0 && (
                <SurveyBuilder />
            )}

            {activeTab === 1 && (
                <SurveyAnswers />
            )}
        </Box>
    );
};

