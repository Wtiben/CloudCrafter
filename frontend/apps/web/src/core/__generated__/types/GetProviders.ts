import type { ProviderOverviewDto } from "./ProviderOverviewDto.ts";

 /**
 * @description OK
*/
export type GetProviders200 = ProviderOverviewDto;

 export type GetProvidersQueryResponse = GetProviders200;

 export type GetProvidersQuery = {
    Response: GetProviders200;
    Errors: any;
};