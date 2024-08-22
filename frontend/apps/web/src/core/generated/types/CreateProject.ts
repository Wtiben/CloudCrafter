import { CreateProjectCommandCommand } from './CreateProjectCommandCommand'
import type { ProjectDto } from './ProjectDto'

 /**
 * @description OK
*/
export type CreateProject200 = ProjectDto;
export type CreateProjectMutationRequest = CreateProjectCommandCommand;
/**
 * @description OK
*/
export type CreateProjectMutationResponse = ProjectDto;
export type CreateProjectMutation = {
    Response: CreateProjectMutationResponse;
    Request: CreateProjectMutationRequest;
};