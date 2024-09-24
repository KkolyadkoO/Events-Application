import React, {useContext, useEffect, useState} from 'react';
import {useNavigate} from "react-router-dom";
import './styles.css';
import {Context} from "../../../index";
import {MembersService} from "../../../api/services/MembersService";

type Props = {
    id: string;
}

const RegistrationOnEventForm = ({id}: Props) => {
    const navigate = useNavigate();
    const {store} = useContext(Context);


    const [formData, setFormData] = useState({
        name: '',
        lastname: '',
        birthday: '',
        email: '',
        userId: store.user.id,
        eventId: id,
    });

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setFormData({
            ...formData,
            [e.target.name]: e.target.value,
        });
    };
    useEffect(() => {
        console.log(formData);
    }, [formData]);

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        try {

            await MembersService.addMember(formData.name, formData.lastname, new Date(formData.birthday).toISOString(),
                formData.email, formData.userId, formData.eventId);
            navigate(`/view_event/${formData.eventId}`);
        } catch (e: any) {
            console.error(e.response.data);
        }
    };

    return (
        <form className='RegistrationOnEventForm' onSubmit={handleSubmit}>
            <h1>Registration On Event</h1>
            <input
                type="text"
                name="name"
                placeholder="Enter Name"
                value={formData.name}
                onChange={handleChange}
                required
            />
            <input
                type="text"
                name="lastname"
                placeholder="Enter LastName"
                value={formData.lastname}
                onChange={handleChange}
                required
            />
            <input
                type="date"
                name="birthday"
                value={formData.birthday}
                onChange={handleChange}
                required
            />
            <input
                type="email"
                name="email"
                placeholder="Enter Email"
                value={formData.email}
                onChange={handleChange}
                required
            />
            <button type="submit">Register</button>
        </form>
    );
};

export default RegistrationOnEventForm;
