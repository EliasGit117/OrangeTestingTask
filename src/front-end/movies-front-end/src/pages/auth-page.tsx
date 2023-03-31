import React, { ChangeEvent, FC } from "preact/compat";
import { signal } from "@preact/signals";
import { toast, ToastContainer } from "react-toastify";
import MovieService from "../services/movies-service";
import useAuthStore from "../stores/auth-store";
import { useLocation, useNavigate } from "react-router-dom";

const type = signal<'auth' | 'reg'>('auth');

const isDataValid = signal<boolean>(false);

const data = signal({
  name: '',
  password: ''
});


const onChange = (e: any) => {

  data.value = {...data.value, [e.target.name]: e.target.value};
  if (data.value.name.length > 5 && data.value.password.length > 5)
    isDataValid.value = true;
  else
    isDataValid.value = false;
};

const AuthPage: FC = () => {
  const navigate = useNavigate();
  const login = useAuthStore((state) => state.login);

  const createAccount = () => {
    MovieService.createAcc(data.value.name, data.value.password)
      .then(_ => {
        data.value = {
          name: '',
          password: ''
        };
        toast('Your account has been created!');
        type.value = 'auth';
      })
      .catch(e => {
        toast(JSON.stringify(e.response.data.errors || e.response.data, null, 2));
      })
  }


  const loginInAccount = () => {
    MovieService.login(data.value.name, data.value.password)
      .then(r => {
        data.value = {
          name: '',
          password: ''
        };
        login(r.data.name, r.data.token);
        navigate('/')
      })
      .catch(e => {
        toast(JSON.stringify(e.response.data.errors || e.response.data, null, 2));
      })
  };


  return (
    <div className="container mx-auto mt-4 flex justify-center content-center">
      <div className="flex flex-col items-center w-full">

        <div className="w-72">
          <div className="mt-4 w-full">
            <label className="block text-gray-700 dark:text-white text-sm font-bold mb-2" htmlFor="username">
              Username
            </label>
            <input
              value={data.value.name}
              name='name'
              onChange={onChange}
              className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
              id="username" type="text" placeholder="Username"/>
          </div>

          <div className="mt-4 w-full">
            <label className="block text-gray-700 dark:text-white text-sm font-bold mb-2" htmlFor="password">
              Password
            </label>
            <input
              value={data.value.password}
              name='password'
              onChange={onChange}
              className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
              id="password" type="password" placeholder="Password"/>
          </div>
          {
            type.value === 'reg' ?
              <button
                onClick={createAccount}
                className="mt-6 py-2 w-full bg-orange-500 disabled:bg-zinc-700 disabled:text-gray-400 text-white rounded-2xl"
                disabled={!isDataValid.value}
              >
                Create account
              </button>
              :
              <button
                onClick={loginInAccount}
                className="mt-6 py-2 w-full bg-orange-500 disabled:bg-zinc-700 disabled:text-gray-400 text-white rounded-2xl"
                disabled={!isDataValid.value}
              >
                Login
              </button>
          }
        </div>

        <div className="mt-8 h-1 w-72 border-t border-slate-200 dark:border-slate-600 "/>

        <button
          className="mt-3 text-orange-400 dark:text-white p-2 px-5 rounded-3xl"
          onClick={() => type.value === 'reg' ? type.value = 'auth' : type.value = 'reg'}
        >
          {type.value !== 'reg' ? "Create new account" : 'Log in existing account'}
        </button>

      </div>

    </div>
  );
};


export default AuthPage;
