import React, { useEffect, useState } from 'react'
import { Link as RouterLink, useSearchParams } from 'react-router-dom'
import {Box, Typography, Paper, Link, CircularProgress,} from '@mui/material'
import api from '../../services/axios'
import { useAuth } from './AuthProvider'

export const TelegramAuth: React.FC = () => {
	const [params] = useSearchParams()
	const authId = params.get('authId')
	const { token } = useAuth()
	const [status, setStatus] = useState<string | null>(null)
	const returnUrl = window.location.pathname + window.location.search

	useEffect(() => {
		if (!authId) {
			setStatus('Параметр authId не задан.')
			return
		}

		async function complete() {
			try {
				await api.post('/security/api/TelegramAuth/complete', { authId })
				setStatus(
					'Успешно — сейчас Telegram-бот получит подтверждение. Можно закрыть страницу.'
				)
			} catch (e: any) {
				if (e?.response?.status === 401) {
					window.location.assign(
						`auth/telegram/login?returnUrl=${encodeURIComponent(returnUrl)}`
					)
					return
				}

				setStatus('Ошибка: ' + (e?.message ?? 'Неизвестная ошибка'))
			}
		}

		if (token) {
			complete()
		}
	}, [authId, token])

	return (
		<Box display="flex" justifyContent="center" alignItems="center" minHeight="100vh" px={2}>
			<Paper elevation={3} sx={{ p: 4, maxWidth: 480, width: '100%' }}>
				<Typography variant="h5" fontWeight="bold" mb={3}>
					Вход через Telegram
				</Typography>

				{status ? (
					<Typography>{status}</Typography>
				) : (
					<Box display="flex" alignItems="center" gap={2}>
						<CircularProgress size={20} />
						<Typography>Готовлюсь…</Typography>
					</Box>
				)}

				{!token && (
					<Box mt={3}>
						<Typography variant="body2">
							Если у вас ещё нет аккаунта —{' '}
							<Link
								component={RouterLink}
								to={`/auth/telegram/register?returnUrl=${encodeURIComponent(returnUrl)}`} >
								зарегистрируйтесь
							</Link>{' '}
							или{' '}
							<Link
								component={RouterLink}
								to={`/auth/telegram/login?returnUrl=${encodeURIComponent(returnUrl)}`}
							>
								войдите
							</Link>
							.
						</Typography>
					</Box>
				)}
			</Paper>
		</Box>
	)
}
