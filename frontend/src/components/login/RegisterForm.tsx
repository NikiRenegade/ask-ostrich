import React, { useState } from "react";
import { TextField, Button, Box, Typography, CircularProgress } from "@mui/material";
import type { LoginProps } from "../../types/LoginProps";

 const RegisterForm : React.FC<LoginProps> = ({onSubmit, onChangeMode}) => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirm, setConfirm] = useState("");
  const [userName, setUserName] = useState("");
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (password !== confirm) {
      setError("Пароли не совпадают");
      return;
    }

    setLoading(true);
    setError("");

    try {
      const res = await fetch("/api/Auth/register", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ email, password, userName, firstName, lastName })
      });

      const data:any = await res.json();

      if (!res.ok) throw new Error(data.message);

      onSubmit(data);

    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <Box component="form" onSubmit={handleSubmit} sx={{ mt: 1 }}>
      <TextField
        margin="normal"
        fullWidth
        label="Email"
        type="email"
        value={email}
        onChange={(e) => setEmail(e.target.value)}
        required
      />
      <TextField
        margin="normal"
        fullWidth
        label="Login"
        type="text"
        value={userName}
        onChange={(e) => setUserName(e.target.value)}
        required
      />
      <TextField
        margin="normal"
        fullWidth
        label="Имя"
        type="text"
        value={firstName}
        onChange={(e) => setFirstName(e.target.value)}
        required
      />
      <TextField
        margin="normal"
        fullWidth
        label="Фамилия"
        type="text"
        value={lastName}
        onChange={(e) => setLastName(e.target.value)}
        required
      />
      <TextField
        margin="normal"
        fullWidth
        label="Пароль"
        type="password"
        value={password}
        onChange={(e) => setPassword(e.target.value)}
        required
      />
      <TextField
        margin="normal"
        fullWidth
        label="Подтверждение пароля"
        type="password"
        value={confirm}
        onChange={(e) => setConfirm(e.target.value)}
        required
      />

      {error && (
        <Typography color="error" sx={{ mt: 1 }}>
          {error}
        </Typography>
      )}

      <Button
        type="submit"
        fullWidth
        variant="contained"
        sx={{ mt: 2 }}
        disabled={loading}
      >
        {loading ? <CircularProgress size={24} /> : "Зарегистрироваться"}
      </Button>

      <Button fullWidth variant="text" sx={{ mt: 1 }} onClick={onChangeMode}>
        Уже есть аккаунт? Скорее заходи
      </Button>
    </Box>
  );
}

export default RegisterForm;