import { InewsInfo } from "./InewsInfo";

export interface INewsPagedResponse {
    pageNumber: number;
    totalPages: number;
    totalRecords: number;
    hasNextPage: boolean
    hasPreviousPage: boolean
    newsInfos:InewsInfo[];
}