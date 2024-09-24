import Header from '../../Header';
import Footer from '../../Footer';
import "./styles.css";
import {Link} from "react-router-dom";
import {useContext, useEffect, useState} from "react";
import {Context} from "../../../index";
import {observer} from "mobx-react-lite";
import {EventsService} from "../../../api/services/EventsService";
import EventCard from "../../EventCard";
import {EventsResponse} from "../../../types/response/EventsResponse";

const HomePage = () => {
    const {store} = useContext(Context);
    const [events, setEvents] = useState<EventsResponse[]>([]);
    const [currentPage, setCurrentPage] = useState(1);
    const [totalPages, setTotalPages] = useState(0);
    const [pageSize, setPageSize] = useState(8);
    const [searchTerm, setSearchTerm] = useState("");
    const [categoryFilter, setCategoryFilter] = useState("");
    const [locationFilter, setLocationFilter] = useState("");
    const [startDateFilter, setStartDateFilter] = useState("");
    const [endDateFilter, setEndDateFilter] = useState("");

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
    }, [currentPage, searchTerm, categoryFilter, startDateFilter, endDateFilter, locationFilter]);

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

    return (
        <div className='wrapper'>
            <Header onSearch={setSearchTerm} onCategory={setCategoryFilter}
                    onStartDate={setStartDateFilter} onEndDate={setEndDateFilter} onLocation={setLocationFilter}/>
            <div className='content'>
                <div className="events">
                    {events.map(event => (
                        <Link to={`/view_event/${event.id}`} draggable={"false"} key={event.id}>
                            <EventCard event={event} />
                        </Link>
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

export default observer(HomePage);
