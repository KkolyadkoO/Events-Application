import React, {useContext, useState} from 'react';
import {useNavigate} from "react-router-dom";
import './styles.css';
import {Context} from "../../../index";

const RegistrationForm = () => {
    const navigate = useNavigate();
    const {store} = useContext(Context);


    const [formData, setFormData] = useState({
        userName: '',
        userEmail: '',
        password: '',
        role: 'User'
    });

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setFormData({
            ...formData,
            [e.target.name]: e.target.value,
        });
    };

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        try {
            await store.Registration(formData.userName, formData.userEmail, formData.password, formData.role);
            navigate("/login");
        } catch (e: any) {
            console.error(e.response.data);
        }
    };

    return (
        <form className='RegistrationForm' onSubmit={handleSubmit}>
            <h1>Registration</h1>
            <input
                type="text"
                name="userName"
                placeholder="Enter Username"
                value={formData.userName}
                onChange={handleChange}
                required
            />
            <input
                type="email"
                name="userEmail"
                placeholder="Enter Email"
                value={formData.userEmail}
                onChange={handleChange}
                required
            />
            <input
                type="password"
                name="password"
                placeholder="Enter Password"
                value={formData.password}
                onChange={handleChange}
                required
            />
            <button type="submit">Register</button>
        </form>
    );
};

export default RegistrationForm;
