import {useState} from "react";
import {useAppSelector} from "../../app/hooks";

export const LoginPage = () => {
    
    const [loginData, setLogin] = useState({} as LoginType);
    const {email, password} = loginData;
    return (<></>);
}

type LoginType = {
    email: string,
    password: string
}