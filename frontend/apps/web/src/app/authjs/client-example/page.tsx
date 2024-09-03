import ClientExample from '@/src/app/authjs/components/client-example.tsx'
import { getCloudCraftSession } from '@/src/core/auth/getCloudCraftSession.ts'
import { SessionProvider } from 'next-auth/react'

export default async function ClientPage() {
	const session = await getCloudCraftSession()
	console.log(session)

	return (
		<SessionProvider basePath={'/auth'} session={session}>
			<ClientExample />
		</SessionProvider>
	)
}
