import requester from '../utils/requester';

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

  static getUsersNotInGroup(groupName, pattern = null) {
    let url = `/select/inviteUsers/${groupName}`;

    if (pattern) {
      url = `${url}?searchPattern=${pattern}`;
    }

    return requester.getAuthorized(url)
      .then(response => Promise.resolve(response.data.users));
  }
}

export default UserService;