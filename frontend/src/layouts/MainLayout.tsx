import { Outlet } from 'react-router-dom'
import Header from '../components/Header'
import Footer from '../components/Footer'

export const MainLayout = () => (
    <div style={{ display: 'flex', flexDirection: 'column', minHeight: '100vh' }}>
        <Header />
        <main style={{ flex: 1, backgroundColor: '#f3f4f6', padding: 24 }}>
            <Outlet />
        </main>
        <Footer />
    </div>
)
