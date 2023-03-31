import userManager from "../../app/services/utils/userManager";
import {ReactEventHandler} from "react";

export const AppHeader = () => {

    const onLoginButtonClick =  () => {
        console.log("here")
         userManager.signinRedirect()
        }
    return (
        <header>
            <nav>
                <li>
                    <button onClick={onLoginButtonClick}>Login</button>
                </li>
            </nav>
        </header>)
}