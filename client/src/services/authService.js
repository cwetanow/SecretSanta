import requester from '../utils/requester';

const authKey = 'auth';

class AuthService {
  static register(user) {
    return requester.post('/users', user)
      .then(response => Promise.resolve(response.data));
  }

  static login(user) {
    return requester.post('/login', user)
      .then(response => Promise.resolve(response.data.token));
  }

  static getToken() {
    const auth = this.getAuth();

    if (auth) {
      return auth.token;
    }
  }

  static getAuth() {
    const auth = JSON.parse(localStorage.getItem(authKey));

    return auth;
  }

  static setAuth(token, user) {
    let auth = this.getAuth();

    localStorage.removeItem(authKey);

    auth = {
      token: token || auth.token,
      user: user
    }

    localStorage.setItem(authKey, JSON.stringify(auth));
  }

  static getCurrentUser() {
    const auth = this.getAuth();

    if (auth) {
      return auth.user;
    }
  }

  static logout() {
    localStorage.removeItem(authKey);

    return Promise.resolve();
  }
}

export default AuthService;