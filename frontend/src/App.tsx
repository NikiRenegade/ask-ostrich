import { BrowserRouter, Routes, Route } from 'react-router-dom'
import { AuthProvider } from './components/auth/AuthProvider'

import { MainLayout } from './layouts/MainLayout'
import { TelegramLayout } from './layouts/TelegramLayout'
import React from 'react';
import {SurveyDetail} from './components/survey-detail/SurveyDetail.tsx'
import {SurveyList} from './components/survey-list/SurveyList.tsx'
import {SurveyUserForm} from './components/survey-user-form/SurveyUserForm.tsx'
import './index.css';
import Header from './components/Header.tsx';
import {AuthProvider} from './components/auth/AuthProvider.tsx';
import Footer from './components/Footer.tsx';
import { BrowserRouter, Routes, Route } from "react-router-dom";

import { SurveyList } from './components/survey-list/SurveyList'
import { SurveyBuilder } from './components/survey-builder/SurveyBuilder'
import { SurveyUserForm } from './components/survey-user-form/SurveyUserForm'

import { TelegramAuth } from './components/auth/TelegramAuth'
import TelegramLogin from './components/login/TelegramLogin'
import TelegramRegister from './components/login/TelegramRegister'

const App = () => (
	<BrowserRouter>
		<AuthProvider>
			<Routes>
				{/* ===== Основное приложение ===== */}
				<Route element={<MainLayout />}>
					<Route path="/" element={<SurveyList />} />
					<Route path="/create" element={<SurveyBuilder />} />
					<Route path="/edit/:id" element={<SurveyBuilder />} />
					<Route path="/survey-form/:id" element={<SurveyUserForm />} />
				</Route>

				{/* ===== Telegram flow ===== */}
				<Route element={<TelegramLayout />}>
					<Route path="/auth/telegram" element={<TelegramAuth />} />
					<Route path="/auth/telegram/login" element={<TelegramLogin />} />
					<Route path="/auth/telegram/register" element={<TelegramRegister />} />
				</Route>

			</Routes>
		</AuthProvider>
	</BrowserRouter>
)

export default App;