import React, { useEffect, useState } from "react";
import Home from '../../../../application/assets/home-solid.svg'
import Psychologist from '../../../../application/assets/search-plus-solid.svg'
import Appointments from '../../../../application/assets/calendar-check-solid.svg'
import Chat from '../../../../application/assets/comment-dots-solid.svg'
import Notes from '../../../../application/assets/clipboard-regular.svg'
import styled from "styled-components";
import { NavLink } from "react-router-dom";
import { getRecentConversationIdRequest } from "../../../../infrastructure/services/api/conversations/ConversationsRequests";
import PermPhoneMsgIcon from '@mui/icons-material/PermPhoneMsg';

const Container = styled.div`
  background-color: #1325C8;
  position: fixed;
  top: 15rem;
  width: 3.5rem;
  border-radius: 0 30px 30px 0;
  .active {
    border-right: 4px solid white;
    img {
      filter: invert(100%) sepia(0%) saturate(0%) hue-rotate(93deg)
        brightness(103%) contrast(103%);
    }
  }
`;

const Button = styled.button`
  background-color: #1325C8;
  color: #1325C8;
  border: none;
  width: 2.5rem;
  height: 2.5rem;
  border-radius: 50%;
  margin: 0.5rem 0 0 0.5rem;
  cursor: pointer;
  display: flex;
  justify-content: center;
  align-items: center;
  position: relative;
  &::before,
  &::after {
    content: "";
    background-color: white;
    height: 2px;
    width: 1rem;
    position: absolute;
    transition: all 0.3s ease;
  }
  &::before {
    top: ${(props) => (props.clicked ? "1.5" : "1rem")};
    transform: ${(props) => (props.clicked ? "rotate(135deg)" : "rotate(0)")};
  }
  &::after {
    top: ${(props) => (props.clicked ? "1.2" : "1.5rem")};
    transform: ${(props) => (props.clicked ? "rotate(-135deg)" : "rotate(0)")};
  }
`;

const SidebarContainer = styled.div`
  background-color: #1325C8;
  width: 3.5rem;
  height: 40vh;
  border-radius: 0 30px 30px 0;
  padding: 1rem 0;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: space-between;
  position: relative;
`;

const SlickBar = styled.ul`
  color: white;
  height: 40vh;
  list-style: none;
  display: flex;
  flex-direction: column;
  align-items: center;
  background-color: #1325C8;
  padding: 2rem 0;
  position: absolute;
  top: 2rem;
  left: 0;
  width: ${(props) => (props.clicked ? "12rem" : "3.5rem")};
  transition: all 0.5s ease;
  border-radius: 0 30px 30px 0;
`;

const Item = styled(NavLink)`
  text-decoration: none;
  color: white;
  width: 100%;
  padding: 1rem 0;
  cursor: pointer;
  display: flex;
  padding-left: 1rem;
  &:hover {
    text-decoration: none;
    color: white;
    border-right: 4px solid white;
    img {
      filter: invert(100%) sepia(0%) saturate(0%) hue-rotate(93deg)
        brightness(103%) contrast(103%);
    }
  }
  img {
    width: 1.2rem;
    height: auto;
    filter: invert(92%) sepia(4%) saturate(1033%) hue-rotate(169deg)
      brightness(78%) contrast(85%);
  }
`;

const Text = styled.span`
  width: ${(props) => (props.clicked ? "100%" : "0")};
  overflow: hidden;
  margin-left: ${(props) => (props.clicked ? "1.5rem" : "0")};
  transition: all 0.3s ease;
`;

const Sidebar = () => {
  const [click, setClick] = useState(false);
  const [recentConversationId, setRecentConversationId] = useState(null);
  const handleClick = () => setClick(!click);

  const getRecentConversationId = async () => {
    try {
        let response = await getRecentConversationIdRequest();
        console.log(response.data);
        if(response.data != null){
            setRecentConversationId(response.data)
        }
    } catch(err) {
        console.log(err);
    }
  }


  useEffect(() => {
    getRecentConversationId();
  }, [])


  return (
    <Container>
      <Button clicked={click} onClick={() => handleClick()}>
        Click
      </Button>
      <SidebarContainer>
        <SlickBar clicked={click}>
          <Item
            onClick={() => setClick(false)}
            activeClassName="active"
            to="/app/home"
          >
              <img src={Home} alt="Home" />
            <Text clicked={click}>Home</Text>
          </Item>
          <Item
            onClick={() => setClick(false)}
            activeClassName="active"
            to="/app/psychologist"
          >
              <img src={Psychologist} alt="Find psychologist" />
            <Text clicked={click}>Find psychologist</Text>
          </Item>
          <Item
            onClick={() => setClick(false)}
            activeClassName="active"
            to="/app/appointments"
          >
              <img src={Appointments} alt="Appointments" />
            <Text clicked={click}>Appointments</Text>
          </Item>
          <Item
            onClick={() => setClick(false)}
            activeClassName="active"
            to={`/app/chat/${recentConversationId}`}
          >
              <img src={Chat} alt="Chat" />
            <Text clicked={click}>Chat</Text>
          </Item>
          <Item
            onClick={() => setClick(false)}
            activeClassName="active"
            to="/app/meeting"
          >
            <PermPhoneMsgIcon/>
            <Text clicked={click}>Meeting</Text>
          </Item>
        </SlickBar>
      </SidebarContainer>
    </Container>
  );
};

export default Sidebar;