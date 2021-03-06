import React from 'react';
import ReactDOM from 'react-dom';
import registerServiceWorker from './registerServiceWorker';
import configureStore from './store/configureStore';
import { Provider } from 'react-redux';
import { BrowserRouter as Router, Route } from 'react-router-dom'
import App from './components/App';
import httpErrorHandlerInit from './utils/httpErrorHandler';

import * as authActions from './actions/authActions';

import './styles.css';
import 'bootstrap/dist/css/bootstrap.min.css'
import 'toastr/build/toastr.min.css'

const store = configureStore();

store.dispatch(authActions.getAuthenticatedUser());

const handleUnauthorized = (err) => {
  store.dispatch(authActions.logout());
}

httpErrorHandlerInit(handleUnauthorized);

ReactDOM.render(
  <Provider store={store}>
    <Router >
      <Route path="/" component={App} />
    </Router >
  </Provider >,
  document.getElementById('root')
);

registerServiceWorker();
