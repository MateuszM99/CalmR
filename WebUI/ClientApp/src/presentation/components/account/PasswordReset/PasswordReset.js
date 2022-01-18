import React,{useState} from 'react'
import {Formik,Form,Field} from 'formik';
import { useParams, useLocation } from 'react-router';
import * as Yup from 'yup'
import styled from 'styled-components';
import { passwordResetRequest } from '../../../../infrastructure/services/api/auth/AuthRequests';
import { FormContainer } from '../../../../application/common/FormContainer/FormContainer';
import { FormTextInput } from '../../../../application/common/FormTextInput/FormTextInput';
import { FormButton } from '../../../../application/common/FormButton/FormButton';

const Header = styled.h5`
    align-self: center;
    color: white;
`

const ErrorDisplay = styled.div`
    margin-top: 5px;
    font-size: 10px;
    color: white;
`

const Text = styled.p`
    margin-top: 5px;
    font-size: 16px;
    color: white;
`


const phoneRegExp = /^((\\+[1-9]{1,4}[ \\-]*)|(\\([0-9]{2,3}\\)[ \\-]*)|([0-9]{2,4})[ \\-]*)*?[0-9]{3,4}?[ \\-]*[0-9]{3,4}?$/
const passwordRegExp = /^(.{0,7}|[^0-9]*|[^A-Z]*|[a-zA-Z0-9]*)$/

function useQuery() {
    const { search } = useLocation();
  
    return React.useMemo(() => new URLSearchParams(search), [search]);
  }

function PasswordReset() {
    const query = useQuery();
    const [isReset,setIsReset] = useState(false);
    
    if(isReset){
        return (
            <FormContainer height='200px' width='500px'>
                    <Header>Success</Header>
                    <Text>Successfully reset password, you can now login to your account with your new password</Text>
            </FormContainer>
        )
    }

    return (
        <Formik
            initialValues={{
                code : query.get("code"),
                email : query.get("email"),
                password : '',
                confirmPassword: '',
                }}
                validationSchema = {Yup.object({
                code : Yup.string()
                    .required('Reset code is missing'),
                email : Yup.string()
                    .email('Email is invalid')
                    .required('Email is missing'),
                password : Yup.string()
                    //.matches(passwordRegExp,'Password has to be at least 8 characters long,contain 1 special sign and 1 uppercase letter')
                    .required('Password is required'),
                confirmPassword: Yup.string()
                    .required('Password confirmation is required')
                })}
                
                onSubmit = {async (values,{setSubmitting, setStatus, resetForm}) => {

                if(values){
                    try{
                        console.log('is here');
                        await passwordResetRequest(values);
                        setIsReset(true);
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
                <FormContainer height='450px' width='500px'>                           
                    <Header>Reset Password</Header>   
                    <FormTextInput type="text" placeholder="Enter new password" name="password" label="New password"/>  
                    <FormTextInput type="text" placeholder="Enter your new password again" name="confirmPassword" label="Confirm new password"/>                           
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

export default PasswordReset
