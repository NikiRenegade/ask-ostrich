import React from 'react';
import {SurveyBuilder} from './components/survey-builder/SurveyBuilder.tsx'
import {SurveyList} from './components/survey-list/SurveyList.tsx'
import {SurveyUserForm} from './components/survey-user-form/SurveyUserForm.tsx'
import './index.css';
import Header from './components/Header.tsx';
import {AuthProvider} from './components/auth/AuthProvider.tsx';
import Footer from './components/Footer.tsx';
import { BrowserRouter, Routes, Route } from "react-router-dom";

const App: React.FC = () => (
    <>
        <BrowserRouter>
            <AuthProvider>
                <div style={{ display: 'flex', flexDirection: 'column', minHeight: '100vh' }}>
                    <Header/>
                    <main style={{ flex: 1, backgroundColor: '#f3f4f6', padding: '24px' }}>
                        <Routes>
                            <Route path="/" element={<SurveyList />} />
                            <Route path="/create" element={<SurveyBuilder />} />
                            <Route path="/edit/:id" element={<SurveyBuilder />} />
                            <Route path="/survey-form/:id" element={<SurveyUserForm />} />
                        </Routes>
                    </main>
                    <Footer/>
                </div>
            </AuthProvider>
        </BrowserRouter>
    </>
);

export default App;