import React, { useState } from 'react';
import { Box, Tabs, Tab, IconButton } from '@mui/material';
import ArrowBackIcon from '@mui/icons-material/ArrowBack';
import { useNavigate } from 'react-router-dom';
import { SurveyBuilder } from '../survey-builder/SurveyBuilder';
import { SurveyAnswers } from '../survey-answers/SurveyAnswers';

export const SurveyDetail: React.FC = () => {
    const [activeTab, setActiveTab] = useState(0);
    const navigate = useNavigate();

    const handleTabChange = (_event: React.SyntheticEvent, newValue: number) => {
        setActiveTab(newValue);
    };

    return (
        <Box sx={{ position: 'relative' }}>
            <Box sx={{ borderBottom: 1, borderColor: 'divider', mb: 1, display: 'flex', alignItems: 'center' }}>
                <IconButton
                    onClick={() => navigate("/")}
                    title='Назад к списку опросов'
                    sx={{ mr: 1 }}>
                    <ArrowBackIcon />
                </IconButton>
                <Tabs 
                    value={activeTab} 
                    onChange={handleTabChange}
                    sx={{
                        flex: 1,
                        '& .MuiTab-root': {
                            fontSize: '1.5rem',
                            fontWeight: 'bold',
                            minHeight: '48px',
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

