import {EventsResponse} from "./EventsResponse";

export interface FilteredEventsResponse {
    events: EventsResponse[];
    totalEventCount: string;
}