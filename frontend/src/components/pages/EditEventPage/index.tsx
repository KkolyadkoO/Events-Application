import Header from '../../Header';
import Footer from '../../Footer';
import './styles.css';
import EditEventForm from "../../forms/EditEventForm";
import {useParams} from "react-router-dom";
import {useEffect, useState} from "react";
import {EventsService} from "../../../api/services/EventsService";
import {EventsResponse} from "../../../types/response/EventsResponse";
import {observer} from "mobx-react-lite";


const EditEventPage = () => {
    const {id} = useParams<{ id: string }>();


    return (
        <div className='wrapper'>
            <Header/>
            <div className='content'>
                <EditEventForm eventId={id}/>
            </div>
            <Footer/>
        </div>
    );
}

export default observer(EditEventPage);