import React from 'react'
import Navbar from '../../../components/Navbar/Navbar'
import Sidebar from '../../../components/user/Sidebar/Sidebar'
import UserNotes from '../../../components/user/UserNotes/UserNotes'

function UserNotesPage() {
    return (
        <div>
            <Navbar></Navbar>
            <Sidebar></Sidebar>
            <UserNotes></UserNotes>
        </div>
    )
}

export default UserNotesPage
