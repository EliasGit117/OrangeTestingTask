import { signal, useSignal } from "@preact/signals";
import MovieService from "../services/movies-service";
import React, { FC, useEffect } from "preact/compat";
import { Badge } from "flowbite-react";
import { useNavigate } from "react-router-dom";

const genres = signal<{ id: number, name: string }[]>([]);

const getGenres = () => {
  MovieService.getGenres()
    .then(r => {
      genres.value = r.data.genres.sort(() => 0.5 - Math.random()).slice(0, 5);
      ;
    })
    .catch(e => console.error(e));
}

const getMoviesByGenre = (genreId: number, onSuccess: (result: any) => void) => {
  MovieService.discover(genreId)
    .then(r => onSuccess(r.data))
    .catch(e => console.error(e));
};

const ByGenresPage = () => {

  useEffect(() => {
    getGenres();
  }, []);

  return (
    <div className="flex flex-col">
      {
        genres.value &&
        <div className="gap-2 m-4 pb-4 overflow-x-auto">
          {
            genres.value.map((g: { id: number, name: string }) => <MovieList genreId={g.id} name={g.name}/>)
          }
        </div>
      }
    </div>
  );
};

const MovieList: FC<{ genreId: number, name: string }> = (p) => {
  const movies = useSignal<any[]>([]);
  const navigate = useNavigate();

  useEffect(() => {
    getMoviesByGenre(p.genreId, (r) => movies.value = r.results);
  }, []);

  return (
    <>
      <h5 className="text-3xl font-light ml-4 my-6">{p.name}</h5>
      <div className="flex flex-row overflow-x-auto pb-4">
        {
          movies.value.map(m =>
            <div className="inline-block px-3">
              <div
                onClick={() => navigate(`/movies/${m.id}`)}
                className="cursor-pointer bg-black h-64 w-44 max-w-xs overflow-hidden rounded-lg shadow-md hover:shadow-xl transition-shadow duration-300 ease-in-out"
              >
                <img src={`https://image.tmdb.org/t/p/w300/${m.poster_path}`}/>
              </div>
            </div>
          )
        }
      </div>
    </>
  );
};

export default ByGenresPage;
