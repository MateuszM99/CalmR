import React from 'react'
import Navbar from '../../../components/common/Navbar/Navbar'
import Sidebar from '../../../components/user/Sidebar/Sidebar'
import UserMeetings from '../../../components/user/UserMeetings/UserMeetings'
import {PageContainer} from '../../../../application/common/PageContainer/PageContainer'

function UserMeetingsPage() {
    return (
        <PageContainer>
            <Navbar></Navbar>
            <Sidebar></Sidebar>
            <UserMeetings></UserMeetings>
        </PageContainer>
    )
}

export default UserMeetingsPage
