import client from "../../backend/client.ts";
import type { RequestConfig } from "../../backend/client.ts";
import type { DeleteServerByIdMutationResponse, DeleteServerByIdPathParams } from "../types/DeleteServerById.ts";

 /**
 * @link /api/Servers/:id
 */
export async function deleteServerById(id: DeleteServerByIdPathParams["id"], config: Partial<RequestConfig> = {}) {
    const res = await client<DeleteServerByIdMutationResponse, Error, unknown>({ method: "DELETE", url: `/api/Servers/${id}`, ...config });
    return res.data;
}