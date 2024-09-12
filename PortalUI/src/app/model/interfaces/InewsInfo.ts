import { ICategory } from "./Icategory";

export interface InewsInfo {
    id: number;
    title: string;
    description: string;
    createDate: string;
    updatedDate: string;
    category: ICategory;
  }