import requester from '../utils/requester';

class InviteService {
  static sendInvite(groupName, username) {
    const url = `/invites/${groupName}`;

    return requester.postAuthorized(url, { username });
  }

  static getUserInvites() {
    return requester.getAuthorized('/invites')
      .then(response => Promise.resolve(response.data.invites));
  }
}

export default InviteService;