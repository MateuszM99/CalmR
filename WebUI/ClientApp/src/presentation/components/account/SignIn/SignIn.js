import React,{useState,useEffect} from 'react';
import {Formik,Form,Field} from 'formik';
import * as Yup from 'yup'
import {Link,withRouter,useHistory, Redirect} from "react-router-dom"
import styled from "styled-components";
import { signInRequest } from '../../../../infrastructure/services/api/auth/AuthRequests';
import { FormContainer } from '../../../../application/common/FormContainer/FormContainer';
import { FormTextInput } from '../../../../application/common/FormTextInput/FormTextInput';
import { FormButton } from '../../../../application/common/FormButton/FormButton';
import { RedirectLink } from '../../../../application/common/RedirectLink/RedirectLink';


const Header = styled.h5`
    align-self: center;
    color: white;
`

const ErrorDisplay = styled.div`
    display: flex;
    align-items: center;
    margin-top: 5px;
    margin-bottom: 20px;
    font-size: 10px;
    color: white;
`


function SignIn() {
    const history = useHistory();
    const [isSignedIn,setIsSignedIn] = useState(false);

    const handleLoggedIn = () => {
        if(localStorage.getItem("userData") != null){
            setIsSignedIn(true);
        }
    }

    useEffect(() => {    
        handleLoggedIn();
    });

    if(isSignedIn){
        return <Redirect to="/app/home"/>
    }

    return (  
        <Formik
            initialValues={{
                username : '',
                password : '',
                }}
                validationSchema = {Yup.object({
                    username : Yup.string()
                        .required('Username is required'),
                    password : Yup.string()
                        .required('Password is required'),
                })}
                
                onSubmit = {async (values,{setSubmitting, setStatus, resetForm}) => {
                if(values){
                    try{
                        let response = await signInRequest(values);
                        localStorage.setItem('userData',JSON.stringify(response.data));
                        history.push('/app/home')
                    } catch(err){
                        setSubmitting(false);
                        resetForm();
                        setStatus({
                            errorMessage : err.response.data.title
                        });
                    }                                                                                                                                                                                        
                }      
            }}
        >
            {({ errors, touched,isSubmitting,status}) => (
            <Form>
                <FormContainer height='550px' width='500px'>                       
                    <Header>Sign in</Header>             
                    <FormTextInput type="text" name="username" label="Username" placeholder="Enter your username"/>                       
                    <FormTextInput type="password" name="password" label="Password" placeholder="Enter your password"/>                  
                    <FormButton width="250px" height="48px" type="submit">{isSubmitting ? 'Signin in ...' : 'Sign in'}</FormButton> 
                    {status && status.errorMessage ? (
                        <ErrorDisplay>{status.errorMessage}</ErrorDisplay>
                    ) : null}            
                    <RedirectLink to="/signup">Don't have account yet? Sign up</RedirectLink> 
                    <RedirectLink to="/account/request-reset">Forgot your password ?</RedirectLink> 
                </FormContainer>   
            </Form>
            )}
        </Formik>
    );
}

export default SignIn;