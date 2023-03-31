import axios from "axios";

export type MovieFilterType = 'popular' | 'top_rated';

export default class MovieService {

  public static api = axios.create({
    baseURL: import.meta.env.VITE_API_URL,
  });


  static login(name: string, password: string) {
    return this.api.post(`auth/login`, { name, password });
  }

  static createAcc(name: string, password: string) {
    return this.api.post(`auth/register`, { name, password });
  };

  static getMovie(id: number) {
    return this.api.get<any>(`movies/${id}`);
  };

  static getFilteredMovies(pageNumber: number, type: MovieFilterType) {
    return this.api.get<any>(`movies/filter?SearchType=${type}&Page=${pageNumber}`);
  };

  static SetFav(movieId: number, isFav: boolean) {
   return this.api.put<any>(`movies?movieId=${movieId}&isFav=${isFav}`);
  }

  static getGenres() {
    return this.api.get<any>(`movies/genres`);
  }

  static getFavMovies() {
    return this.api.get<any[]>(`movies/fav`);
  }

  static discover(genreId: number) {
    return this.api.get<any[]>(`movies/discover?genreId=${genreId}`);
  }
}

