import 'babel-polyfill';
import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import Route from 'react-router-dom/Route';
import { withRouter } from 'react-router';
import Register from './auth/Register';
import Login from './auth/Login';

import { Container, Jumbotron } from 'reactstrap';
import Navbar from './common/Navbar';

class App extends Component {
  constructor(props) {
    super(props);
  }

  componentWillReceiveProps(nextProps) {
    this.setState({ isAuthenticated: nextProps.isAuthenticated });
  }

  render() {
    return (
      <Container fluid>
        <Navbar isAuthenticated={this.props.isAuthenticated}></Navbar>
        {!this.props.isAuthenticated && <Route exact path="/register" component={Register} />}
        <Route exact path="/login" component={Login} />

      </Container>
    );
  }
}

App.propTypes = {
  children: PropTypes.object,
}

function mapStateToProps(state, ownProps) {
  return { isAuthenticated: state.auth.isAuthenticated };
}

export default withRouter(connect(mapStateToProps)(App));
