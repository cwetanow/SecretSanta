import React, { Component } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import GroupsList from '../group/GroupsList';
import CreateGroupModal from '../group/CreateGroupModal';
import { Redirect } from 'react-router-dom';
import { getGroupUsers, checkGroupOwner } from '../../actions/groupActions.js';
import { Button, Container } from 'reactstrap';

class GroupPage extends Component {
  constructor(props, context) {
    super(props, context);

    this.loadUsers = this.loadUsers.bind(this);
    this.handleUserChange = this.handleUserChange.bind(this);
    this.inviteSelectedUser = this.inviteSelectedUser.bind(this);
  }

  componentDidMount() {
    this.props.getGroupUsers(this.props.groupName);
    this.props.checkGroupOwner(this.props.groupName);
  }

  inviteSelectedUser() {

  }

  render() {
    return (
      <Container>
        {this.props.isUserOwner && <InviteUsers />}
      </Container>
    );
  }
}

const mapStateToProps = (state, ownProps) => {
  return {
    groupName: ownProps.match.params.groupName,
    isUserOwner: state.group.isUserOwner
  };
}

const mapDispatchToProps = (dispatch) => {
  return bindActionCreators({ getGroupUsers, checkGroupOwner }, dispatch)
};

export default connect(mapStateToProps, mapDispatchToProps)(GroupPage);