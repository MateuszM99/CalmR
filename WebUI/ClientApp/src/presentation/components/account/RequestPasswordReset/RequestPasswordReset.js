import React,{useState} from 'react'
import {Formik,Form,Field} from 'formik';
import * as Yup from 'yup'
import { sendPasswordResetRequest } from '../../../../infrastructure/services/api/auth/AuthRequests';
import { FormContainer } from '../../../../application/common/FormContainer/FormContainer';
import styled from 'styled-components';
import { FormTextInput } from '../../../../application/common/FormTextInput/FormTextInput';
import { FormButton } from '../../../../application/common/FormButton/FormButton';

const Header = styled.h3`
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


function RequestPasswordReset() {
    const [isSent,setIsSent] = useState(false);
    
    if(isSent){
        return (
            <FormContainer height='200px' width='400px'> 
                    <Header>Success</Header>  
                    <Text>Sent password reset link, now go to your email and confirm your account</Text>
            </FormContainer>
        )
    }

    return (
        <Formik
            initialValues={{
                email : '',
                }}
                validationSchema = {Yup.object({
                    email : Yup.string()
                        .required('Email is required')
                        .email('Email is not valid'),
                })}
                
                onSubmit = {async (values,{setSubmitting, setStatus, resetForm}) => {
                if(values){
                    try{
                        await sendPasswordResetRequest(values);
                        setIsSent(true);
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
                <FormContainer height='300px' width='400px'>                           
                    <Header>Reset Password</Header>  
                    <FormTextInput type="text" placeholder="Enter your email" name="email"/>                              
                    <FormButton width="250px" height="48px">{isSubmitting ? 'Reseting password ...' : 'Reset password'}</FormButton> 
                    {status && status.errorMessage ? (
                        <ErrorDisplay>{status.errorMessage}</ErrorDisplay>
                    ) : null}            
                </FormContainer>
            </Form>
            )}
        </Formik>
    )
}

export default RequestPasswordReset
