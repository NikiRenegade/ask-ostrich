import React from 'react';
import {SurveyBuilder} from './components/SurveyBuilder.tsx'
import './index.css';
import Header from './components/Header.tsx';
import {AuthProvider} from './components/auth/AuthProvider.tsx';

const App: React.FC = () => (
    <>
        <AuthProvider>
            <Header/>
            <div className="min-h-screen bg-gray-100 p-6">
                <SurveyBuilder />
            </div>
        </AuthProvider>
    </>
);

export default App;