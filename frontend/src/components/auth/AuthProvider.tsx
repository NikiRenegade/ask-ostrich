import { createContext, useContext, useEffect, useState } from "react";
import type { ReactNode } from "react";

export interface User {
    id: string;
    email: string;
    firstName: string;
    lastName: string;
}

interface AuthContextType {
  user: User | null;
  token: string | null;
  login: (user: User, token: string, expiresIn: number) => void;
  logout: () => void;
}

interface AuthProviderProps {
    children: ReactNode;
}

const AuthContext = createContext<AuthContextType | null>(null);

export function AuthProvider({ children }: AuthProviderProps) {
  const [user, setUser] = useState<User | null>(null);
  const [token, setToken] = useState<string | null>(null);
  const [expiresAt, setExpiresAt] = useState<number | null>(null);

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

  const login = (userData: User, token: string, expiresIn: number) => {
    const expiresAt = Date.now() + expiresIn * 60 * 1000;
    setUser(userData);
    setToken(token);
    setExpiresAt(expiresAt);
    localStorage.setItem("token", token);
    localStorage.setItem("user", JSON.stringify(userData));
    localStorage.setItem("expiresAt", String(expiresAt));
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
export const useAuth = () => {
  const ctx = useContext(AuthContext);
  
  if (!ctx) {
    throw new Error(
      "useAuth должен использоваться внутри компонента AuthProvider"
    );
  }
  return ctx;
}