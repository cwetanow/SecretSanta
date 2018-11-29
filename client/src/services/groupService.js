import requester from '../utils/requester';

const API_URL = '/api';

class GroupService {
  static getJoinedGroups() {
    return requester.getAuthorized(`/groups/user`)
      .then(response => Promise.resolve(response.data.groups));
  }

  static createGroup(group) {
    return requester.postAuthorized(`/groups`, group)
      .then(response => Promise.resolve(response.data))
      .catch(err => err.response.status === 400 && Promise.reject(err.response.data));
  }
}

export default GroupService;