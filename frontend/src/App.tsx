import React from 'react';
import {SurveyBuilder} from './components/SurveyBuilder.tsx'
import './index.css';

const App: React.FC = () => (
    <div className="min-h-screen bg-gray-100 p-6">
        <SurveyBuilder />
    </div>
);

export default App;