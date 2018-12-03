import requester from '../utils/requester';
const requestUtil = requester.createNew();
requestUtil.apiUrl = '/message';

class MessageService {
  static getMessages(groupName) {
    const url = `/message/${groupName}`;


    return requestUtil.get(url);
  }
}

export default MessageService;