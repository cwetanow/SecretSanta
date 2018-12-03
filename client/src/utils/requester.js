import axios from 'axios';
import authService from '../services/authService';

const getConfig = () => {
  const token = authService.getToken();

  let config = {
    headers: { 'Authorization': `Bearer ${token}` }
  };

  return config;
}

const API_URL = '/api';

const requester = {
  apiUrl: API_URL,
  getAuthorized: (url) => {
    return axios.get(`${requester.apiUrl}${url}`, getConfig());
  },
  get: (url) => {
    return axios.get(`${requester.apiUrl}${url}`);
  },
  postAuthorized: (url, body) => {
    return axios.post(`${requester.apiUrl}${url}`, body, getConfig());
  },
  post: (url, body) => {
    return axios.post(`${requester.apiUrl}${url}`, body);
  },
  putAuthorized: (url, body) => {
    return axios.put(`${requester.apiUrl}${url}`, body, getConfig());
  },
  deleteAuthorized: (url, body) => {
    return axios.delete(`${requester.apiUrl}${url}`, { ...getConfig(), data: body });
  },
  createNew: () => {
    const copy = {};
    Object.keys(requester)
      .forEach(key => copy[key] = requester[key]);

    return copy;
  }
}

export default requester;