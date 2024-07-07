import { userDtoSchema } from './userDtoSchema'
import { z } from 'zod'


export const userDtoPaginatedListSchema = z.object({ 'page': z.number(), 'totalPages': z.number(), 'totalItems': z.number(), 'result': z.array(z.lazy(() => userDtoSchema)) })