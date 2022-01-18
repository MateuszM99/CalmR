import React from 'react'
import Navbar from '../../../components/common/Navbar/Navbar'
import Sidebar from '../../../components/user/Sidebar/Sidebar'
import {PageContainer} from '../../../../application/common/PageContainer/PageContainer'

function UserHomePage() {
    return (
        <PageContainer>
            <Navbar></Navbar>
            <Sidebar></Sidebar>
        </PageContainer>
    )
}

export default UserHomePage
