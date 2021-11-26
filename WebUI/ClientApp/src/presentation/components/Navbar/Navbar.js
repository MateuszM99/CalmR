import React,{useState, useEffect} from 'react'
import { Link, useHistory } from 'react-router-dom'
import './style.scss'

function Navbar() {
    const [isSignedIn,setIsSignedIn] = useState(false);
    const history = useHistory();

    const handleLoggedIn = () => {
        if(localStorage.getItem("userData") != null){
            setIsSignedIn(true);
        }
    }

    const handleLogout = () => {
        localStorage.setItem("userData",'')
        localStorage.clear();
        setIsSignedIn(false);
        history.push('/home')
    }

    useEffect(() => {    
        handleLoggedIn();
    });

    return (
        <nav>
            <span>
                <Link style={{display : isSignedIn ? 'none' : 'block'}} to="/signin">Sign in</Link>
                <Link style={{display : isSignedIn ? 'none' : 'block'}} to="/signup">Sign up</Link>
                <Link style={{display : isSignedIn ? 'block' : 'none'}} to="/signin">Username</Link>
                <a onClick={handleLogout} style={{display : isSignedIn ? 'block' : 'none'}}>Log out</a>
            </span>
        </nav>
    )
}
export default Navbar