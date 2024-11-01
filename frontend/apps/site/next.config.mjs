import { createMDX } from 'fumadocs-mdx/next'

const withMDX = createMDX()

/** @type {import('next').NextConfig} */
const config = {
    output: 'standalone',
	reactStrictMode: true,
}

export default withMDX(config)
