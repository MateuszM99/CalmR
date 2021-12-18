import React, { useEffect, useRef, useState } from 'react'
import Dialog from '@mui/material/Dialog';
import DatePicker from '@mui/lab/DatePicker';
import AdapterDateMoment from '@mui/lab/AdapterMoment';
import LocalizationProvider from '@mui/lab/LocalizationProvider';
import Grid from '@mui/material/Grid';
import CloseIcon from '@mui/icons-material/Close';
import { Form, Formik } from 'formik';
import * as Yup from 'yup'
import PropTypes from 'prop-types';
import { Avatar, TextField } from '@mui/material';
import { getAppointmentsAvailableHoursRequest, updateAppointmentRequest } from '../../../../infrastructure/services/api/appointments/AppointmentsRequests';
import * as moment from 'moment';
import { AppointmentButton, AppointmentFormContainer, DateItem, DatePickerContainer, DialogContainer,Header,PriceContainer, PsychologistData, PsychologistDisplay, ErrorDisplay } from './style';

function ChangeAppointmentDateForm(props) {

    const { onClose, open, psychologist, appointmentId } = props;
    const [selectedDate, setSelectedDate] = React.useState(moment());
    const [availableDates, setAvailableDates] = useState(null);
    const [chosenDateIndex, setChosenDateIndex] = useState(null);

    useEffect(() => {
        console.log(appointmentId);
        setChosenDateIndex(null);
        getAvaialableHoursList();
    }, [selectedDate])

    const handleClose = () => {
        setChosenDateIndex(null);
        onClose();
      };

    const handleDateChange = (appointmentDate) => {        
            setSelectedDate(appointmentDate);
    }

    const getAvaialableHoursList = async () => {

        setAvailableDates(null);

        if(selectedDate)
        {
            try{
                let response = await getAppointmentsAvailableHoursRequest(1, selectedDate.format('YYYY-MM-DD'), 1);
                setAvailableDates(response.data);
            } catch(err){
                console.log(err);
            }    
        }
    }

    return (
        <Dialog onClose={handleClose} open={open} maxWidth="xs">
            <DialogContainer>
                <CloseIcon onClick={handleClose} className="close-icon"/>
                <Header>Change appointment date</Header>
                <AppointmentFormContainer>
                    <Formik                            
                            initialValues={{
                                appointmentId : appointmentId,
                                appointmentDurationTime: 1,
                                appointmentDate: '',
                            }}
                            validationSchema = {Yup.object({
                                appointmentId : Yup.number()
                                                    .integer()
                                                    .required(),
                                appointmentDurationTime : Yup.number()
                                                            .integer()
                                                            .moreThan(0)
                                                            .required('You must specify appointment duration'),
                                appointmentDate: Yup.object()
                                                    .required('You must choose date of the meeting')
                            })}

                            onSubmit = {async (values,{setSubmitting,setStatus,resetForm}) => {
                                console.log(values);
                                if(values){
                                    try{                               
                                        await updateAppointmentRequest(values)
                                        setSubmitting(false);
                                        resetForm();
                                        handleClose();
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
                        {({ errors, touched,status,isSubmitting,setFieldValue, handleChange}) => (
                        <Form style={{width:'100%'}}>                         
                            <PsychologistDisplay>
                                <PsychologistData>
                                    <Avatar src={psychologist?.profileImageUrl}/>
                                    <p>{psychologist?.firstName} {psychologist?.lastName}</p>
                                </PsychologistData>
                            </PsychologistDisplay>
                            <DatePickerContainer>                               
                                <LocalizationProvider dateAdapter={AdapterDateMoment}>
                                    <DatePicker
                                        minDate={moment()}
                                        label="Appointment date"
                                        value={selectedDate}
                                        onChange={(newValue) => {
                                            handleDateChange(newValue)

                                        }}
                                        renderInput={(props) => <TextField {...props} />}                                       
                                    />
                                </LocalizationProvider>
                            </DatePickerContainer>   
                            <Grid container rowSpacing={1} columnSpacing={{ xs: 0.5}} columns={{ xs: 6}} style={{marginTop:'30px'}}>
                                {availableDates?.map(((date,index) => 
                                    <Grid item xs={1} key={index}>
                                        <DateItem active={chosenDateIndex === index}  name="appointmentDate" onClick={() => {
                                            setFieldValue('appointmentDate', moment(date))
                                            setChosenDateIndex(index);                                           
                                            }}>
                                            {moment(date).format('HH:mm')}
                                        </DateItem>
                                    </Grid>
                                ))}
                            </Grid>                       
                            <PriceContainer>
                                    <AppointmentButton type="submit">{isSubmitting ? 'Changing date ...' : 'Change date'}</AppointmentButton>
                                    {status && status.errorMessage ? (
                                            <ErrorDisplay>{status.errorMessage}</ErrorDisplay>
                                        ) : null}
                                    {errors.appointmentDate && touched.appointmentDate ? (
                                        <ErrorDisplay>{errors.appointmentDate}</ErrorDisplay>
                                    ) : null}
                            </PriceContainer>                           
                        </Form>
                        )}
                    </Formik>
                </AppointmentFormContainer>
            </DialogContainer>
        </Dialog>
    )
}

export default ChangeAppointmentDateForm
