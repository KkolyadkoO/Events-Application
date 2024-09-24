import $api from "../../http";
import { AxiosResponse } from "axios";
import {CategoryOfEventResponse} from "../../types/response/CategoryOfEventResponse";

export class CategoryOfEventService {
    static async getAllCategories(): Promise<AxiosResponse<CategoryOfEventResponse[]>> {
        return $api.get<CategoryOfEventResponse[]>('CategoryOfEvents');
    }

    static async getCategoryById(id: string | undefined): Promise<AxiosResponse<CategoryOfEventResponse>> {
        return $api.get<CategoryOfEventResponse>(`CategoryOfEvents/${id}`);
    }

    static async addCategory(title: string): Promise<AxiosResponse<string>> {
        return $api.post<string>(`CategoryOfEvents`, {title});
    }
}