import React,{useState, useEffect} from 'react';
import {Link,useParams, useLocation} from 'react-router-dom';
import axios from 'axios';
import { confirmEmailRequest } from '../../../../infrastructure/services/api/auth/AuthRequests';

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
            <div>
                <h1>Hello</h1>
                <p>Your account is confirmed <Link to="/">go to main page</Link></p>
            </div>
        )
    }
    if(errorMessage){
        return (
            <div>
                <p>Something went wrong try resending confirmation <Link to="/account/resend-confirm">here</Link></p>
            </div>
        )
    }

    return (
        <div>
        </div>
    )
}

export default ConfirmEmail
