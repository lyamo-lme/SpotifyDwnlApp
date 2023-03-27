import {config} from "../configoidc";

export const loginRedirect = () => {
    let authUrl: string = `/connect/authorize/callback` +
        `?client_id=${config.clientId}` +
        `&redirect_uri=${encodeURIComponent("https://localhost:3000/signin")}` +
        `&response_type=${encodeURIComponent("id_token token")}` +
        `&scope=${encodeURIComponent("openid profile")}` +
        `&nonce=asdjoiQJIUQI12jojroi2ji12ro1p23jrp1io2j3rpio12jp3orj21p3iorjonv` +
        `&state=KJHKJHQKJHDkjdfjkafjkaksjfkjqoijfoijweiofjoiwjefjqwi1i312`;
    
    let returnUrl = encodeURIComponent(authUrl);
    console.log(returnUrl);
    let redirectUrl = "https://localhost:7187/Auth/Login?returnUrl=" + returnUrl;
    console.log(redirectUrl);
    window.location.replace(redirectUrl);
};