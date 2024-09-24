import './styles.css';
import {EventsResponse} from "../../types/response/EventsResponse";
import { BASE_API_URL} from "../../http";


type Props = {
    event: EventsResponse;
}

const EventCard = ({event}: Props) => {
    const {title, imageUrl, date} = event;


    const formatDate = (dateString: string): string => {
        const date = new Date(dateString);
        return date.toLocaleDateString('en-GB', {
            day: '2-digit',
            month: 'short',
            year: 'numeric',
        });
    };

    const formatImageUrl = (url: string): string => {
        if (url) {
            console.log(`${BASE_API_URL}${url}`);
            return `${BASE_API_URL}${url}`;
        }
        return url
    };



    return (
        <div className="event-card">
            <img src={formatImageUrl(imageUrl) || "https://placehold.co/300x400"} alt={title} className="event-image"/>
            <span className="event-title">{title}</span>
            <span className="event-date">{formatDate(date)}</span>
        </div>
    )
}

export default EventCard;
