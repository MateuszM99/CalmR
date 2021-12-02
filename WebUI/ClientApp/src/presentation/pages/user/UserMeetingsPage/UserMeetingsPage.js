import React from 'react'
import Navbar from '../../../components/common/Navbar/Navbar'
import Sidebar from '../../../components/user/Sidebar/Sidebar'
import UserMeetings from '../../../components/user/UserMeetings/UserMeetings'

function UserMeetingsPage() {
    return (
        <div>
            <Navbar></Navbar>
            <Sidebar></Sidebar>
            <UserMeetings></UserMeetings>
        </div>
    )
}

export default UserMeetingsPage
