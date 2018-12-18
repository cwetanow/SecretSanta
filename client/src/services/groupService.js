import requester from '../utils/requester';

const API_URL = '/api';

class GroupService {
  static getGroups() {
    return requester.getAuthorized(`/groups/user`)
      .then(response => Promise.resolve(response.data.groups));
  }

  static createGroup(group) {
    return requester.postAuthorized(`/groups`, group)
      .then(response => Promise.resolve(response.data))
      .catch(err => err.response.status === 400 && Promise.reject(err.response.data));
  }

  static getGroupUsers(groupName) {
    return requester.getAuthorized(`/groups/${groupName}/users`)
      .then(response => Promise.resolve(response.data.users));
  }

  static checkGroupOwner(groupName) {
    return requester.getAuthorized(`/groups/${groupName}/checkOwner`)
      .then(response => Promise.resolve(response.data.isUserOwner));
  }

  static closeGroup(groupName) {
    return requester.postAuthorized(`/groups/${groupName}/closed`);
  }

  static isGroupClosed(groupName) {
    return requester.getAuthorized(`/groups/${groupName}/closed`)
      .then(response => Promise.resolve(response.data));
  }

  static removeUser(groupName, user) {
    return requester.deleteAuthorized(`/groups/${groupName}/users`, user);
  }
}

export default GroupService;