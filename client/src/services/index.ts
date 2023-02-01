import axios from "axios";
import { AxiosError, AxiosRequestConfig, AxiosResponse } from "axios";
import { getCookie } from "utils/cookieHelper";

const http = axios.create({
  baseURL: "https://localhost:5000/api/",
});

export const setConfig = (config: AxiosRequestConfig): AxiosRequestConfig => {
  const token = sessionStorage.getItem("candidateToken") || getCookie("token");

  if (token) {
    config.headers["Authorization"] = `Access-Token ${token}`;
  }
  return config;
};

export const catchError = (error: AxiosError): Promise<string> =>
  Promise.reject(error);

export const getResponseWithHeader = (response: AxiosResponse): AxiosResponse =>
  response;

export const getResponseData = (response: AxiosResponse): AxiosResponse =>
  response.data;

http.interceptors.request.use(setConfig, catchError);

http.interceptors.response.use(getResponseData, catchError);

export default http;
