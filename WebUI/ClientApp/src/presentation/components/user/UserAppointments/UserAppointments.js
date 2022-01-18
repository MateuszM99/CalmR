import React, { useEffect,  useState } from 'react'
import { cancelAppointmentRequest, getAppointmentsRequest } from '../../../../infrastructure/services/api/appointments/AppointmentsRequests';
import ChangeAppointmentDateForm from '../ChangeAppointmentDateForm/ChangeAppointmentDateForm';
import DatePicker from '@mui/lab/DatePicker';
import AdapterDateMoment from '@mui/lab/AdapterMoment';
import LocalizationProvider from '@mui/lab/LocalizationProvider';
import SearchIcon from '@mui/icons-material/Search';
import TextField from '@mui/material/TextField';
import { MenuItem, Select, Avatar, FormControl, InputLabel, Pagination } from '@mui/material';
import { Container, InputContainer, ListContainer, ListItem, ListItemContainer, ListItemRow, SearchBar, Header, VerticalLine, FiltersContainer, DatePickerContainer } from './style';
import { FormButton } from '../../../../application/common/FormButton/FormButton';
import AccessTimeIcon from '@mui/icons-material/AccessTime';
import PhoneIcon from '@mui/icons-material/Phone';
import EventIcon from '@mui/icons-material/Event';
import PlaceIcon from '@mui/icons-material/Place';
import AdjustIcon from '@mui/icons-material/Adjust';
import * as moment from 'moment';
import useQueryString from '../../../../application/hooks/useQueryString';
import ConfirmationDialog from '../../../../application/common/ConfirmationDialog/ConfirmationDialog';
import { mapAppointmentStatusToString } from '../../../../application/helpers/mapAppointmentStatusToString';
import ClearIcon from "@material-ui/icons/Clear";
import { makeStyles } from '@material-ui/core';

const useStyles = makeStyles(() => ({
    ul: {
      "& .MuiPaginationItem-root": {
        color : 'white'
      }
    }
  }));


