import React, { useState, useContext } from 'react';
import {
  AppBar,
  Toolbar,
  Container,
  Typography,
  Button,
  Box,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  IconButton
} from "@mui/material";
import CloseIcon from "@mui/icons-material/Close";
import LoginForm from './login/LoginForm';
import RegisterForm from './login/RegisterForm';
import { useAuth } from './auth/AuthProvider'; 
import logoImage from '../assets/header_logo.png'; 

const Header: React.FC = () => {
  
  const [openLogin, setOpenLogin] = useState(false);
  const [openConfirm, setOpenConfirm] = useState(false);
  const [loginMode, setLoginMode] = useState("login");
  const {user, token, login, logout} = useAuth();
 
  const handleLoginSubmit = (data:any) => {
    setOpenLogin(false);
    login(data.userProfile, data.accessToken, data.expiresIn);
  };

  const handleLoginClick = () => setOpenLogin(true);
  const handleLoginClose = () => {
    setOpenLogin(false);
    setTimeout(() => setLoginMode("login"), 300);
  }

  const handleOpenConfirm = () => setOpenConfirm(true);
  const handleCloseConfirm = () => setOpenConfirm(false);

  const handleLogoutConfirm = () => {
    logout();
    handleCloseConfirm();
  };

  return (
    <>
      <AppBar position="static" color="primary">
        <Toolbar sx={{display: "flex",  alignItems: "center"}}>
          
          <Box sx={{ flex: 1 }} />

          <Typography variant="h6" component="div" sx={{ flex: 1, textAlign: "center" }}>
            <img src={logoImage} alt="Logo" className="logo-img"/>
          </Typography>

          <Box sx={{ flex: 1, display: "flex", justifyContent: "flex-end", alignItems: "center", gap: 2 }}>
            {user && (
              <Box sx={{ display: "flex", flexDirection: "column", alignItems: "flex-end" }}>
                <Typography variant="body2" sx={{ color: 'inherit', fontWeight: 500, lineHeight: 1.2 }}>
                  {user.firstName} {user.lastName}
                </Typography>
                <Typography variant="caption" sx={{ color: 'inherit', opacity: 0.8, lineHeight: 1.2 }}>
                  {user.email}
                </Typography>
              </Box>
            )}
            {user ? (
              <Button color="inherit" onClick={handleOpenConfirm}>
                Выйти
              </Button>
            ) : (
              <Button color="inherit" onClick={handleLoginClick}>
                Войти
              </Button>
            )}
          </Box>
        </Toolbar>
      </AppBar>
      <Dialog open={openLogin} onClose={handleLoginClose}>
        <DialogTitle
          sx={{
            display: "flex",
            justifyContent: "space-between",
            alignItems: "center",
            pr: 1,
          }}
        >
          {loginMode === "login" ? "Вход" : "Регистрация"}
          <IconButton aria-label="close" onClick={handleLoginClose}>
            <CloseIcon />
          </IconButton>
        </DialogTitle>
        <DialogContent>
           {loginMode === "login" ? (
            <LoginForm
              onSubmit={handleLoginSubmit}
              onChangeMode={() => setLoginMode("register")}
            />
          ) : (
            <RegisterForm 
              onSubmit={handleLoginSubmit}
              onChangeMode={() => setLoginMode("login")}
            />
          )}
        </DialogContent>
      </Dialog>
      <Dialog open={openConfirm} onClose={handleCloseConfirm}>
        <DialogTitle>Подтверждение выхода</DialogTitle>
        <DialogContent>
          <Typography>Вы уверены, что хотите выйти из аккаунта?</Typography>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleCloseConfirm}>Отмена</Button>
          <Button onClick={handleLogoutConfirm} color="error" variant="contained">
            Выйти
          </Button>
        </DialogActions>
      </Dialog>
    </>
  );
};

export default Header;