import $api from "../../http";
import { AxiosResponse } from "axios";
import {LocationOfEventResponse} from "../../types/response/LocationOfEventResponse";

export class LocationOfEventService {
    static async getAllLocations(): Promise<AxiosResponse<LocationOfEventResponse[]>> {
        return $api.get<LocationOfEventResponse[]>('LocationOfEvents');
    }

    static async getLocationById(id: string | undefined): Promise<AxiosResponse<LocationOfEventResponse>> {
        return $api.get<LocationOfEventResponse>(`LocationOfEvents/${id}`);
    }

    static async addLocation(title: string): Promise<AxiosResponse<string>> {
        return $api.post<string>(`LocationOfEvents`, {title});
    }
}