import requester from '../utils/requester';

const API_URL = '/api';

class GiftService {
  static distributeGifts(groupName) {
    return requester.postAuthorized(`/gifts/${groupName}`)
      .then(response => Promise.resolve(response.data.gifts));
  }

  static getGift(groupName) {
    return requester.getAuthorized(`/gifts/${groupName}`)
      .then(response => Promise.resolve(response.data));
  }
}

export default GiftService;