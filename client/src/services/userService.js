import requester from '../utils/requester';

const API_URL = '/api';

class UserService {
  static getByUsername(username) {
    return requester.getAuthorized(`/users/${username}`)
  }

  static getUsers(pattern = null, sortAscending = true, offset = 0, limit = 10) {
    let url = `/users?`;

    if (pattern) {
      url = `${url}searchPattern=${pattern}&`
    }

    if (sortAscending) {
      url = `${url}sortAscending=${sortAscending}&`
    }

    if (offset) {
      url = `${url}offset=${offset}&`
    }

    if (limit) {
      url = `${url}limit=${limit}&`
    }

    return requester.getAuthorized(url);
  }
}

export default UserService;