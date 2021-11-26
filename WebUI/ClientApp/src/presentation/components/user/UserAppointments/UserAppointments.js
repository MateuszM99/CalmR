import React from 'react'
import styled from "styled-components";
import Search from '../../../../application/assets/search-solid.svg';

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
`
const ListItem = styled.div`
    margin-top: 1rem;
    height: 350px;
    width: 700px;
    border-radius: 5px;
    box-shadow: 1px 1px 10px rgba(0,0,0,.2);
    padding: 15px;
    display: flex;

    .actions {
        margin-top: 50px;
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
`

const ListItemContainer = styled.div`
    display: flex;
    flex-direction: row;
`

const ListItemRow = styled.div `
    display: flex;
    flex-direction: column;

    label {
        font-size: 16px;
        font-weight: 700;
    }
`

const InputContainer = styled.span`
    display: flex;
    align-items: center;

    img {
        height: 40px;
        width: 40px;
        border-radius: 50%;
        margin-right: 10px;
    }

    p {
        font-size: 14px;
        color: grey;
    }
`

function UserAppointments() {
    return (
        <Container>
            <SearchBar>
                <input type="text" placeholder="Type to search.."/>
                <div className="search-icon">
                    <img src={Search} alt="Search"></img>
                </div>
            </SearchBar>
            <ListContainer>
                <ListItem>
                    <ListItemContainer>
                        <ListItemRow>
                            <label>Doctor:</label>
                            <InputContainer>
                                <img src="https://s3-eu-west-1.amazonaws.com/znanylekarz.pl/doctor/866515/8665158d72bdce9242f7d1f1b526b19e_large.jpg"></img>
                                <p>mgr Krzysztof Zawada</p>
                            </InputContainer>
                            <label>Doctor phone:</label>
                            <InputContainer>
                                <img></img>
                                <p>mgr Krzysztof Zawada</p>
                            </InputContainer>
                            <label>Doctor address:</label>
                            <InputContainer>
                                <img></img>
                                <p>mgr Krzysztof Zawada</p>
                            </InputContainer>
                        </ListItemRow>
                        <ListItemRow>
                            <label>Appointment date:</label>
                            <InputContainer>
                                <img></img>
                                <p>mgr Krzysztof Zawada</p>
                            </InputContainer>
                            <label>Appointment start time:</label>
                            <InputContainer>
                                <img></img>
                                <p>mgr Krzysztof Zawada</p>
                            </InputContainer>
                            <label>Appointment duration time:</label>
                            <InputContainer>
                                <img></img>
                                <p>mgr Krzysztof Zawada</p>
                            </InputContainer>
                    </ListItemRow>
                   </ListItemContainer>
                   <span className="actions">
                        <a>Make appointment</a>
                        <a>Send a message</a>
                    </span>
                </ListItem>
            </ListContainer>
        </Container>
    )
}

export default UserAppointments
