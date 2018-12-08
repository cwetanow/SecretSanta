import requester from '../utils/requester';

class MessageService {
  static getMessages(room) {
    const url = `/message/${room}`;

    return requester.get(url)
      .then(response => Promise.resolve(response.data));
  }
}

export default MessageService;