import { FC, useEffect } from "preact/compat";
import { useParams } from "react-router-dom";
import { signal } from "@preact/signals";
import MovieService from "../services/movies-service";
import useAuthStore from "../stores/auth-store";
import { toast } from "react-toastify";
import { Spinner } from "flowbite-react";
import React from "react";

const movie = signal<any | undefined>(undefined);

const getMovie = (id: number) => {
  MovieService.getMovie(id)
    .then(res => movie.value = res.data)
    .catch(e => console.error(e));
};

const setFav = (isFav: boolean, movieId: number) => {
  MovieService.SetFav(movieId, isFav)
    .then(res => {
      const movieCopy = { ...movie.value };
      movieCopy.isFav = isFav;
      movie.value = movieCopy;
    })
    .catch(e =>
      toast(JSON.stringify(e.response.data.errors || e.response.data, null, 2))
    );
};

const MovieDetails: FC = () => {
  const {id} = useParams();
  const {token} = useAuthStore((state) => ({token: state.token}));

  useEffect(() => {
    movie.value = undefined;
    getMovie(Number(id));
  }, []);

  return (
    <div className="container mx-auto mt-4 dark:text-white">

      {
        movie.value ?
          <div className="grid grid-cols-12 gap-4 m-4">
            <div className="col-span-12 sm:col-span-4 flex flex-col justify-center">
              <img className="rounded-xl w-56 sm:w-full"
                   src={`https://image.tmdb.org/t/p/w500/${movie.value?.poster_path}`}
              />
              {
                token && (
                  !movie.value.isFav ?
                    <button
                      onClick={() => setFav(true, movie.value.id)}
                      className="border rounded-3xl border-orange-500 text-orange-500 px-6 py-2 mt-2"
                    >
                      Add to favorite
                    </button>
                    :
                    <button
                      onClick={() => setFav(false, movie.value.id)}
                      className="border rounded-3xl bg-orange-500 text-white px-6 py-2 mt-2"
                    >
                      Remove from favorite
                    </button>
                )
              }
            </div>
            <div className="flex flex-col col-span-12 sm:col-span-8">
              <h5 className="text-2xl mt-2">{movie.value.title}</h5>
              <h5 className="text-xl mt-2">Popularity: {movie.value.popularity}</h5>
              <h5 className="text-xl mt-2">Vote average: {movie.value.vote_average}</h5>
              <a href={movie.value.homePage} className="mt-2">{movie.value.homePage}</a>
              <h5 className="text-md font-light mt-2">{movie.value.overview}</h5>
            </div>
          </div>
          :
          <div className="flex justify-center flex-row mt-8">
            <Spinner size="xl"/>
          </div>
      }
    </div>
  );
};

export default MovieDetails;
