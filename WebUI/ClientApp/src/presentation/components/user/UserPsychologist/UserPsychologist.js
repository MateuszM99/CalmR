import React, { useEffect, useState } from 'react'
import { useHistory, useLocation } from 'react-router';
import styled from "styled-components";
import SearchIcon from '@mui/icons-material/Search';
import { Button} from '@mui/material';
import { createConversationRequest, getUserPsychologistConversationRequest } from '../../../../infrastructure/services/api/conversations/ConversationsRequests';
import { getPsychologistsListRequest } from '../../../../infrastructure/services/api/psychologists/PsychologistsRequests';
import AppointmentDialogForm from '../AppointmentForm/AppointmentDialogForm';
import PaidIcon from '@mui/icons-material/Paid';
import PlaceIcon from '@mui/icons-material/Place';
import { FormButton } from '../../../../application/common/FormButton/FormButton';
import useQueryString from "../../../../application/hooks/useQueryString";
import Pagination from '@mui/material/Pagination';
import { makeStyles } from '@material-ui/core';

const Container = styled.div`
    margin-top: 2rem;
    align-self: center;
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
`

export const SearchBar = styled.div`
    display: flex;
    align-items: center;
    justify-content: center;
    width: 422px;
    font-family: "Poppins", sans-serif;
    .search-box {
        height: 42px;
        width: 368px;
        border: none;
        background: #fff;
        outline: none;
        padding: 0 60px 0 20px;
        font-size: 16px;
        color: grey;
        border-radius: 5px 0px 0px 5px;
    }

    .search-icon {
        background: #3232C1;
        width: 64px;
        height: 42px;
        cursor: pointer;
        color: white;
        display: flex;
        align-items: center;
        justify-content: center;
        border-radius: 0px 5px 5px 0px;
    }
`

const ListContainer = styled.div`
    margin-top: 2rem;
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: space-between;
    font-family: "Poppins", sans-serif;
`
const ListItem = styled.div`
    margin-top: 1rem;
    height: 480px;
    width: 600px;
    background: white;
    border-radius: 5px;
    box-shadow: 1px 1px 3px rgba(0,0,0,.2);
    padding: 30px;

    span {
        margin-bottom: 5px;
        
        img {
            float: left;
            height: 100px;
            width: 100px;
            border-radius: 50%;
            margin-right: 10px;
        }   
    }

    .description {
        padding-top: 50px;
        height: 125px;
        font-size: 13px;
        color: grey;
    }

    .actions {
        display: flex;
        justify-content: space-between;
        padding-bottom: 10px;
    }

    .line {
        display:block;
        width:100%;
        height: 2px;
        border-top: 2px solid grey;
        margin: 3px 0px 15px 0px;
    }

    .info {
        display: flex;
        flex-direction: row;
        align-items: center;
        height: 36px;
        p {
            padding: 0;
            margin: 0;
            margin-left: 10px;
            font-size: 13px;
            font-weight: 500;
            color: grey;
        }
    }
`

const useStyles = makeStyles(() => ({
    ul: {
      "& .MuiPaginationItem-root": {
        color : 'white'
      }
    }
  }));



function UserPsychologist() {
    const classes = useStyles();
    const history = useHistory();
    const [totalPages, setTotalPages] = useState(0);
    const [currentPageIndex, onSetCurrentPageIndex] = useQueryString("pageIndex", 1);
    const [textSearch, onSetTextSearch] = useQueryString("s", "");
    const [psychologistList, setPsychologistList] = useState(null);
    const [isLoaded, setIsLoaded] = useState(false);
    const [open, setOpen] = useState(false);
    const [selectedPsychologist, setSelectedPsychologist] = useState(null);

    const handleClickOpen = (psychologist) => {
        setSelectedPsychologist(psychologist);
        console.log(psychologist);
        setOpen(true);
    };

    const handleClose = () => {
        setOpen(false);
    };

    const handleSendMessageClick = async (psychologistId) => {
        try{
            let conversationId = null;
            let response = await getUserPsychologistConversationRequest(psychologistId);
            
            conversationId = response.data?.id;

            if(!response.data){
                let createResponse = await createConversationRequest({psychologistId: psychologistId});
                conversationId = createResponse.data;
            }
            
            history.push(`/app/chat/${conversationId}`);
        } catch(err){
            
        } 
    }

    const getPsychologistList = async (pageIndex, s) => {
        try
        {
            let response = await getPsychologistsListRequest(pageIndex, s);
            console.log(response.data);
            setPsychologistList(response.data.data);
            setTotalPages(response.data.totalPages);
            setIsLoaded(true);
        } 
        catch(err){
            
        }  
    }

    const handleTextSearchChange = (e) => {
        onSetTextSearch(e.target.value);
    }

    const handlePageChange = (event,pageNumber) => {
        onSetCurrentPageIndex(pageNumber);
    }

    useEffect(() => {    
        getPsychologistList(currentPageIndex, textSearch);
    },[currentPageIndex]);

    return (
        <div>
            <Container>
                <SearchBar>
                    <input className='search-box' type="text" placeholder="Type to search ..." onChange={handleTextSearchChange}/>
                    <div className="search-icon" onClick={() => getPsychologistList(currentPageIndex, textSearch)}>
                        <SearchIcon fontSize="medium"/>
                    </div>
                </SearchBar>
                <ListContainer>
                    {psychologistList?.map(((psychologist,index) => 
                        <ListItem key={index}> 
                            <span>
                                <img alt="profile" src={psychologist.profileImageUrl}></img>
                                <h4>{psychologist.firstName} {psychologist.lastName}</h4>
                                <h6>Psycholog</h6>
                            </span>
                            <div className="description">
                                <p>"{psychologist.description}"</p>
                            </div>
                            <span className="actions">
                                <FormButton width="200px" height="42px" fontSize="12px" onClick={() => handleClickOpen(psychologist)}>Make appointment</FormButton>
                                <FormButton width="200px" height="42px" fontSize="12px" onClick={() => handleSendMessageClick(psychologist.id)}>Send a message</FormButton>
                            </span>
                            <span className="line"></span>
                            <span className="info">
                                <PlaceIcon fontSize="medium"/>
                                <p>{psychologist.country} &bull; {psychologist.city} &bull; {psychologist.addressLine1}</p>
                            </span>
                            <span className="info">
                                <PaidIcon fontSize="medium"/>
                                <p>Online consultation &bull; {psychologist.costPerHour}$</p>
                            </span>
                        </ListItem>
                    ))}
                </ListContainer>
                <Pagination count={totalPages} classes={{ul: classes.ul}} defaultPage={1} page={currentPageIndex} onChange={handlePageChange} style={{margin:"30px 0px"}}/>
            </Container>
            <AppointmentDialogForm
                psychologist = {selectedPsychologist}
                open={open}
                onClose={handleClose}
            />
        </div>
    )
}

export default UserPsychologist
