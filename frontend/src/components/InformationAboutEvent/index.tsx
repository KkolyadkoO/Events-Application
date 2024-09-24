import './styles.css'
import { EventsResponse } from "../../types/response/EventsResponse";
import { CategoryOfEventResponse } from "../../types/response/CategoryOfEventResponse";
import { Context } from "../../index";
import { useContext, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { MembersService } from "../../api/services/MembersService";
import {LocationOfEventResponse} from "../../types/response/LocationOfEventResponse";
import {API_URL, BASE_API_URL} from "../../http";

type Props = {
    event: EventsResponse;
    category: CategoryOfEventResponse;
    location: LocationOfEventResponse;
}

const InformationOfEvent = ({ event, category, location }: Props) => {
    const { store } = useContext(Context);
    const navigate = useNavigate();
    const [isRegistered, setIsRegistered] = useState(false);

    useEffect(() => {
        const checkRegistration = async () => {
            if (store.isAuth) {
                try {
                    const response = await MembersService.getMembersByUserId(store.user.id);
                    console.log(response.data);
                    if (response.data.some(member => String(member.eventId).toLowerCase() === String(event.id).toLowerCase()))
                        setIsRegistered(true);
                } catch (e) {
                    console.error(e);
                }
            }
        }
        checkRegistration();
    }, [event.id, isRegistered, store.isAuth, store.user.id]);

    if (!event) {
        return <p>No event information available</p>;
    }

    const { title, imageUrl, date, description, maxNumberOfMembers, numberOfMembers } = event;

    const formatDate = (dateString: string): string => {
        const date = new Date(dateString);
        return date.toLocaleDateString('en-GB', {
            day: '2-digit',
            month: 'short',
            year: 'numeric',
        });
    };

    const handleOnClick = () => {
        navigate(`/registration_on_event/${event.id}`);
    };

    const formatImageUrl = (url: string): string => {
        if (url) {
            return `${BASE_API_URL}${url}`;
        }
        return url
    };

    return (
        <div className="information-of-event">
            <span className="title">{title}</span>
            <img className="image" src={formatImageUrl(imageUrl) || "https://placehold.co/1280x720"} alt={title} />
            <span className="description">{description}</span>
            <div className="info">
                <span className="info-text">{formatDate(date)}</span>
                <span className="info-text">{numberOfMembers}/{maxNumberOfMembers}</span>
            </div>
            <div className="info">
                <span className="info-text">{location.title}</span>
                <span className="info-text">{category.title}</span>
            </div>
            {(isRegistered && !(numberOfMembers === maxNumberOfMembers)) ? (
                <button className="full-button">Already registered</button>
            ) : (
                numberOfMembers === maxNumberOfMembers ? (
                    <button className="full-button">Full</button>
                ) : (
                    <button className="sing-up" onClick={handleOnClick}>Sign up</button>
                )
            )}
        </div>
    );
};

export default InformationOfEvent;
