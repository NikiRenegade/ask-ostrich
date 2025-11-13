import React, { createContext, useContext, useEffect, useState } from "react";

const AuthContext = createContext(null);

export function AuthProvider({ children }) {
  const [user, setUser] = useState(null);
  const [token, setToken] = useState(null);
  const [expiresAt, setExpiresAt] = useState(null);

  // При старте проверяем localStorage
  useEffect(() => {
    const savedToken = localStorage.getItem("token");
    const savedUser = localStorage.getItem("user");
    const savedExpiresAt = localStorage.getItem("expiresAt");

    if (savedToken && savedExpiresAt) {
      if (Date.now() < Number(savedExpiresAt)) {
        setToken(savedToken);
        setUser(savedUser ? JSON.parse(savedUser) : null);
        setExpiresAt(Number(savedExpiresAt));
      } else {
        logout(); // токен просрочен
      }
    }
  }, []);

  const login = (userData, token, expiresIn) => {
    const expiresAt = Date.now() + expiresIn * 60 * 1000;
    setUser(userData);
    setToken(token);
    setExpiresAt(expiresAt);
    localStorage.setItem("token", token);
    localStorage.setItem("user", JSON.stringify(userData));
    localStorage.setItem("expiresAt", expiresAt);
  };

  const logout = () => {
    setUser(null);
    setToken(null);
    setExpiresAt(null);
    localStorage.removeItem("token");
    localStorage.removeItem("user");
    localStorage.removeItem("expiresAt");
  };

  
  return (
    <AuthContext.Provider value={{ user, token, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
}
export const useAuth = () => useContext(AuthContext);