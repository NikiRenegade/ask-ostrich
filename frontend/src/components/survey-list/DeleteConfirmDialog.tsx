import React from "react";
import {Dialog, DialogTitle, DialogContent, DialogActions, Button, Typography} from "@mui/material";
import WarningAmberIcon from "@mui/icons-material/WarningAmber";

interface Props {
    open: boolean;
    onCancel: () => void;
    onConfirm: () => void;
    title: string
}

export const DeleteConfirmDialog: React.FC<Props> = ({open, onCancel, onConfirm, title}) => {
    return (
        <Dialog
            open={open}
            onClose={onCancel}>
            <DialogTitle sx={{ display: "flex", alignItems: "center", gap: 1 }}>
                <WarningAmberIcon color="error" />
                Подтверждение удаления
            </DialogTitle>

            <DialogContent>
                <Typography>
                    {title}
                </Typography>
            </DialogContent>

            <DialogActions sx={{ p: 2 }}>
                <Button onClick={onCancel} variant="outlined">
                    Отмена
                </Button>
                <Button
                    onClick={onConfirm}
                    variant="contained"
                    color="error"
                    sx={{ boxShadow: "none" }}>
                    Удалить
                </Button>
            </DialogActions>
        </Dialog>
    );
};
