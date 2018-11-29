import 'babel-polyfill';
import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import Route from 'react-router-dom/Route';
import { withRouter } from 'react-router';
import Register from './auth/Register';
import Login from './auth/Login';
import Logout from './auth/Logout';
import UsersList from './user/UsersList';
import UserProfile from './user/UserProfile';
import PrivateRoute from './auth/PrivateRoute';

import Notification from './common/Notification'

import { Container, Jumbotron } from 'reactstrap';
import Navbar from './common/Navbar';
import Home from './home/Home';
import GroupPage from './group/GroupPage';
import UserInvites from './invite/UserInvites';

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
        <Notification />
        <Navbar isAuthenticated={this.props.isAuthenticated}></Navbar>
        <Route exact path="/register" component={Register} />
        <Route exact path="/login" component={Login} />

        <PrivateRoute exact path="/logout" component={Logout} isAuthenticated={this.props.isAuthenticated} />

        <PrivateRoute exact path="/users" component={UsersList} isAuthenticated={this.props.isAuthenticated} />
        <PrivateRoute exact path="/users/:username" component={UserProfile} isAuthenticated={this.props.isAuthenticated} />

        <PrivateRoute exact path="/" component={Home} isAuthenticated={this.props.isAuthenticated} />

        <PrivateRoute exact path="/invites" component={UserInvites} isAuthenticated={this.props.isAuthenticated} />

        <PrivateRoute exact path="/groups/:groupName" component={GroupPage} isAuthenticated={this.props.isAuthenticated} />
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
