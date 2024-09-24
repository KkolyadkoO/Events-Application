import {IUser} from "../types/IUser";
import {makeAutoObservable} from "mobx";
import AuthService from "../api/services/AuthService";
import axios from "axios";
import {AuthResponse} from "../types/response/AuthResponse";
import {API_URL} from "../http";

export default class Store {
    user = {} as IUser;
    isAuth = false;
    isLoading = false;

    constructor() {
        makeAutoObservable(this);
    }

    setAuth(bool: boolean) {
        this.isAuth = bool;
    }

    setUser(user: IUser) {
        this.user = user;
    }

    setLoading(bool: boolean) {
        this.isLoading = bool;
    }

    async Login(userName: string, password: string) {
        try {
            const response = await AuthService.login(userName, password);
            localStorage.setItem('accessToken', response.data.tokensResponse.accessToken);
            this.setAuth(true);
            this.setUser(response.data.usersResponse);
        } catch (e){
            throw e;
        }
    }
    async Registration(userName: string, userEmail: string,  password: string, role: string) {
        try {
            await AuthService.registration(userName, userEmail, password, role);
        } catch (e){
            throw e;
        }
    }

    async Logout() {
        try {
            await AuthService.logout();
            localStorage.removeItem('accessToken');
            this.setAuth(false);
            this.setUser({} as IUser);
        } catch (e){
            console.error(e);
        }
    }

    async checkAuth(){
        this.setLoading(true);
        try {
            const response = await axios.get<AuthResponse>(`${API_URL}/Auth/refresh`, {withCredentials: true});
            console.log(response);
            localStorage.setItem('accessToken', response.data.tokensResponse.accessToken);
            this.setAuth(true);
            this.setUser(response.data.usersResponse);
        } catch (e) {
            console.error(e);
        } finally {
            this.setLoading(false);
        }
    }
}