import requester from '../utils/requester';

class InviteService {
  static sendInvite(groupName, username) {
    const url = `/invites/${groupName}`;

    return requester.postAuthorized(url, { username })
      .then(response => Promise.resolve(response.data));
  }

  static getUserInvites() {
    return requester.getAuthorized('/invites')
      .then(response => Promise.resolve(response.data.invites));
  }

  static answerInvite(groupName, accept) {
    return requester.postAuthorized('/groups/members', { groupName, accepted: accept });
  }
}

export default InviteService;