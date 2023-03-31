import React, { FC } from "react";
import './header.css';
import { Link, NavLink } from "react-router-dom";
import useAuthStore from "../../stores/auth-store";
import { Navbar } from "flowbite-react";


const Header: FC = () => {
  const {token, logout} = useAuthStore((state) => ({token: state.token, logout: state.logout}));

  return (
    <header>
        <Navbar
          fluid={true}
          rounded={true}
          className="shadow-lg p-8 h-16 pt-4"
        >
          <div className="ml-4 flex grow justify-between text-xl dark:text-white">
            <div className="space-x-5">
              <NavLink to='/home' className='routing-link uppercase'>Home</NavLink>
              <NavLink to='/by-genre' className='routing-link uppercase'>By genre</NavLink>

              {
                token && <NavLink to='/fav' className='routing-link uppercase'>Favorite</NavLink>
              }
            </div>

            {
              !token ?
                <NavLink to='/auth'>Auth</NavLink>
                :
                <NavLink onClick={logout}>Logout</NavLink>
            }
          </div>


        </Navbar>
    </header>
  );
};

export default Header;
