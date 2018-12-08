import axios from 'axios';
import authService from '../services/authService';

export default (callback) => {
  axios.interceptors.response.use(null, (err) => {
    if (err.response.status === 401) {
      callback(err);
    }

    return Promise.reject(err);
  });
}