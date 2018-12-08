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
  getAuthorized: (url) => {
    return axios.get(`${API_URL}${url}`, getConfig());
  },
  get: (url) => {
    return axios.get(`${API_URL}${url}`);
  },
  postAuthorized: (url, body) => {
    return axios.post(`${API_URL}${url}`, body, getConfig());
  },
  post: (url, body) => {
    return axios.post(`${API_URL}${url}`, body);
  },
  putAuthorized: (url, body) => {
    return axios.put(`${API_URL}${url}`, body, getConfig());
  },
  deleteAuthorized: (url, body) => {
    return axios.delete(`${API_URL}${url}`, { ...getConfig(), data: body });
  }
}

export default requester;