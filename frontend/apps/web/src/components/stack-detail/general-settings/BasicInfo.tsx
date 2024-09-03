import {
	Card,
	CardContent,
	CardDescription,
	CardHeader,
	CardTitle,
} from '@ui/components/ui/card'
import { Input } from '@ui/components/ui/input'
import { Label } from '@ui/components/ui/label'
import { Button } from '@ui/components/ui/button'
import { Textarea } from '@ui/components/ui/textarea'
import { Switch } from '@ui/components/ui/switch'

export const BasicInfo = () => {
	return (
		<div className='space-y-6'>
			<Card>
				<CardHeader>
					<CardTitle>Stack Information</CardTitle>
					<CardDescription>Basic details about your Stack</CardDescription>
				</CardHeader>
				<CardContent className='space-y-4'>
					<div className='space-y-2'>
						<Label htmlFor='stack-name'>Stack Name</Label>
						<Input id='stack-name' placeholder='my-awesome-stack' />
					</div>
					<div className='space-y-2'>
						<Label htmlFor='stack-description'>Description</Label>
						<Textarea
							id='stack-description'
							placeholder='Describe your stack...'
						/>
					</div>
				</CardContent>
			</Card>

			<Card>
				<CardHeader>
					<CardTitle>Environment Variables</CardTitle>
					<CardDescription>
						Configure environment variables for your stack
					</CardDescription>
				</CardHeader>
				<CardContent className='space-y-4'>
					{/* Add a component for managing env variables */}
					<Button>Add Environment Variable</Button>
				</CardContent>
			</Card>

			<Card>
				<CardHeader>
					<CardTitle>Network Configuration</CardTitle>
					<CardDescription>
						Set up networking for your Docker stack
					</CardDescription>
				</CardHeader>
				<CardContent className='space-y-4'>
					<div className='flex items-center space-x-2'>
						<Switch id='use-custom-network' />
						<Label htmlFor='use-custom-network'>Use Custom Network</Label>
					</div>
					<div className='space-y-2'>
						<Label htmlFor='network-name'>Network Name</Label>
						<Input id='network-name' placeholder='my-custom-network' />
					</div>
				</CardContent>
			</Card>

			<Card>
				<CardHeader>
					<CardTitle>Volume Configuration</CardTitle>
					<CardDescription>
						Manage persistent storage for your stack
					</CardDescription>
				</CardHeader>
				<CardContent className='space-y-4'>
					{/* Add a component for managing volumes */}
					<Button>Add Volume</Button>
				</CardContent>
			</Card>

			<Card>
				<CardHeader>
					<CardTitle>Deployment Settings</CardTitle>
					<CardDescription>
						Configure deployment options for your stack
					</CardDescription>
				</CardHeader>
				<CardContent className='space-y-4'>
					<div className='flex items-center space-x-2'>
						<Switch id='enable-rolling-updates' />
						<Label htmlFor='enable-rolling-updates'>
							Enable Rolling Updates
						</Label>
					</div>
					<div className='space-y-2'>
						<Label htmlFor='replicas'>Number of Replicas</Label>
						<Input id='replicas' type='number' min='1' defaultValue='1' />
					</div>
				</CardContent>
			</Card>
		</div>
	)
}
