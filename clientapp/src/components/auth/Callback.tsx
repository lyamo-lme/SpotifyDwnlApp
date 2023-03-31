import React from "react";
import {CallbackComponent} from "redux-oidc";
import userManager from "../../app/services/utils/userManager";

export const Callback = () => {
    console.log("auth")
    const callBackProps = {
        userManager,
        successCallback: ()=>{
            window.location.replace("/");
        },
        errorCallback: ()=>{}
    }
    return (
        <CallbackComponent {...callBackProps}>
            <div>Redirecting...</div>
        </CallbackComponent>)
}