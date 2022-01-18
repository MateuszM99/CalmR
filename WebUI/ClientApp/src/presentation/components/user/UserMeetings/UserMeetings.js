import { HubConnectionBuilder } from '@microsoft/signalr'
import React, { useEffect, useRef, useState } from 'react'
import { FormButton } from '../../../../application/common/FormButton/FormButton'
import { Container, Header, InputContainer, LinkButton, ListItem, ListItemContainer, ListItemRow, MyVideo, NoAppointmentsContainer, VerticalLine } from './style'
import { Box, VideoContainer, StyledVideo } from './style'
import Peer from 'simple-peer'
import { getUpcomingAppointment } from '../../../../infrastructure/services/api/appointments/AppointmentsRequests'
import { Avatar } from '@mui/material'
import AccessTimeIcon from '@mui/icons-material/AccessTime';
import PhoneIcon from '@mui/icons-material/Phone';
import EventIcon from '@mui/icons-material/Event';
import PlaceIcon from '@mui/icons-material/Place';
import AdjustIcon from '@mui/icons-material/Adjust';
import * as moment from 'moment';
import { mapAppointmentStatusToString } from '../../../../application/helpers/mapAppointmentStatusToString'



const Video = (props) => {
    const ref = useRef();

    useEffect(() => {
		console.log('is here in component')
        props.peer.on("stream", stream => {
			console.log('streaming');
            ref.current.srcObject = stream;
        })
    }, []);

    return (
		<StyledVideo playsInline autoPlay ref={ref} />
    );
}

function UserMeetings() {
    const [connection, setConnection] = useState();
    const [connectionStarted, setConnectionStarted] = useState();

	const [appointment, setAppointment] = useState(null);
	const [user, setUser] = useState(null);
	const [joinedMeeting, setJoinedMeeting] = useState(false);
	const [peer, setPeer] = useState(null);
	const peerRef = useRef(null)
    const myVideo = useRef()
	const connectionRef= useRef()

	const handleAppointmentGet = async () => {
		try {
            let response = await getUpcomingAppointment();
            setAppointment(response.data);
        } catch(err) {
            
        }
	}

	const handleUserGet = () => {
		if(localStorage.getItem("userData") != null){
            if(!user){
                const userData = JSON.parse(localStorage.getItem('userData'));
                setUser(userData);
            }
        }
	}

    const handleJoinMeeting = () => {
		setJoinedMeeting(true);

		navigator.mediaDevices.getUserMedia({ video: true, audio: true }).then((stream) => {
			myVideo.current.srcObject = stream
			connection.on('JoinedMeeting', data => {							
				handleJoinedMeeting(data, stream);
			})
			connection.on('UserJoined', data => {							
				handleUserJoined(data, stream)
			});
			connection.on('ReceiveReturnedSignal', data => {									
				handleReceivedSignal(data);
			})
			connection.invoke('JoinMeeting', {appointmentId: 100});
		})
	}

	const handleJoinedMeeting = (data, stream) => {
		if(data.isUserConnected){
			console.log('JoinedMeeting')
			console.log(stream);	
			const peer = new Peer({
				initiator: true,
				trickle: false,
				stream,
			});

			peer.on("signal", signal => {
				connection.invoke("SendSignal", {appointmentId: 100, peerSignal: signal})
			})

			peerRef.current = peer;
			setPeer(peer);
		}
	}

	const handleUserJoined = (incomingSignal, stream) => {
		console.log('UserJoined')
		const peer = new Peer({
            initiator: false,
            trickle: false,
            stream,
        })

        peer.on("signal", signal => {
            connection.invoke('ReturnSignal', {appointmentId: 100, peerSignal: signal})
        })

        peer.signal(incomingSignal);

		peerRef.current = peer;
		setPeer(peer);
	}

	const handleReceivedSignal = (signal) => {
		console.log('ReceivedSignal')
		peerRef.current.signal(signal);
	}

	const leaveMeeting = () => {
		setJoinedMeeting(false);	
		connectionRef.current.invoke('LeaveMeeting', {appointmentId: 100});
		if(peerRef.current != null){			
			peerRef.current.destroy()
		}
	}

	const onUserLeaveMeeting = () => {
		setPeer(null);
		if(peerRef.current != null){			
			peerRef.current.destroy()
		}
	}

	useEffect(() => {
		connectionRef.current = connection;
	  }, [connection]);
	

    useEffect(() => {
        const newConnection = new HubConnectionBuilder()
            .withUrl('https://localhost:5001/hubs/videoChat')
            .build();

        setConnection(newConnection);

		//handleAppointmentGet();
		handleUserGet();

		return () => {
			leaveMeeting();
		  };
    }, [])

    useEffect(() => {
        if (connection) {
            connection.start()
                .then(result => {
                    setConnectionStarted(true);
					connection.on('UserDisconnected', data => {
						onUserLeaveMeeting();
					});
                })
                .catch(e => console.log('Connection failed: ', e));
        }
    }, [connection]);


	if(!appointment){
		return (
			<NoAppointmentsContainer>			
				<span>You do not have any upcoming appointments.</span>
				<span>Go ahead and make one.</span>
				<LinkButton to="/app/psychologist">Search for psychologist</LinkButton>
			</NoAppointmentsContainer>
		)
	}

	if(appointment && !joinedMeeting){
		return (
			<ListItem>
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
				{!joinedMeeting && connectionStarted ? <FormButton onClick={handleJoinMeeting} width="250px">Join meeting</FormButton> : null}
				{joinedMeeting ? <StyledVideo playsInline muted ref={myVideo} autoPlay style={{ width: "300px" }} /> : null}
            </ListItem>
		)
	}

	if(appointment && joinedMeeting){
		return (
			<Container>
				<Box>
					{joinedMeeting ? 
					<MyVideo playsInline muted ref={myVideo} autoPlay style={{ width: "300px" }} /> : null}
					{peer ? <Video peer={peer}/> : null}					
				</Box>
				<FormButton onClick={leaveMeeting}>Leave meeting</FormButton>
			</Container>
		)
	}
}

export default UserMeetings
