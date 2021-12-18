import React, { useState } from 'react';
import {Formik,Form,Field} from 'formik';
import * as Yup from 'yup';
import styled from 'styled-components';
import { resendConfirmationEmailRequest } from '../../../../infrastructure/services/api/auth/AuthRequests';
import { FormContainer } from '../../../../application/common/FormContainer/FormContainer';
import { FormTextInput } from '../../../../application/common/FormTextInput/FormTextInput';
import { FormButton } from '../../../../application/common/FormButton/FormButton';

const Header = styled.h5`
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


function ResendEmailConfirmation() {
    const [isSent,setIsSent] = useState(false);
    
    if(isSent){
        return (
            <FormContainer height='200px' width='500px'>
                    <Header>Success</Header>
                    <Text>Successfully sent confirmation email, now go to your email and confirm your account</Text>
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
                            await resendConfirmationEmailRequest(values);
                            setIsSent(true);
                        } catch(err){
                            console.log(err.response.data.title);
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
                    <FormContainer height='350px' width='500px'>                            
                        <Header>Resend email confirmation</Header>        
                        <FormTextInput type="text" placeholder="Enter your email" name="email" label="E-mail"/>                           
                        <FormButton width="250px" height="48px">{isSubmitting ? 'Sending ...' : 'Send confirmation link'}</FormButton> 
                        {status && status.errorMessage ? (
                            <ErrorDisplay>{status.errorMessage}</ErrorDisplay>
                        ) : null}            
                   </FormContainer>
                </Form>
                )}
            </Formik>
    )
}

export default ResendEmailConfirmation
