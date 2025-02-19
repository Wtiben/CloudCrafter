import { createProjectCommandCommandSchema } from "./createProjectCommandCommandSchema.ts";
import { projectDtoSchema } from "./projectDtoSchema.ts";
import { z } from "zod";

 /**
 * @description OK
 */
export const createProject200Schema = z.lazy(() => projectDtoSchema);

 export const createProjectMutationRequestSchema = z.lazy(() => createProjectCommandCommandSchema);

 export const createProjectMutationResponseSchema = z.lazy(() => createProject200Schema);