import requester from '../utils/requester';

class InviteService {
  static sendInvite(groupName, username) {
    const url = `/invites/${groupName}`;

    return requester.postAuthorized(url, { username });
  }
}

export default InviteService;