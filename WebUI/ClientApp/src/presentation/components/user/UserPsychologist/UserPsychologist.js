import React, { useEffect, useState } from 'react'
import styled from "styled-components";
import MapPin from '../../../../application/assets/map-marker-alt-solid.svg';
import Money from '../../../../application/assets/money-bill-wave-solid.svg';
import Search from '../../../../application/assets/search-solid.svg';
import { getPsychologistsListRequest } from '../../../../infrastructure/services/api/psychologists/PsychologistsRequests';

const Container = styled.div`
    margin-top: 2rem;
    align-self: center;
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
`

const SearchBar = styled.div`
    display: flex;
    flex-direction: row;
    align-items: center;
    justify-content: center;
    input {
        height: 40px;
        width: 350px;
        border: none;
        border-radius: 5px;
        background: #fff;
        outline: none;
        padding: 0 60px 0 20px;
        font-size: 14px;
        box-shadow: 1px 1px 10px rgba(0,0,0,.2);
    }

    .search-icon {
        margin-left: 10px;
        background-color: black;
        width: 40px;
        height: 40px;
        border-radius: 5px;
        cursor: pointer;
        img {
            height: 20px;
            width: 20px;
        }
    }
`

const ListContainer = styled.div`
    margin-top: 2rem;
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: space-between;
`
const ListItem = styled.div`
    margin-top: 1rem;
    height: 350px;
    width: 500px;
    border-radius: 5px;
    box-shadow: 1px 1px 10px rgba(0,0,0,.2);
    padding: 15px;

    span {
        margin-bottom: 5px;
        
        img {
            float: left;
            height: 100px;
            width: 100px;
            border-radius: 3px;
            margin-right: 10px;
        }   
    }

    p {
        font-size: 12px;
        color: grey;
    }

    .actions {
        display flex;
        justify-content: flex-end;
        margin-left: auto;
        a {
            display: flex;
            align-items: center;
            justify-content: center;
            height: 40px;
            width: 150px;
            background-color: black;
            border-radius: 3px;
            color: white;
            font-size: 12px;
            margin-left: 15px;
        }
    }

    .line {
        display:block;
        width:100%;
        height: 1px;
        border-top: 1px solid grey;
        margin: 3px 0px 15px 0px;
    }

    .icon {
        height: 20px;
        width: 20px;
        color: grey;
    }
`



function UserPsychologist() {

    const [psychologistList, setPsychologistList] = useState(null);
    const [isLoaded, setIsLoaded] = useState(false);

    const getPsychologistList = async () => {
        try{
            let response = await getPsychologistsListRequest();
            console.log(response);
            setPsychologistList(response.data);
            setIsLoaded(true);
        } catch(err){
            
        }  
    }

    useEffect(() => {    
        getPsychologistList();
    },[]);

    return (
        <Container>
            <SearchBar>
                <input type="text" placeholder="Type to search.."/>
                <div className="search-icon">
                    <img src={Search} alt="Search"></img>
                </div>
            </SearchBar>
            <ListContainer>
                {psychologistList?.map((psychologist => 
                        <ListItem>
                        <span>
                            <img alt="profile" src={psychologist.profileImageUrl}></img>
                            <h4>{psychologist.firstName} {psychologist.lastName}</h4>
                            <h7>Psycholog</h7>
                        </span>
                        <p>{psychologist.description}</p>
                        <span className="actions">
                            <a>Make appointment</a>
                            <a>Send a message</a>
                        </span>
                        <span className="line"></span>
                        <span>
                            <img className="icon" src={MapPin} alt="MapPin"/>
                            <p>{psychologist.country} {psychologist.city} {psychologist.street}, {psychologist.houseNumber} {psychologist.apartmentNumber}</p>
                        </span>
                        <span>
                            <img className="icon" src={Money} alt="Price"/>
                            <p>{psychologist.costPerHour}</p>
                        </span>
                    </ListItem>
                ))}
            </ListContainer>
        </Container>
    )
}

export default UserPsychologist
