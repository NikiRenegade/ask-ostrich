import React, {useEffect, useState} from "react";
import {Box, Typography, Paper, IconButton} from "@mui/material";
import AddIcon from "@mui/icons-material/Add";
import {useNavigate} from "react-router-dom";
import api from "../../services/axios";
import type {SurveyShort} from "../../types/SurveyShort";
import {SurveyShortCard} from "./SurveyShortCard";
import {useAuth} from "../auth/AuthProvider.tsx";
import {DeleteConfirmDialog} from "./DeleteConfirmDialog.tsx";

export const SurveyList: React.FC = () => {
    const {user} = useAuth();
    const navigate = useNavigate();
    const [surveys, setSurveys] = useState<SurveyShort[]>([]);
    const [loading, setLoading] = useState(false);
    const [deleteId, setDeleteId] = useState<string | null>(null);

    const loadSurveys = async () => {
        if (!user) return;
        setLoading(true);

        try {
            const res = await api.get(`/survey-manage/api/Survey/existing/${user.id}`);
            setSurveys(res.data);
        } catch (err) {
            console.error(err);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        loadSurveys();
    }, [user]);

    const handleDelete = async () => {
        if (!deleteId) return;

        try {
            await api.delete(`/survey-manage/api/survey/${deleteId}`);
            setSurveys((prev) => prev.filter((x) => x.id !== deleteId));
        } catch (err) {
            console.error(err);
        } finally {
            setDeleteId(null);
        }
    };

    const handleCreate = () => {
        navigate("/create");
    };

    if (!user) return null;

    return (
        <Box sx={{ position: 'relative' }}>
            <Typography variant="h4" sx={{mb: 3, fontWeight: "bold"}}>
                Мои опросы
            </Typography>

            <Paper sx={{ p: 2, mb: 3, display: 'flex', justifyContent: 'flex-end', gap: 2 }}>
                <IconButton
                    color="primary"
                    title='Создать опрос'
                    onClick={handleCreate}
                    disabled={!user}>
                    <AddIcon />
                </IconButton>
            </Paper>

            {surveys.map((s) => (
                <SurveyShortCard
                    key={s.id}
                    survey={s}
                    onDelete={() => setDeleteId(s.id)}/>
            ))}

            {surveys.length === 0 && !loading && (
                <Typography>У вас пока нет созданных опрсов.</Typography>
            )}

            <DeleteConfirmDialog
                open={deleteId !== null}
                onCancel={() => setDeleteId(null)}
                onConfirm={handleDelete}
                title="Вы действительно хотите удалить данный опрос?"/>
        </Box>
    );
};