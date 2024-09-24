import React, {useContext, useState} from 'react';
import './styles.css';
import {useNavigate} from "react-router-dom";
import {Context} from "../../../index";


const LoginForm = () => {
    const navigate = useNavigate();
    const [userName, setLogin] = useState<string>('');
    const [password, setPassword] = useState<string>('');
    const {store} = useContext(Context);

    const handleLogin = async (e: React.FormEvent) => {
        e.preventDefault();
        try{
            await store.Login(userName, password);
            navigate("/");
        } catch (e: any){
            console.error(e.response.data);
        }
    }

    return (
        <form className='LoginForm' onSubmit={handleLogin}>
            <h1>LOG IN</h1>
            <input
                type="text"
                name="username"
                placeholder="Enter Login"
                value={userName}
                onChange={e => setLogin(e.target.value)}
                required
            />
            <input
                type="password"
                name="password"
                placeholder="Enter Password"
                value={password}
                onChange={e => setPassword(e.target.value)}
                required
            />
            <button type="submit">Log in</button>
        </form>
    );
};

export default LoginForm;
