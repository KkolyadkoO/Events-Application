import $api from "../../http";
import {AxiosResponse} from "axios";
import {MembersResponse} from "../../types/response/MembersResponse";
import {FilteredEventsResponse} from "../../types/response/FilteredEventsResponse";

export class MembersService {
    static async getMembersByUserId(userId: string): Promise<AxiosResponse<MembersResponse[]>> {
        return $api.get<MembersResponse[]>(`/MembersOfEvent/user/${userId}`);
    }

    static async getMembersByEventId(eventId: string): Promise<AxiosResponse<MembersResponse[]>> {
        return $api.get<MembersResponse[]>(`/MembersOfEvent/event/${eventId}`);
    }

    static async getMembersById(id: string): Promise<AxiosResponse<MembersResponse[]>> {
        return $api.get<MembersResponse[]>(`/MembersOfEvent/${id}`);
    }

    static async addMember(name: string, lastname: string, birthday: string,
                           email: string, userId: string, eventId: string): Promise<AxiosResponse<string>> {
        return $api.post<string>(`MembersOfEvent`, {name, lastname, birthday, email, userId, eventId});
    }

    static async deleteByEventIdAndUserId(eventId: string, userId: string) {
        return $api.delete('/MembersOfEvent', {
            params: {
                eventId: eventId,
                userId: userId
            }
        });
    }
}