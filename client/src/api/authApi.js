import axios from 'axios';

const API_URL = '/api';

class AuthApi {
  static register(user) {
    return axios.post(API_URL + '/users', user);
  }

  static login(user) {
    return axios.post(API_URL + '/login', user);
  }
}

export default AuthApi;