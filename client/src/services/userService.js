import requester from '../utils/requester';

const API_URL = '/api';

class UserService {
  static getByUsername(username) {
    return requester.getAuthorized(`/users/${username}`)
  }
}

export default UserService;