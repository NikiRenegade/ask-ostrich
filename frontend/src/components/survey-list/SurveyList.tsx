import React, {useEffect, useState} from "react";
import {Box, Typography} from "@mui/material";
import api from "../../services/axios";
import type {SurveyShort} from "../../types/SurveyShort";
import {SurveyShortCard} from "./SurveyShortCard";
import {useAuth} from "../auth/AuthProvider.tsx";
import {DeleteConfirmDialog} from "./DeleteConfirmDialog.tsx";

export const SurveyList: React.FC = () => {
    const {user} = useAuth();
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
    if (!user) return null;

    return (
        <Box sx={{p: 3}}>
            <Typography variant="h4" sx={{mb: 3, fontWeight: "bold"}}>
                Мои опросы
            </Typography>

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