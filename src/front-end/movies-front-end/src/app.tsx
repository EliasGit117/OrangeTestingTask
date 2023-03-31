import MovieService from "./services/movies-service";
import React, { useEffect } from "preact/compat";
import { signal } from "@preact/signals";
import { createBrowserRouter, Navigate, RouterProvider } from "react-router-dom";
import Root from "./components/root";
import AuthPage from "./pages/auth-page";
import HomePage from "./pages/home-page";
import FavPage from "./pages/fav-page";
import MovieDetails from "./pages/movie-details";
import 'react-toastify/dist/ReactToastify.css';
import { ToastContainer } from "react-toastify";
import useAuthStore from "./stores/auth-store";
import { shallow } from "zustand/shallow";
import ByGenresPage from "./pages/by-genres-page";

const movies = signal<any | null>(null);

export function App() {
  const { token, name } = useAuthStore(
    (s) => ({ token: s.token, name: s.name }),
    shallow
  );

  const router = createBrowserRouter([
    {
      path: "/",
      element: <Root/>,
      children: [
        {
          path: "home",
          element: <HomePage/>,
        },
        {
          path: "fav",
          element: <FavPage/>,
        },
        {
          path: "movies/:id",
          element: <MovieDetails/>,
        },
        {
          path: "auth",
          element: <AuthPage/>,
        },
        {
          path: "by-genre",
          element: <ByGenresPage/>,
        },
        {
          path: '*',
          element: <Navigate to="/home" replace />
        },
        {
          path: '',
          element: <Navigate to="/home" replace />
        }
      ],
    },
  ]);



  return (
    <>
      <ToastContainer />
      <RouterProvider router={router}/>
    </>
  )
}
