import {observer} from "mobx-react-lite";
import Header from "../../Header";
import Footer from "../../Footer";
import {useContext, useEffect, useState} from "react";
import {Context} from "../../../index";
import {EventsResponse} from "../../../types/response/EventsResponse";
import {EventsService} from "../../../api/services/EventsService";
import {Link, useNavigate} from "react-router-dom";
import EventCard from "../../EventCard";
import "./styles.css"


const AdminPage = () => {
    const {store} = useContext(Context);
    const navigate = useNavigate();
    const [events, setEvents] = useState<EventsResponse[]>([]);
    const [currentPage, setCurrentPage] = useState(1);
    const [totalPages, setTotalPages] = useState(0);
    const [pageSize, setPageSize] = useState(8);
    const [searchTerm, setSearchTerm] = useState("");
    const [categoryFilter, setCategoryFilter] = useState("");
    const [locationFilter, setLocationFilter] = useState("");
    const [startDateFilter, setStartDateFilter] = useState("");
    const [endDateFilter, setEndDateFilter] = useState("");
    const [updateEvents, setUpdateEvents] = useState<boolean>(false);

    useEffect(() => {
        const fetchEvents = async () => {
            try {
                const startDate = startDateFilter ? new Date(startDateFilter).toISOString() : "";
                const endDate = endDateFilter ? addHourAndMinutes(endDateFilter) : "";
                const response = await EventsService.getEventsByFilter(
                    searchTerm,
                    locationFilter,
                    startDate,
                    endDate,
                    categoryFilter,
                    "",
                    currentPage.toString(),
                    pageSize.toString()
                );
                setEvents(response.data.events);
                const totalEventCount = Number(response.data.totalEventCount);
                setTotalPages(Math.ceil(totalEventCount / pageSize));
            } catch (e) {
                console.error(e);
            }
        };
        fetchEvents();
        setUpdateEvents(false);
    }, [currentPage, searchTerm, categoryFilter, startDateFilter, endDateFilter, updateEvents, locationFilter]);

    useEffect(() => {
        setCurrentPage(1);
    }, [searchTerm, categoryFilter, startDateFilter, endDateFilter]);

    const addHourAndMinutes = (date: string) => {
        const newDate = new Date(date);
        newDate.setHours(23);
        newDate.setMinutes(59);
        return newDate.toISOString();
    }

    const handleNextPage = () => {
        if (currentPage < totalPages) {
            setCurrentPage(prevPage => prevPage + 1);
        }
        scrollToTop();
    };

    const handlePreviousPage = () => {
        if (currentPage > 1) {
            setCurrentPage(prevPage => prevPage - 1);
        }
        scrollToTop();
    };

    const scrollToTop = () => {
        window.scrollTo({
            top: 0,
            behavior: 'smooth'
        });
    };

    const handleDelete = async (id: string) => {
        try {
            await EventsService.deleteEvent(id);
            setCurrentPage(1);
            setUpdateEvents(true);
        } catch (e) {
            console.error(e);
        }
    }

    return (
        <div className='wrapper'>
            <Header onSearch={setSearchTerm} onCategory={setCategoryFilter} onStartDate={setStartDateFilter}
                    onEndDate={setEndDateFilter} onLocation={setLocationFilter}/>
            <div className='content'>
                <button className="btn_edit" onClick={() => navigate('add_event')}>Add new Event</button>
                <div className="events">
                    {events.map(event => (
                        <div key={event.id} className={"event_in_admit_page"}>
                                <EventCard event={event}/>
                            <div className="events_button">
                                <button className="btn_edit" onClick={() => navigate(`edit_event/${event.id}`)}>Edit</button>
                                <button className="btn_delete" onClick={() => handleDelete(event.id)}>Delete</button>
                            </div>
                        </div>
                    ))}
                </div>
                {totalPages > 1 && (
                    <div className="pagination">
                        <button onClick={handlePreviousPage} disabled={currentPage === 1}>Prev</button>
                        <span>{currentPage} of {totalPages}</span>
                        <button onClick={handleNextPage} disabled={currentPage === totalPages}>Next</button>
                    </div>
                )}
            </div>
            <Footer/>
        </div>
    );
};

export default observer(AdminPage);