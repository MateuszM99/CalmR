import axios from 'axios'

const baseUrl = "https://localhost:5001";

export function getPsychologistsListRequest(pageIndex, searchText){
    return axios.get(`${baseUrl}/api/psychologist/get/psychologists?pageIndex=${pageIndex}&s=${searchText}`);
}

export function getPsychologistsProfile(){
    return axios.get(`${baseUrl}/api/psychologist/get/psychologistProfile`);
}

export function editPsychologistProfile(editProfileData){
    const config = {
        headers: { 'content-type': 'multipart/form-data' }
    }
    let formData = new FormData();
    formData.append('psychologistId', editProfileData.psychologistId);
    formData.append('firstName', editProfileData.firstName);
    formData.append('lastName', editProfileData.lastName);
    formData.append('description', editProfileData.description);
    formData.append('costPerHour', editProfileData.costPerHour);
    formData.append('profileImage', editProfileData.profileImage);
    formData.append('country', editProfileData.country);
    formData.append('city', editProfileData.city);
    formData.append('addressLine1', editProfileData.addressLine1);
    formData.append('addressLine2', editProfileData.addressLine2);
    formData.append('zipCode', editProfileData.zipCode);

    return axios.post(`${baseUrl}/api/psychologist/editPsychologistProfile`, formData, config);
}