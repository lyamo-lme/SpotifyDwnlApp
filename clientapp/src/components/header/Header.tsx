import userManager from "../../app/services/utils/userManager";
import {ReactEventHandler} from "react";
import {NavLink} from "react-router-dom";

export const AppHeader = () => {

    const onLoginButtonClick = () => {
        console.log("here")
        userManager.signinRedirect()
    }
    const test = async () => {
        fetch("https://localhost:7020/api/spotify", {
            method: "GET",
            credentials: 'include'
        });
    }
    return (
        <header className={'app-headers'}>
            <div className={'header-block'}>
                <nav className={'menu'}>
                    <li className={'logo'}>
                        <a>Lyamo</a>
                    </li>
                </nav>
                <nav className={'main-menu menu'}>
                    <li>
                       <NavLink to={'/downloader'}>Downloader</NavLink>
                    </li>
                </nav>
                <nav className={'menu'}>
                    <li>
                        <a>Account</a>
                    </li>
                </nav>
            </div>
        </header>)
}