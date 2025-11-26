import React, { useState, useEffect } from 'react';
import { v4 as uuidv4 } from 'uuid';
import { TextField, Button, Box, Paper, Typography, Tabs, Tab, IconButton, Dialog, DialogContent, DialogTitle, Snackbar, Alert } from '@mui/material';
import type { Question } from "../../types/Question.ts";
import type {Survey, SurveyEdit} from '../../types/Survey.ts';
import { QuestionEditor } from './QuestionEditor';
import {OrderArrows} from "./OrderArrows.tsx";
import { useAuth } from '../auth/AuthProvider.tsx';
import { CodeEditor } from './CodeEditor.tsx';
import { AIAssistant } from './AIAssistant';
import type { ChatMessage } from '../../models/aiAssistantModels';
import SaveIcon from '@mui/icons-material/Save';
import VisibilityIcon from '@mui/icons-material/Visibility';
import CloseIcon from '@mui/icons-material/Close';
import { SurveyViewer } from '../survey-viewer/SurveyViewer.tsx';
import api from '../../services/axios';
import {yamlToObject, objectToYaml} from "../../services/Converters/yamlConverter.ts";
import {surveyToSurveyEditConverter} from "../../services/Converters/surveyToSurveyEditConverter.ts";
import {surveyEditToSurveyConverter} from "../../services/Converters/surveyEditToSurveyConverter.ts";

