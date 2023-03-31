import { create } from 'zustand'
import MovieService from "../services/movies-service";
import config from "tailwindcss/defaultConfig";

interface AuthStoreState {
  name: string | undefined,
  token: string | undefined,
  logout: () => void,
  login: (name: string, token: string) => void
}

const useAuthStore = create<AuthStoreState>((set) => ({
  token: localStorage.getItem('token') as string,
  name: localStorage.getItem('name') as string,
  login: (name, token) => set(() => {
    localStorage.setItem('token', token);
    localStorage.setItem('name', name);

    return { name, token };
  }),
  logout: () => set(() => {
    console.warn('TO')
    localStorage.removeItem('token');
    localStorage.removeItem('name');
    return { token: undefined, name: undefined };
  }),
}));

const injectToken = MovieService.api.interceptors.request.use(
  async (config) => {
    const token = useAuthStore.getState().token;
    config.headers.set("Authorization", token);
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

export default useAuthStore;
