import React, { useEffect, useState } from "react";
import { Box, Typography } from "@mui/material";
import type { SurveyShort } from "../../types/SurveyShort";
import { PassedSurveyCard } from "./PassedSurveyCard";
import { useAuth } from "../auth/AuthProvider.tsx";
import { getPassedSurveysByUserId } from "../../services/surveyUserFormApi";

export const PassedSurveysList: React.FC = () => {
    const { user } = useAuth();
    const [surveys, setSurveys] = useState<SurveyShort[]>([]);
    const [loading, setLoading] = useState(false);

    const loadSurveys = async () => {
        if (!user) return;
        setLoading(true);

        try {
            const response = await getPassedSurveysByUserId(user.id);
            // Map SurveyResponse to SurveyShort
            const mappedSurveys: SurveyShort[] = response.map((s) => ({
                id: s.id,
                title: s.title,
                description: s.description,
                isPublished: s.isPublished,
                authorGuid: s.author.id,
                createdAt: s.createdAt,
                questionCount: s.questions.length,
            }));
            setSurveys(mappedSurveys);
        } catch (err) {
            console.error(err);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        loadSurveys();
    }, [user]);

    if (!user) return null;

    return (
        <Box>
            {surveys.map((s) => (
                <PassedSurveyCard key={s.id} survey={s} />
            ))}

            {surveys.length === 0 && !loading && (
                <Typography>Вы пока не прошли ни одного опроса.</Typography>
            )}
        </Box>
    );
};

