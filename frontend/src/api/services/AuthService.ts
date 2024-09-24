import $api from "../../http";
import { AxiosResponse } from "axios";
import {AuthResponse} from "../../types/response/AuthResponse";


export default class AuthService {
    static async login(userName: string, password: string): Promise<AxiosResponse<AuthResponse>> {
        return $api.post<AuthResponse>('/Auth/login', {userName, password});
    }

    static async registration(userName: string, userEmail: string,  password: string, role: string): Promise<AxiosResponse<AuthResponse>> {
        return $api.post<AuthResponse>('/User/register', {userName, userEmail, password, role});
    }
    static async logout(): Promise<void> {
        return $api.post('/User/logout');
    }
}