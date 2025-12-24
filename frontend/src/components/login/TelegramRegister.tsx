import React from 'react'
import { useNavigate, useSearchParams } from 'react-router-dom'
import { useAuth } from '../auth/AuthProvider'
import RegisterForm from './RegisterForm'
import {Box} from "@mui/material";

const TelegramRegister: React.FC = () => {
	const [params] = useSearchParams()
	const returnUrl = params.get('returnUrl') || '/'
	const navigate = useNavigate()
	const { login } = useAuth()

	const handleSubmit = (data: any) => {
		const token = data.accessToken ?? data.AccessToken
		const expiresIn = data.expiresIn ?? data.ExpiresIn ?? 15
		const user = data.userProfile ?? data.UserProfile

		if (!token || !user) {
			console.warn('Unexpected auth response shape', data)
		}

		login(user, token, expiresIn)
		navigate(returnUrl)
	}

	return (
		<Box display="flex" justifyContent="center" alignItems="center" minHeight="100vh" px={2}>
			<RegisterForm
				onSubmit={handleSubmit}
				onChangeMode={() =>
					navigate(`/auth/telegram/login?returnUrl=${encodeURIComponent(returnUrl)}`)
				}
			/>
		</Box>
	)
}

export default TelegramRegister
