import { Dialog, DialogTitle, DialogContent, DialogActions, Button, TextField, Box } from '@mui/material';
import { useState } from 'react';

export const GuestNameModal = ({ open, onSave }: { open: boolean; onSave: (name: string) => void }) => {
    const [name, setName] = useState('');

    return (
        <Dialog
            open={open}
            maxWidth="sm"
            fullWidth>
            <DialogTitle sx={{ fontSize: '1.5rem' }}>Введите имя</DialogTitle>
            <DialogContent sx={{ mt: 1 }}>
                <Box sx={{ mt: 1 }}>
                    <TextField
                        autoFocus
                        fullWidth
                        label="Ваше имя"
                        value={name}
                        onChange={(e) => setName(e.target.value)}
                        sx={{ fontSize: '1rem' }}/>
                </Box>
            </DialogContent>
            <DialogActions sx={{ justifyContent: 'flex-end', mt: 2 }}>
                <Button onClick={() => onSave('')}>Пропустить</Button>
                <Button onClick={() => onSave(name)} variant="contained" size="large">
                    OK
                </Button>
            </DialogActions>
        </Dialog>
    );
};