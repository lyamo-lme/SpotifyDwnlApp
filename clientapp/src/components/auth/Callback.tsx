import React from "react";
import {CallbackComponent} from "redux-oidc";
import userManager from "../../app/services/utils/userManager";
import {User} from 'oidc-client';
export const Callback = () => {
    console.log("auth")
    const callBackProps = {
        userManager,
        successCallback: (user:User)=>{
            console.log(user);
        },
        errorCallback: ()=>{}
    }
    return (
        <CallbackComponent {...callBackProps}>
            <div>Redirecting...</div>
        </CallbackComponent>)
}