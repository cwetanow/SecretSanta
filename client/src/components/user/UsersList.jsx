import React, { Component } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { getUsers } from '../../actions/userActions';

import { Container } from 'reactstrap'
import UserRow from './UserRow'

class UsersList extends Component {
  constructor(props, context) {
    super(props, context);
  }

  componentDidMount() {
    this.props.getUsers();
  }

  render() {
    return (<Container>
      {this.props.users.map((user, index) =>
        <UserRow user={user} key={index} />
      )}
    </Container>);
  }
}

const mapStateToProps = (state, ownProps) => {
  return { users: state.user.users || [] };
}

const mapDispatchToProps = (dispatch) => {
  return bindActionCreators({ getUsers }, dispatch)
};

export default connect(mapStateToProps, mapDispatchToProps)(UsersList);