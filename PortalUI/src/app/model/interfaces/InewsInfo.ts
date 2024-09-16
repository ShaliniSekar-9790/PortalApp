import { ICategory } from "./Icategory";

export interface InewsInfo {
    news_Id: number;
    title: string;
    news_Description: string;
    create_Date: string;
    updated_Date: string;
    category: ICategory;
  }