enum TabValues{
    AIAssistant,
    YamlEditor,
    JsonEditor,
}
export const SurveyBuilder: React.FC = () => {
    
    const { user } = useAuth();
    
    const [survey, setSurvey] = useState<Survey>({
        SurveyId: uuidv4(),
        Title: '',
        Description: '',
        IsPublished: false,
        AuthorGuid: '',
        CreatedAt: new Date().toISOString(),
        ShortUrl: '',
        Questions: [],
    });

    const [yamlText, setYamlText] = useState<string>(
        objectToYaml(surveyToSurveyEditConverter(survey))
    );

    React.useEffect(() => {
        if (user) {
            setSurvey({
                ...survey,
                AuthorGuid: user.id
            });
        }
    }, [user]);

    const [jsonText, setJsonText] = useState<string>(
        JSON.stringify(surveyToSurveyEditConverter(survey), null, 2)
    );

    type SnackSeverity = 'error' | 'warning' | 'info' | 'success'

    const [aiMessages, setAiMessages] = useState<ChatMessage[]>([]);
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [tabValue, setTabValue] = useState<number>(0);
    const [previewOpen, setPreviewOpen] = useState<boolean>(false);
    const [isUserEditingYaml, setIsUserEditingYaml] = useState(false);
    const [openedSnack, setOpenedSnack] = useState<boolean>(false);
    const [snackMessage, setSnackMessage] = useState<string>();
    const [snackSeverity, setSnackSeverity] = useState<SnackSeverity>();

    const handleCloseSnack = () => {
        setOpenedSnack(false);
    };
    const showSuccess = (msg: string) => {
        setSnackMessage(msg);
        setSnackSeverity("success")
        setOpenedSnack(true);
    };
    const showError = (msg: string) => {
        setSnackMessage(msg);
        setSnackSeverity("error")
        setOpenedSnack(true);
    };

    useEffect(() => {

        if (!isUserEditingYaml) {
            setYamlText(objectToYaml(surveyToSurveyEditConverter(survey)));
            setJsonText(JSON.stringify(surveyToSurveyEditConverter(survey), null, 2));
        }
    }, [survey, isUserEditingYaml]);

    React.useEffect(() => {
        if (!user) {
            setSurvey({
                ...survey,
                Title: '',
                Description: '',
                Questions: [],
            });
        }
    }, [user]);

    const handleYamlChange = (text: string) => {
        setYamlText(text);

        try {
            const parsedYaml = yamlToObject<SurveyEdit>(text);

            setSurvey(prev => surveyEditToSurveyConverter(parsedYaml, prev));
        } catch (e) {
        }
    };
    const handleJsonChange = (text: string) => {
        setJsonText(text);

        try {
            const parsedJson = JSON.parse(text);

            setSurvey(prev => surveyEditToSurveyConverter(parsedJson, prev));
        } catch (e) {
        }
    };

    const addQuestion = () => {
        const newQuestion: Question = {
            QuestionId: uuidv4(),
            Type: 'Text',
            Title: '',
            Order: survey.Questions.length + 1,
            InnerText: '',
            Options: [],
        };
        setSurvey({ ...survey, Questions: [...survey.Questions, newQuestion] });
    };

    const updateQuestionById = (id: string, updated: Question) => {
        const newQuestions = survey.Questions.map(q =>
            q.QuestionId === id ? updated : q
        );
        setSurvey({ ...survey, Questions: newQuestions });
    };
    const deleteQuestionById = (id: string) => {
        const newQuestions = survey.Questions.filter(q => q.QuestionId !== id).
                map((q, i) => ({ ...q, Order : i + 1}));
        setSurvey({...survey, Questions: newQuestions});
    };
    
    const handleSave = async (e: React.SyntheticEvent) => {
        e.preventDefault();

        setIsLoading(true);
        
        try {
          
          await api.post("/survey-manage/api/survey", {
            ...survey
          });

          showSuccess("Данные успешно сохранены!");
    
        } catch (err: unknown) {
          if (err instanceof Error) {
            showError(err.message);
          } else {
            showError("Неизвестная ошибка");
          }
        } finally {
            setIsLoading(false);
        }
    };

    const handleSurveyGenerationStarted = () => {
        setIsLoading(true);
    };

    const handleSurveyGenerated = (generatedSurvey: Survey | null) => {
        if (generatedSurvey) {
            setSurvey(generatedSurvey);
        }
        setIsLoading(false);
    };

    const handleTabChange = (_event: React.SyntheticEvent, newValue: number) => {
        setTabValue(newValue);
    };

    return (
        <Box sx={{ position: 'relative' }}>
            <Paper sx={{ p: 2, mb: 3, display: 'flex', justifyContent: 'flex-end', gap: 2 }}>
                <IconButton
                    onClick={() => setPreviewOpen(true)}
                    title='Предпросмотр'
                    disabled={!user}>
                        <VisibilityIcon />
                </IconButton>

                <IconButton
                    color="success"
                    title='Сохранить'
                    onClick={handleSave}
                    disabled={!user}>
                        <SaveIcon />
                </IconButton>
            </Paper>

            <Dialog
                open={previewOpen}
                onClose={() => setPreviewOpen(false)}
                maxWidth="lg"
                fullWidth
            >
                <DialogTitle sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                    <Typography variant="h6">Предпросмотр опроса</Typography>
                    <IconButton onClick={() => setPreviewOpen(false)} size="small">
                        <CloseIcon />
                    </IconButton>
                </DialogTitle>
                <DialogContent dividers>
                    <SurveyViewer survey={survey} />
                </DialogContent>
            </Dialog>
            <Box sx={{ display: 'grid', gridTemplateColumns: { xs: '1fr', md: '1fr 1fr' }, gap: 3 }}>
                <Paper sx={{ p: 3, opacity: isLoading ? 0.5 : 1, pointerEvents: isLoading ? 'none' : 'auto' }}>
                    <Typography variant="h4" component="h2" sx={{ mb: 3, fontWeight: 'bold' }}>
                        Создать опрос
                    </Typography>

                    <TextField
                        fullWidth
                        label="Название опроса"
                        placeholder="Название опроса"
                        value={survey.Title}
                        onChange={(e) => setSurvey({ ...survey, Title: e.target.value })}
                        sx={{ mb: 2 }}
                    />

                    <TextField
                        fullWidth
                        multiline
                        rows={3}
                        label="Описание"
                        placeholder="Описание"
                        value={survey.Description}
                        onChange={(e) => setSurvey({ ...survey, Description: e.target.value })}
                        sx={{ mb: 2 }}
                    />

                    {[...survey.Questions].sort((a, b) => a.Order - b.Order).map(q => (
                        <Box key={q.QuestionId} sx={{ display: 'flex', alignItems: 'center', gap: 1, mb: 2 }}>
                            <OrderArrows item={q}
                                        list={survey.Questions}
                                        setList={(updated) => updateQuestionById(updated.QuestionId, updated)}/>
                            <Box sx={{ flex: 1 }}>
                                <QuestionEditor
                                    question={q}
                                    onChange={(updated) => updateQuestionById(updated.QuestionId, updated)}
                                    onDelete={() => deleteQuestionById(q.QuestionId)}/>
                            </Box>
                        </Box>
                    ))}

                    <Box sx={{ display: 'flex', gap: 2, mt: 3 }}>
                        <Button
                            variant="contained"
                            color="primary"
                            onClick={addQuestion}
                            disabled={!user}>
                            + Добавить вопрос
                        </Button>
                    </Box>
                </Paper>
                <Paper sx={{ p: 2 }}>
                    <Box sx={{ borderBottom: 1, borderColor: 'divider', mb: 2 }}>
                        <Tabs value={tabValue} onChange={handleTabChange}>
                            <Tab icon={<span>✨</span>} iconPosition="start" label="AI" />
                            <Tab icon={<span>📝</span>} iconPosition="start" label="YAML" />
                            <Tab icon={<span>📄</span>} iconPosition="start" label="JSON" />
                        </Tabs>
                    </Box>

                    {tabValue === TabValues.AIAssistant && (
                        <AIAssistant                             
                            messages={aiMessages}                            
                            currentSurveyJson={JSON.stringify(survey, null, 2)}
                            onMessagesChange={setAiMessages}
                            onSurveyGenerationStarted={handleSurveyGenerationStarted}
                            onSurveyGenerated={handleSurveyGenerated}
                            disabled={!user || isLoading} 
                        />
                    )}
                    {tabValue === TabValues.YamlEditor && (
                        <CodeEditor codeText={yamlText} onCodeChange={handleYamlChange} disabled={!user || isLoading} onUserEditingChange={setIsUserEditingYaml} codeType = "Yaml"/>
                    )}
                    {tabValue === TabValues.JsonEditor && (
                        <CodeEditor codeText={jsonText} onCodeChange={handleJsonChange} disabled={!user || isLoading} onUserEditingChange={setIsUserEditingYaml} codeType = "Json"/>
                    )}
                </Paper>
            </Box>
            <Snackbar
                open={openedSnack}
                autoHideDuration={3000}
                onClose={handleCloseSnack}
                anchorOrigin={{ vertical: 'top', horizontal: 'center' }}
            >
                <Alert onClose={handleCloseSnack} severity={snackSeverity} variant="filled">
                    {snackMessage}
                </Alert>
            </Snackbar>
        </Box>
    );
};