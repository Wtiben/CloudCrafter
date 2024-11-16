import type { ServerDto } from "./ServerDto.ts";

 /**
 * @description OK
*/
export type GetServers200 = ServerDto[];

 export type GetServersQueryResponse = GetServers200;

 export type GetServersQuery = {
    Response: GetServers200;
    Errors: any;
};