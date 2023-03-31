import React, { FC, useEffect } from "preact/compat";
import { signal } from "@preact/signals";
import MovieService from "../services/movies-service";
import { Button, Card } from "flowbite-react";
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";

const movies = signal<any[]>([]);


const getFavorites = () => {
  MovieService.getFavMovies()
    .then(r => movies.value = r.data)
    .catch(e => console.error(e));
}

const remove = (isFav: boolean, movieId: number, index: number) => {
  MovieService.SetFav(movieId, isFav)
    .then(res => {
      movies.value.splice(index, 1);
      movies.value = [...movies.value];
    })
    .catch(e =>
      toast(JSON.stringify(e.response.data.errors || e.response.data, null, 2))
    );
};


const FavPage: FC = () => {
  const navigate = useNavigate();
  useEffect(() => {
    getFavorites();
  }, []);

  return (
    <div className="container mx-auto mt-8">
      <div className="grid grid-cols-12">
        {
          movies.value.map((m, i) => <>
            <div
              // horizontal={true}
              className="mt-4 mx-2 bg-neutral shadow-lg dark:bg-neutral-900 rounded-xl p-8 col-span-12 sm:col-span-6 xl:col-span-4"
            >
              <div className="grid grid-cols-12 gap-4 overflow-hidden justify-self-start h-full">

                <div className="col-span-12 lg:col-span-6 rounded-xl">
                  <img src={`https://image.tmdb.org/t/p/w500/${m.poster_path}`}/>
                </div>

                <div className="col-span-12 lg:col-span-6 flex flex-col gap-4">
                  <h5 className="text-2xl font-bold tracking-tight text-gray-900 dark:text-white">
                    {m.title}
                  </h5>

                  <h5 className="text-xl tracking-tight text-gray-900 dark:text-white">
                    Popularity: {m.popularity}
                  </h5>
                  <h5 className="text-xl tracking-tight text-gray-900 dark:text-white">
                    Vote average: {m.vote_average}
                  </h5>
                  <div className="flex flex-col gap-4 flex-1 justify-end">
                    <Button
                      onClick={() => navigate(`/movies/${m.id}`)}
                      type="submit"
                      className="flex"
                      size="sm"
                      color="light"
                    >
                      Details
                    </Button>
                    <Button size="sm" color="failure" onClick={() => remove(false, m.id, i)}>
                      Remove
                    </Button>
                  </div>

                </div>
              </div>

            </div>
          </>)
        }
      </div>
    </div>
  );
};

export default FavPage;
