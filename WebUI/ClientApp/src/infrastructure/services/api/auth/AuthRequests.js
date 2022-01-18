import axios from 'axios'

const baseUrl = "https://localhost:5001";

export function signInRequest(signInData){
    return axios.post(`${baseUrl}/api/authenticate/signIn`,signInData);
}

export function signUpRequest(signUpData){
    return axios.post(`${baseUrl}/api/authenticate/signUp`,signUpData);
}

export function signUpPsychologistRequest(signUpData){
    const config = {
        headers: { 'content-type': 'multipart/form-data' }
    }
    let formData = new FormData();
    formData.append('username', signUpData.username);
    formData.append('email', signUpData.email);
    formData.append('password', signUpData.password);
    formData.append('confirmPassword', signUpData.confirmPassword);
    formData.append('firstName', signUpData.firstName);
    formData.append('lastName', signUpData.lastName);
    formData.append('phoneNumber', signUpData.phoneNumber);
    formData.append('profileImage', signUpData.profileImage);
    formData.append('country', signUpData.country);
    formData.append('city', signUpData.city);
    formData.append('addressLine1', signUpData.addressLine1);
    formData.append('addressLine2', signUpData.addressLine2);
    formData.append('zipCode', signUpData.zipCode);

    return axios.post(`${baseUrl}/api/authenticate/signUp-psychologist`, formData, config);
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
