import Header from '../../Header';
import Footer from '../../Footer';
import "./styles.css"
import {useContext, useEffect, useState} from "react";
import {Context} from "../../../index";
import {observer} from "mobx-react-lite";
import {Link} from "react-router-dom";
import EventCard from "../../EventCard";
import {EventsResponse} from "../../../types/response/EventsResponse";
import {EventsService} from "../../../api/services/EventsService";
import {MembersService} from "../../../api/services/MembersService";

const ProfilePage = () => {
    const {store} = useContext(Context);
    const [events, setEvents] = useState<EventsResponse[]>([]);
    const [currentPage, setCurrentPage] = useState(1);
    const [totalPages, setTotalPages] = useState(0);
    const [pageSize] = useState(8);
    const [updateEvents, setUpdateEvents] = useState<boolean>(false);
    useEffect(() => {
        const fetchEvents = async () => {
            try {
                const response = await EventsService.getEventsByFilter("", "", "", "", "", store.user.id, currentPage.toString(), pageSize.toString());
                if (response?.data?.events) {
                    setEvents(response.data.events);
                    const totalEventCount = Number(response.data.totalEventCount);
                    setTotalPages(Math.ceil(totalEventCount / pageSize));
                } else {
                    console.error("Ошибка загрузки данных: Пустой ответ.");
                }
            } catch (e) {
                console.error("Ошибка при загрузке событий: ", e);
            }
        };

        fetchEvents();
        setUpdateEvents(false);
    }, [currentPage, store.user.id, pageSize, updateEvents]);

    const handleNextPage = () => {
        if (currentPage < totalPages) {
            setCurrentPage((prevPage) => prevPage + 1);
        }
        scrollToTop();
    };

    const handlePreviousPage = () => {
        if (currentPage > 1) {
            setCurrentPage((prevPage) => prevPage - 1);
        }
        scrollToTop();
    };

    const scrollToTop = () => {
        window.scrollTo({
            top: 0,
            behavior: 'smooth',
        });
    };

    const cancelRecording = async (eventId: string) => {
        try {
            await MembersService.deleteByEventIdAndUserId(eventId, store.user.id);
            setCurrentPage(1);
            setUpdateEvents(true);
        } catch (e) {
            console.error("Ошибка при удалении записи: ", e);
        }
    };

    return (
        <div className="wrapper">
            <Header />
            <div className="profile-content">
                <span className="userName">{store.user.userName}</span>
                <div className="events">
                    {events.length > 0 ? (
                        events.map((event) => (
                            <div key={event.id} className={"event_in_profile"}>
                                <Link to={`/view_event/${event.id}`} draggable={"false"}>
                                    <EventCard event={event} />
                                </Link>
                                <button className={"btn_cancel"} onClick={() => cancelRecording(event.id)}>Cancel</button>
                            </div>
                        ))
                    ) : (
                        <p>Событий не найдено</p>
                    )}
                </div>
                {totalPages > 1 && (
                    <div className="pagination">
                        <button onClick={handlePreviousPage} disabled={currentPage === 1}>
                            Prev
                        </button>
                        <span>
                            {currentPage} of {totalPages}
                        </span>
                        <button onClick={handleNextPage} disabled={currentPage === totalPages}>
                            Next
                        </button>
                    </div>
                )}
            </div>
            <Footer />
        </div>
    );
};

export default observer(ProfilePage);