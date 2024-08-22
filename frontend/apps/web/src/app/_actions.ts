'use server'

import { createProject, CreateProjectCommandCommand } from '@/src/core/generated'
import { getCurrentCloudCrafterUser } from '@/src/utils/auth'

export async function createProjectAction(dto: CreateProjectCommandCommand) {
    // TODO: Create a test that this call is only possible when logged in
    await getCurrentCloudCrafterUser()

    const result = await createProject(dto)

    return result

}