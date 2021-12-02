import axios from 'axios'

const baseUrl = "https://localhost:5001";

export function signInRequest(signInData){
    return axios.post(`${baseUrl}/api/authenticate/signIn`,signInData);
}

export function signUpRequest(signUpData){
    return axios.post(`${baseUrl}/api/authenticate/signUp`,signUpData);
}

export function signUpPsychologistRequest(signUpData){
    return axios.post(`${baseUrl}/api/authenticate/signUp-psychologist`,signUpData);
}

export function confirmEmailRequest(confirmEmailData){
    console.log(confirmEmailData);
    return axios.post(`${baseUrl}/api/authenticate/confirm`,confirmEmailData)
}

export function resendConfirmationEmailRequest(resendConfirmationEmailData){
    return axios.post(`${baseUrl}/api/authenticate/resend-confirm`,resendConfirmationEmailData)
}

export function sendPasswordResetRequest(sendPasswordResetData){
    return axios.post(`${baseUrl}/api/authenticate/request-reset`,sendPasswordResetData)
}

export function passwordResetRequest(passwordResetData){
    return axios.post(`${baseUrl}/api/authenticate/reset`,passwordResetData)
}
