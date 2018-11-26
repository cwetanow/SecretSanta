import React from 'react';
import ReactDOM from 'react-dom';
import registerServiceWorker from './registerServiceWorker';
import configureStore from './store/configureStore';
import { Provider } from 'react-redux';
import { BrowserRouter as Router, Route } from 'react-router-dom'
import App from './components/App';

import 'bootstrap/dist/css/bootstrap.min.css'

const store = configureStore();

ReactDOM.render(
  <Provider store={store}>
    <Router >
      <Route path="/" component={App} />
    </Router >
  </Provider >,
  document.getElementById('root')
);

registerServiceWorker();
