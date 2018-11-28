import requester from '../utils/requester';

const API_URL = '/api';

class UserService {
  static getByUsername(username) {
    return requester.getAuthorized(`/users/${username}`)
  }

  static getUsers(pattern = null, sortAscending = true, offset = 0, limit = 10) {
    let url = `/users`;

    return requester.getAuthorized(url);
  }
}

export default UserService;