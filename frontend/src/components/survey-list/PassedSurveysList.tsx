import React, { useEffect, useState } from "react";
import { Box, Typography } from "@mui/material";
import { PassedSurveyCard } from "./PassedSurveyCard";
import { useAuth } from "../auth/AuthProvider.tsx";
import { getPassedSurveysWithResultsByUserId, type PassedSurveyResponse } from "../../services/surveyUserFormApi";

export const PassedSurveysList: React.FC = () => {
    const { user } = useAuth();
    const [surveys, setSurveys] = useState<PassedSurveyResponse[]>([]);
    const [loading, setLoading] = useState(false);

    const loadSurveys = async () => {
        if (!user) return;
        setLoading(true);

        try {
            const response = await getPassedSurveysWithResultsByUserId(user.id);
            setSurveys(response);
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
                <PassedSurveyCard key={s.surveyId} survey={s} />
            ))}

            {surveys.length === 0 && !loading && (
                <Typography>Вы пока не прошли ни одного опроса.</Typography>
            )}
        </Box>
    );
};

