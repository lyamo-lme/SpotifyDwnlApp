import {useState} from "react";
import {loginRedirect} from "../../app/services/auth";

export const LoginPage = () => {
    
    const [loginData, setLogin] = useState({} as LoginType);
    const {email, password} = loginData;
    
    
    return (
        <form>
            <div>
                <label>Email</label>
                <input value={email}/>
            </div>
            <div>
                <label>Password</label>
                <input
                    type={"password"}
                    value={password}
                    onChange={(e) => setLogin({...loginData, password: e.target.value})}/>
            </div>
            <div>
                <button type={"reset"} onClick={()=>loginRedirect()} >Log in</button>
            </div>
        </form>
    );
}

type LoginType = {
    email: string,
    password: string
}