import React from 'react';
import {SurveyBuilder} from './components/survey-builder/SurveyBuilder.tsx'
import {SurveyList} from './components/survey-list/SurveyList.tsx'
import './index.css';
import Header from './components/Header.tsx';
import {AuthProvider} from './components/auth/AuthProvider.tsx';
import Footer from './components/Footer.tsx';
import { BrowserRouter, Routes, Route } from "react-router-dom";

const App: React.FC = () => (
    <>
        <BrowserRouter>
            <AuthProvider>
                <Header/>
                <div className="min-h-screen bg-gray-100 p-6">
                    <Routes>
                        <Route path="/" element={<SurveyList />} />
                        <Route path="/create" element={<SurveyBuilder />} />
                    </Routes>
                </div>
                <Footer/>
            </AuthProvider>
        </BrowserRouter>
    </>
);

export default App;