import {ITokens} from "../ITokens";
import {IUser} from "../IUser";


export interface AuthResponse {
    tokensResponse: ITokens;
    user: IUser;
}