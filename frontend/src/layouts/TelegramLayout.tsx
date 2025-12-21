import { Outlet } from 'react-router-dom'

export const TelegramLayout = () => (
    <main style={{ minHeight: '100vh', backgroundColor: '#f3f4f6', padding: 24 }}>
        <Outlet />
    </main>
)