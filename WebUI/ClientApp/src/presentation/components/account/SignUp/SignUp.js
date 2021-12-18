import React, { Component,useState, useEffect } from 'react'
import {Formik,Form, yupToFormErrors,Field} from 'formik'
import * as Yup from 'yup'
import {Link, Redirect} from 'react-router-dom'
import styled from "styled-components";
import { signUpRequest } from '../../../../infrastructure/services/api/auth/AuthRequests'
import { FormContainer } from '../../../../application/common/FormContainer/FormContainer';
import { FormTextInput } from '../../../../application/common/FormTextInput/FormTextInput';
import { FormButton } from '../../../../application/common/FormButton/FormButton';
import { RedirectLink } from '../../../../application/common/RedirectLink/RedirectLink';


const Header = styled.h5`
    margin-top: 15px;
    align-self: center;
    color: grey;
`

const ErrorDisplay = styled.div`
    margin-top: 5px;
    font-size: 10px;
    color: grey;
`

const Text = styled.p`
    margin-top: 5px;
    font-size: 16px;
    color: grey;
`

const phoneRegExp = /^((\\+[1-9]{1,4}[ \\-]*)|(\\([0-9]{2,3}\\)[ \\-]*)|([0-9]{2,4})[ \\-]*)*?[0-9]{3,4}?[ \\-]*[0-9]{3,4}?$/
const passwordRegExp = /^(.{0,7}|[^0-9]*|[^A-Z]*|[a-zA-Z0-9]*)$/

function SignUp() {
    const [didSignUp,setDidSignUp] = useState(false);

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

    if(didSignUp){
        return (
            <FormContainer height='200px' width='450px'>
                    <Header>Success</Header>
                    <Text>You successfully signed up, now go to your email and confirm your account.</Text>
            </FormContainer>
        )
    }

    if(!didSignUp){
        return (  
                <Formik
                    initialValues={{
                        username : '',
                        email : '',
                        password : '',
                        confirmPassword: '',
                    }}
                    validationSchema = {Yup.object({
                        username : Yup.string()
                            .required('Username is required'),
                        email : Yup.string()
                            .email('Invalid email')
                            .required('Email is required'),
                        password : Yup.string()
                            //.matches(passwordRegExp,'Password has to be at least 8 characters long,contain 1 special sign and 1 uppercase letter')
                            .required('Password is required'),
                        confirmPassword: Yup.string()
                            .oneOf([Yup.ref('password'), null], 'Passwords must match')
                            .required('You must confirm your password')
                    })}

                    onSubmit = {async (values,{setSubmitting,setStatus,resetForm}) => {
                        if(values){
                            try{
                                let response = await signUpRequest(values);
                                setSubmitting(false);
                                setDidSignUp(true);
                                resetForm();
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
                    {({ errors, touched,status,isSubmitting }) => (
                    <Form>  
                        <FormContainer height='850px' width='500px'>
                            <Header>Sign up</Header>       
                            <FormTextInput type="text" placeholder="Enter your username" name="username" label="Username"/>
                            <FormTextInput type="text" placeholder="Enter your email" name="email" label="E-mail"/>
                            <FormTextInput type="password" placeholder="Enter your password" name="password" label="Password"/>
                            <FormTextInput type="password" placeholder="Enter your password again" name="confirmPassword" label="Confirm Password"/>
                            <FormButton width="250px" height="48px" type="submit">{isSubmitting ? 'Signin up ...' : 'Sign up'}</FormButton>
                            {status && status.errorMessage ? (
                                    <ErrorDisplay>{status.errorMessage}</ErrorDisplay>
                                ) : null}
                            <RedirectLink to="/signIn">Already have an account? Login</RedirectLink>
                        </FormContainer>
                    </Form>
                    )}
                </Formik>
        )
    }
}

export default SignUp