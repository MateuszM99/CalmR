import React,{useState, useEffect} from 'react';
import {Link,useParams, useLocation} from 'react-router-dom';
import axios from 'axios';
import { confirmEmailRequest } from '../../../../infrastructure/services/api/auth/AuthRequests';
import { FormContainer } from '../../../../application/common/FormContainer/FormContainer';
import styled from 'styled-components';
import { RedirectLink } from '../../../../application/common/RedirectLink/RedirectLink';


const Header = styled.h3`
    align-self: center;
    color: grey;
`

const Text = styled.p`
    margin-top: 5px;
    font-size: 16px;
    color: grey;
`

function useQuery() {
    const { search } = useLocation();
  
    return React.useMemo(() => new URLSearchParams(search), [search]);
  }

function ConfirmEmail() {
    const [isConfirmed,setIsConfirmed] = useState(false);
    const [errorMessage,setErrorMessage] = useState(null);
    const query = useQuery();
    const handleEmailConfirmation = async () => {
        try{
            const code = query.get("code");
            const email = query.get("email");
            let data = {code: code,email: email}
            await confirmEmailRequest(data);
            setIsConfirmed(true);
        } catch(err){
            setErrorMessage(err.response.data.title);
        }    
    }

    useEffect(() => {    
        if(!isConfirmed){
            handleEmailConfirmation();
        }
    });

    if(isConfirmed){
        return (
            <FormContainer height='200px' width='550px'>
                <Header>Hello!</Header>
                <Text>Your account is confirmed <RedirectLink fontSize="16px" to="/">go to main page.</RedirectLink></Text>
            </FormContainer>
        )
    }
    
    if(errorMessage){
        return (
            <FormContainer height='200px' width='550px'>
                <Header>Hello!</Header>
                <Text>Something went wrong try resending confirmation <RedirectLink fontSize="16px" to="/account/resend-confirm">here.</RedirectLink></Text>
            </FormContainer>
        )
    }

    return (
        <div>
        </div>
    )
}

export default ConfirmEmail
