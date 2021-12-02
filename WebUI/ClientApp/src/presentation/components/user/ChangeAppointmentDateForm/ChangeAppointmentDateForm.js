import React, { useEffect, useRef, useState } from 'react'
import Dialog from '@mui/material/Dialog';
import DialogTitle from '@mui/material/DialogTitle';
import DatePicker from '@mui/lab/DatePicker';
import AdapterDateMoment from '@mui/lab/AdapterMoment';
import LocalizationProvider from '@mui/lab/LocalizationProvider';
import { styled } from '@mui/material/styles';
import Paper from '@mui/material/Paper';
import Grid from '@mui/material/Grid';
import { Field, Form, Formik } from 'formik';
import * as Yup from 'yup'
import PropTypes from 'prop-types';
import { Button, TextField } from '@mui/material';
import { createAppointmentRequest, getAppointmentsAvailableHoursRequest, updateAppointmentRequest } from '../../../../infrastructure/services/api/appointments/AppointmentsRequests';
import * as moment from 'moment';

function ChangeAppointmentDateForm(props) {

    const { onClose, open, appointmentId } = props;
    const formRef = useRef(null);
    const [selectedDate, setSelectedDate] = React.useState(moment());
    const [availableDates, setAvailableDates] = useState(null);

    useEffect(() => {
        //getAvaialableHoursList();
    })

    const handleClose = () => {
        console.log(open);
        onClose();
      };

    const handleDateChange = (appointmentDate) => {        
            setSelectedDate(appointmentDate);
    }

    const getAvaialableHoursList = async () => {
        
        let appointmentDurationTime = formRef?.current.values.appointmentDurationTime;

        setAvailableDates(null);

        if(selectedDate && appointmentDurationTime > 0)
        {
            try{
                let response = await getAppointmentsAvailableHoursRequest(1, selectedDate.format('YYYY-MM-DD'), appointmentDurationTime);
                setAvailableDates(response.data);
            } catch(err){
                console.log(err);
            }    
        }
    }

    const Item = styled(Paper)(({ theme }) => ({
        ...theme.typography.body2,
        padding: theme.spacing(1),
        textAlign: 'center',
        color: theme.palette.text.secondary,
        cursor: 'pointer',
      }));

    return (
        <Dialog onClose={handleClose} open={open}>
            <Button onClick={handleClose}>Close</Button>
            <DialogTitle>{"Make an appointment"}</DialogTitle>
            <Formik
                    innerRef = {formRef}
                    initialValues={{
                        appointmentId : appointmentId,
                        appointmentDurationTime: 1,
                        appointmentDate: '',
                    }}
                    validationSchema = {Yup.object({
                        appointmentId : Yup.number()
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
                    <Form>  
                        <div style={{padding: '20px'}}>
                            <LocalizationProvider dateAdapter={AdapterDateMoment}>
                                <DatePicker
                                    renderInput={(props) => <TextField {...props} />}
                                    label="DateTimePicker"
                                    value={selectedDate}
                                    onChange={(newValue) => {handleDateChange(newValue)
                                                             getAvaialableHoursList()}}
                                />
                            </LocalizationProvider>
                            <div>
                                <label>Choose appointment duration</label>
                                <Field type="number" placeholder="" name="appointmentDurationTime" onChange={(e) => {handleChange(e)
                                                                                                                    getAvaialableHoursList()}}/> 
                                {errors.appointmentDurationTime && touched.appointmentDurationTime ? <div className="signup-validation">{errors.appointmentDurationTime}</div> : null}              
                            </div>
                            <Grid container rowSpacing={1} columnSpacing={{ xs: 0.5}} columns={{ xs: 6}}>
                                {availableDates?.map(((date,index) => 
                                    <Grid item xs={1} key={index}>
                                        <Item name="appointmentDate" onClick={() => setFieldValue('appointmentDate', moment(date).add(1,'hour'))}>{moment(date).format('HH:mm')}</Item>
                                    </Grid>
                                ))}
                            </Grid>
                            <Button type="submit">{isSubmitting ? 'Setting up appointment ...' : 'Set appointment'}</Button>
                            {status && status.errorMessage ? (
                                    <div className="signup-validation">{status.errorMessage}</div>
                                ) : null}
                        </div>
                    </Form>
                    )}
                </Formik>
        </Dialog>
    )
}

export default ChangeAppointmentDateForm
