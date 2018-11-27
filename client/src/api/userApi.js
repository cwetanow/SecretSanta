import requester from './requester';

const API_URL = '/api';

class UserApi {
  static getByUsername(username) {
    return requester.getAuthorized(`/users/${username}`)
  }
}

export default UserApi;