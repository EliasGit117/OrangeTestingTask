import { FC, useEffect } from "preact/compat";
import MovieService, { MovieFilterType } from "../services/movies-service";
import { signal } from "@preact/signals";
import { useNavigate } from "react-router-dom";
import useAuthStore from "../stores/auth-store";
import { Card, Pagination } from "flowbite-react";
import React from "react";

const popularMovies = signal([]);
const topRatedMovies = signal([]);

const popularPagination = signal<{ page: number, total: number }>({
  page: 1,
  total: 1,
});

const topRatedPagination = signal<{ page: number, total: number }>({
  page: 1,
  total: 1,
});

const getPopular = (page: number) => {
  MovieService.getFilteredMovies(page, 'popular')
    .then(r => {
      popularMovies.value = r.data.results;
      popularPagination.value = {page: page, total: r.data.total_pages};
    })
    .catch(e => console.error(e));
}

const getTopRated = (page: number) => {
  MovieService.getFilteredMovies(page, "top_rated")
    .then(r => {
      topRatedMovies.value = r.data.results;
      topRatedPagination.value = {page: page, total: r.data.total_pages};
    })
    .catch(e => console.error(e));
}

const getMovies = () => {
  getPopular(1);
  getTopRated(1);
};


const HomePage: FC = () => {
  const navigate = useNavigate();
  const name = useAuthStore((state) => state.name);

  const onPopularPageChange = (n: number) => {
    getPopular(n);
  };

  const onTopRatedPageChange = (n: number) => {
    getTopRated(n);
  };

  useEffect(() => {
    getMovies();
  }, []);

  return (
    <div className="p-8">

      <h1 className="text-3xl font-light dark:text-white">Most popular</h1>
      <div
        className="flex overflow-x-auto pb-10 hide-scroll-bar mt-4"
      >
        {
          popularMovies?.value.map((m: any) => <>

            <div className="inline-block px-3">
              <div
                onClick={() => navigate(`/movies/${m.id}`)}
                className="cursor-pointer bg-black h-64 w-44 max-w-xs overflow-hidden rounded-lg shadow-md hover:shadow-xl transition-shadow duration-300 ease-in-out"
              >
                <img src={`https://image.tmdb.org/t/p/w300/${m.poster_path}`}/>
              </div>
            </div>
          </>)
        }
      </div>
      <h3 className="flex justify-between items-center mt-2">
        <div className="text-gray-500 dark:text-gray-400">
          Selected page: {popularPagination.value.page}, total pages: {popularPagination.value.total}
        </div>
        <Pagination
          layout="navigation"
          currentPage={popularPagination.value.page}
          totalPages={popularPagination.value.total}
          onPageChange={(number) => onPopularPageChange(number)}
        />
      </h3>


      <h1 className="text-3xl font-light mt-4 dark:text-white">
        Top rated
      </h1>
      <div
        className="flex overflow-x-auto pb-10 hide-scroll-bar mt-4"
      >
        {
          topRatedMovies?.value.map((m: any) => <>
            <div className="inline-block px-3">
              <div
                onClick={() => navigate(`/movies/${m.id}`)}
                className="cursor-pointer bg-black h-64 w-44 max-w-xs overflow-hidden rounded-lg shadow-md hover:shadow-xl transition-shadow duration-300 ease-in-out"
              >
                <img src={`https://image.tmdb.org/t/p/w300/${m.poster_path}`}/>
              </div>
            </div>
          </>)
        }
      </div>
      <div className="flex justify-between items-center mt-2">
        <div className="text-gray-500 dark:text-gray-400">
          Selected page: {topRatedPagination.value.page}, total pages: {topRatedPagination.value.total}
        </div>
        <Pagination
          layout="navigation"
          currentPage={topRatedPagination.value.page}
          totalPages={topRatedPagination.value.total}
          onPageChange={(number) => onTopRatedPageChange(number)}
        />
      </div>
    </div>
  );
};

export default HomePage;
