import React from 'react'
import { useNavigate, useSearchParams } from 'react-router-dom'
import { useAuth } from '../auth/AuthProvider'
import LoginForm from './LoginForm'
import {Box} from "@mui/material";

const TelegramLogin: React.FC = () => {
	const [params] = useSearchParams()
	const returnUrl = params.get('returnUrl') || '/'
	const navigate = useNavigate()
	const { login } = useAuth()

	const handleSubmit = (data: any) => {
		const token = data.accessToken ?? data.AccessToken
		const expiresIn = data.expiresIn ?? data.ExpiresIn ?? 15
		const user = data.userProfile ?? data.UserProfile

		login(user, token, expiresIn)
		navigate(returnUrl)
	}

	return (
		<Box display="flex" justifyContent="center" alignItems="center" minHeight="100vh" px={2}>
			<LoginForm
				onSubmit={handleSubmit}
				onChangeMode={() =>
					navigate(`/auth/telegram/register?returnUrl=${encodeURIComponent(returnUrl)}`)
				}
			/>
		</Box>
	)
}

export default TelegramLogin
