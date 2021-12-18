import axios from 'axios'

const baseUrl = "https://localhost:5001";

export function getPsychologistsListRequest(pageIndex, searchText){
    return axios.get(`${baseUrl}/api/psychologist/get/psychologists?pageIndex=${pageIndex}&s=${searchText}`);
}