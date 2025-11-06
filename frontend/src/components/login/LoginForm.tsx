import React, { useState } from "react";
import { TextField, Button, Box, Typography, Alert } from "@mui/material";
import type { LoginProps } from "../../types/LoginProps";

const LoginForm : React.FC<LoginProps> = ({onSubmit, onChangeMode}) => {
  
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState(null);

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!email || !password) {
      setError("Пожалуйста, заполните все поля");
      return;
    }

    setError(null);

    try {
      const res = await fetch("/api/Auth/login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ email, password })
      });

      const data:any = await res.json();

      if (!res.ok) throw new Error(data.message);

      onSubmit(data);

    } catch (err) {
      setError(err.message);
    }
  };

  return (
    <Box component="form" onSubmit={handleSubmit}

      sx={{
        maxWidth: 400,
        mx: "auto",
        mt: 5,
        display: "flex",
        flexDirection: "column",
        gap: 2,
        p: 3,
        border: "1px solid #ccc",
        borderRadius: 2,
        marginTop: 0,
      }}
    >

      <TextField
        label="Email"
        type="email"
        value={email}
        onChange={(e) => setEmail(e.target.value)}
        required
      />

      <TextField
        label="Пароль"
        type="password"
        value={password}
        onChange={(e) => setPassword(e.target.value)}
        required
      />

      {error && (
        <Typography color="error" sx={{ mt: 1 }}>
          {error}
        </Typography>
      )}

      <Button type="submit" variant="contained" color="primary">
        Войти
      </Button>
      <Button fullWidth variant="text" sx={{ mt: 1 }} onClick={onChangeMode}>
        Нет аккаунта? Скорее регистрируйся
      </Button>
    </Box>
  );
}

export default LoginForm;