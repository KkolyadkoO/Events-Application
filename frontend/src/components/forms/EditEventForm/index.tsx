import React, {useEffect, useState} from 'react';
import './styles.css';
import {useLocation, useNavigate} from "react-router-dom";
import {CategoryOfEventService} from "../../../api/services/CategoryOfEventService";
import {CategoryOfEventResponse} from "../../../types/response/CategoryOfEventResponse";
import {EventsService} from "../../../api/services/EventsService";
import {observer} from "mobx-react-lite";
import {LocationOfEventResponse} from "../../../types/response/LocationOfEventResponse";
import {LocationOfEventService} from "../../../api/services/LocationOfEventService";

type Props = {
    eventId: string | undefined;
}

const EditEventForm = ({eventId}: Props) => {
    const navigate = useNavigate();
    const id = eventId ?? "";
    const [categories, setCategories] = useState<CategoryOfEventResponse[]>([]);
    const [locations, setLocations] = useState<LocationOfEventResponse[]>([]);
    const [title, setTitle] = useState('');
    const [newCategoryTitle, setNewCategoryTitle] = useState('');
    const [newLocationTitle, setNewLocationTitle] = useState('');
    const [description, setDescription] = useState('');
    const [date, setDate] = useState('');
    const [locationId, setLocationId] = useState('');
    const [maxNumberOfMembers, setMaxNumberOfMembers] = useState<number>(0);
    const [categoryId, setCategoryId] = useState('');
    const [imageFile, setImageFile] = useState<File | null>(null); // Новое состояние для хранения файла

    useEffect(() => {
        const fetchEvent = async () => {
            if (!(id === "")) {
                try {
                    const response = await EventsService.getEventById(eventId);
                    setTitle(response.data.title);
                    setDescription(response.data.description);
                    setDate(response.data.date.substring(0, 10));
                    setLocationId(response.data.locationId);
                    setMaxNumberOfMembers(response.data.maxNumberOfMembers);
                    setCategoryId(response.data.categoryId);
                } catch (e) {
                    console.error(e);
                }
            }
        }
        fetchEvent();
    }, []);

    useEffect(() => {
        const fetchCategories = async () => {
            try {
                const response = await CategoryOfEventService.getAllCategories();
                setCategories(response.data);
            } catch (e) {
                console.error(e);
            }
        };
        const fetchLocations = async () => {
            try {
                const response = await LocationOfEventService.getAllLocations();
                setLocations(response.data);
            } catch (e) {
                console.error(e);
            }
        };
        fetchCategories();
        fetchLocations();
    }, []);

    const handleEdit = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            var newLocationId = "";
            var newCategoryId = "";
            if (locationId === "another") {
                const addedLocationId = await LocationOfEventService.addLocation(newLocationTitle);
                newLocationId = addedLocationId.data;
                console.log(newLocationId, addedLocationId.data);
            }
            if (categoryId === "another") {
                const addedCategoryId = await CategoryOfEventService.addCategory(newCategoryTitle);
                newCategoryId = addedCategoryId.data;
                console.log(newCategoryId, addedCategoryId.data);

            }
            const rightDate = new Date(date);
            rightDate.setUTCHours(0);
            rightDate.setUTCMinutes(0);
            let location = "";
            let category = "";
            if (newLocationId === "") {
                location = locationId;
            } else {
                location = newLocationId;
            }
            console.log(location);
            if (newCategoryId === "") {
                category = categoryId;
            } else {
                category = newCategoryId;
            }
            console.log(category);
            const formData = new FormData();

            formData.append("title", title);
            formData.append("description", description);
            formData.append("date", rightDate.toISOString());
            formData.append("locationId", location);
            formData.append("categoryId", category);
            formData.append("imageUrl", "url");
            formData.append("maxNumberOfMembers", maxNumberOfMembers.toString());

            if (imageFile) {
                formData.append("imageFile", imageFile); // Добавляем файл в FormData
            }

            if (!(id === "")) {
                await EventsService.updateEvent(id, formData);
                navigate("/admin_page");
            } else {
                await EventsService.addEvent(formData);
                navigate("/admin_page");
            }
        } catch (e: any) {
            console.error(e.response.data);
        }
    }

    return (
        <form className='EditForm' onSubmit={handleEdit}>
            {eventId === "" ? (
                <h1>ADD EVENT</h1>
            ) : (
                <h1>EDIT EVENT</h1>
            )}
            <input
                type="text"
                name="title"
                placeholder="Enter Title"
                value={title}
                onChange={e => setTitle(e.target.value)}
                required
            />
            <input
                type="text"
                name="description"
                placeholder="Enter Description"
                value={description}
                onChange={e => setDescription(e.target.value)}
                required
            />
            <input
                type="date"
                name="date"
                value={date}
                onChange={e => setDate(e.target.value)}
                required
            />
            <select value={locationId} onChange={e => setLocationId(e.target.value)}>
                {locations.map(location => (
                    <option key={location.id} value={location.id}>{location.title}</option>
                ))}
                <option value="another">Another</option>
            </select>
            {locationId === "another" && (
                <input
                    type="text"
                    name="newLocationTitle"
                    placeholder="Enter title of location"
                    value={newLocationTitle}
                    onChange={e => setNewLocationTitle(e.target.value)}
                    required
                />
            )}

            <select value={categoryId} onChange={e => setCategoryId(e.target.value)}>
                {categories.map(category => (
                    <option key={category.id} value={category.id}>{category.title}</option>
                ))}
                <option value="another">Another</option>
            </select>
            {categoryId === "another" && (
                <input
                    type="text"
                    name="newCategoryTitle"
                    placeholder="Enter title of category"
                    value={newCategoryTitle}
                    onChange={e => setNewCategoryTitle(e.target.value)}
                    required
                />
            )}
            <input
                type="number"
                name="maxNumberOfMembers"
                placeholder="Enter maximum number of members"
                value={maxNumberOfMembers}
                onChange={e => setMaxNumberOfMembers(parseInt(e.target.value))}
                required
            />
            <input
                type="file"
                name="imageFile"
                onChange={e => setImageFile(e.target.files ? e.target.files[0] : null)} // Обработчик выбора файла
            />
            {eventId === "" ? (
                <button type="submit">Add</button>
            ) : (
                <button type="submit">Edit</button>
            )}
        </form>
    );
};

export default observer(EditEventForm);
