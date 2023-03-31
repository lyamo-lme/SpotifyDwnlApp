import React from 'react';
import './App.css';
import {LoginPage} from "./components/auth/LoginPage";
import { Route, Routes} from 'react-router-dom';
import {Callback} from './components/auth/Callback';
import {AppHeader} from "./components/header/Header";

function App() {
    return (
        <>
            <AppHeader/>
            <Routes>
                <Route path={"/"} Component={LoginPage}/>
                <Route path={"/callback"} Component={Callback}/>
            </Routes>
        </>
    );
}

export default App;