function UserAppointments() {
    const classes = useStyles(); 
    const [totalPages, setTotalPages] = useState(0);
    const [currentPageIndex, onSetCurrentPageIndex] = useQueryString("pageIndex", 1);
    const [textSearch, onSetTextSearch] = useQueryString("s", "");
    const [appointmentStatus, setAppointmentStatus] = useQueryString("status", "");
    const [appointmentSort, setAppointmentSort] = useQueryString("sort", "");
    const [appointmentDate, setAppointmentDate] = useQueryString("date",null);
    const [appointments, setAppointments] = useState(null);
    const [open, setOpen] = useState(false);
    const [selectedPsychologist, setSelectedPsychologist] = useState(null);
    const [selectedAppointmentId, setSelectedAppointmentId] = useState(null);
    const [openConfirmationDialog, setOpenConfirmationDialog] = useState(false);
    const [user, setUser] = useState(null);
    
    const handleClickOpen = (psychologist) => {   
        setSelectedPsychologist(psychologist)  
        setOpen(true);
    };

    const handleClose = () => {
        setOpen(false);
    };

    const handleConfirmationDialogClose = () => {
        setOpenConfirmationDialog(false);
    };

    const getAppointments = async () => {
        try{
            let response = await getAppointmentsRequest(currentPageIndex, textSearch, appointmentStatus, appointmentSort, appointmentDate);
            setAppointments(response.data.data);
            console.log(response.data.data);
            setTotalPages(response.data.totalPages);
        } catch(err){
            console.log(err);
        }
    }

    const handleTextSearchChange = (e) => {
        onSetTextSearch(e.target.value);
    }

    const handlePageChange = (event,pageNumber) => {
        onSetCurrentPageIndex(pageNumber);
    }

    const handleUserGet = () => {
        if(localStorage.getItem("userData") != null){
            if(!user){
                const userData = JSON.parse(localStorage.getItem('userData'));
                setUser(userData);
            }
        }
    }

    const handleDateClr = () => {
        setAppointmentDate(null);
    }

    useEffect(() => {
        getAppointments();
        handleUserGet();
    },[])


    useEffect(() => {
            getAppointments();
    },[currentPageIndex, appointmentStatus, appointmentSort, appointmentDate])

    return (
        <Container>
            <SearchBar>
                    <input className="search-box" type="text" placeholder="Type to search ..." onChange={handleTextSearchChange}/>
                    <div className="search-icon" onClick={getAppointments}>
                        <SearchIcon fontSize="medium"/>
                    </div>
            </SearchBar>
            <FiltersContainer>
                <DatePickerContainer>
                    <LocalizationProvider dateAdapter={AdapterDateMoment}>               
                            <DatePicker
                                label="Appointment Date"
                                value={appointmentDate}
                                onChange={(newValue) => {
                                    setAppointmentDate(newValue.format('YYYY-MM-DD'));
                                }}
                                renderInput={(props) => <TextField {...props} />}                                        
                            />
                    </LocalizationProvider>
                    <ClearIcon onClick={(e) => handleDateClr()}/>
                </DatePickerContainer>
                <FormControl sx={{ m: 1, minWidth: 250}}>
                    <InputLabel id="status-select" style={{color: 'white'}}>Status</InputLabel>
                    <Select
                        labelId="status-select"
                        id="status"
                        label="Status"
                        onChange={(event) => setAppointmentStatus(event.target.value)}
                    >
                        <MenuItem value="">All</MenuItem>
                        <MenuItem value="awaiting">Awaiting for confirmation</MenuItem>
                        <MenuItem value="confirmed">Confirmed</MenuItem>
                        <MenuItem value="rejected">Rejected</MenuItem>
                        <MenuItem value="cancelled">Cancelled</MenuItem>
                        <MenuItem value="ended">Ended</MenuItem>
                    </Select>
                </FormControl>
                <FormControl sx={{ m: 1, minWidth: 250}}>
                    <InputLabel id="sort-select" style={{color: 'white'}}>Sort</InputLabel>
                    <Select
                        labelId="sort-select"
                        id="sort"
                        label="Sort"
                        onChange={(event) => setAppointmentSort(event.target.value)}
                    >
                        <MenuItem value="">None</MenuItem>
                        <MenuItem value="date">Order by date</MenuItem>
                    </Select>
                </FormControl>
            </FiltersContainer>
            <ListContainer>
                {appointments?.map(appointment =>
                    <ListItem key={appointment.id}>
                        <ListItemContainer>
                            <ListItemRow>
                                <Header>Psychologist</Header>
                                <InputContainer>
                                    <Avatar src={appointment.psychologist?.profileImageUrl}/>
                                    <p>{appointment.psychologist?.firstName} {appointment.psychologist?.lastName}</p>
                                </InputContainer>
                                <InputContainer>
                                    <PhoneIcon/>
                                    <p>{appointment.psychologist?.phoneNumber}</p>
                                </InputContainer>                               
                                <InputContainer>
                                    <PlaceIcon/>
                                    <p>{appointment.psychologist?.address?.country} &bull; {appointment.psychologist?.address?.city} &bull; {appointment.psychologist?.address?.addressLine1}</p>
                                </InputContainer>
                                {appointment.psychologist?.address?.addressLine2 ?
                                    <InputContainer>
                                        <PlaceIcon/>
                                        <p>{appointment.psychologist?.address?.country} &bull; {appointment.psychologist?.address?.city} &bull; {appointment.psychologist?.address?.addressLine2}</p>
                                    </InputContainer> : null}
                            </ListItemRow>
                            <VerticalLine></VerticalLine>
                            <ListItemRow>
                                <Header>Appointment</Header>
                                <InputContainer>
                                    <EventIcon/>
                                    <p>{moment(appointment.startDate).format('YYYY-MM-DD')}</p>
                                </InputContainer>                               
                                <InputContainer>
                                    <AccessTimeIcon/>
                                    <p>{moment(appointment.startDate).format('HH:mm')}</p>
                                </InputContainer>                               
                                <InputContainer>
                                    <AdjustIcon/>
                                    <p>{mapAppointmentStatusToString(appointment.status)}</p>
                                </InputContainer>
                            </ListItemRow>
                        </ListItemContainer>
                        {appointment.status === 1 ?
                        <span className="actions">
                            <FormButton width="200px" height="42px" fontSize="12px" onClick={() => {
                                setSelectedAppointmentId(appointment.id)
                                setOpenConfirmationDialog(true)
                            }}>Cancel appointment</FormButton>
                            <FormButton width="200px" height="42px" fontSize="11px" onClick={() => {
                                setSelectedAppointmentId(appointment.id)
                                handleClickOpen(appointment.psychologist)
                                }}>Change appointment date</FormButton>
                        </span>
                        : null}
                        {appointment.status !== 1 ? 
                        <span className="actions"></span> : null}
                    </ListItem>
                )}               
            </ListContainer>
            <ChangeAppointmentDateForm
                psychologist = {selectedPsychologist}
                appointmentId = {selectedAppointmentId}
                open={open}
                onClose={handleClose}
            />
            <ConfirmationDialog
                open = {openConfirmationDialog}
                onClose = {handleConfirmationDialogClose}
                onAgreeAction = {() => cancelAppointmentRequest({appointmentId: selectedAppointmentId})}
            />
            <Pagination count={totalPages} classes={{ul: classes.ul}} defaultPage={1} page={currentPageIndex} onChange={handlePageChange} style={{margin:"30px 0px"}}/>
        </Container>
    )
}

export default UserAppointments
