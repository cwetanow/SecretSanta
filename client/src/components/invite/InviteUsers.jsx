import React from 'react';
import { bindActionCreators } from 'redux';
import { sendInvite } from '../../actions/inviteActions.js';
import { connect } from 'react-redux';
import { Button } from 'reactstrap';
import userService from '../../services/userService.js';
import AsyncSelect from 'react-select/lib/Async';

export class InviteUsers extends React.Component {
  constructor(props, context) {
    super(props, context);

    this.state = {
      selectedUsername: '',
      value: {}
    };

    this.loadUsers = this.loadUsers.bind(this);
    this.handleUserChange = this.handleUserChange.bind(this);
    this.inviteSelectedUser = this.inviteSelectedUser.bind(this);
    this.clearSelectedItem = this.clearSelectedItem.bind(this);
  }

  loadUsers(pattern, callback) {
    userService.getUsersNotInGroup(this.props.groupName, pattern)
      .then(users => {
        const mapped = users
          .map(user => { return { value: user.username, label: `${user.displayName} (${user.username})` } });

        callback(mapped);
      });
  }

  handleUserChange(newValue) {
    this.setState({ value: newValue });
  }

  inviteSelectedUser() {
    this.props.sendInvite(this.props.groupName, this.state.value.value);

    this.clearSelectedItem();
  }

  clearSelectedItem() {
    this.setState({ value: {} })
  }

  render() {
    return (<div>
      <AsyncSelect
        cacheOptions
        loadOptions={this.loadUsers}
        defaultOptions
        value={this.state.value}
        onChange={this.handleUserChange} />

      <Button type="submit" size="xl" color="primary" onClick={this.inviteSelectedUser} >Invite</Button></div>);
  }
}

const mapDispatchToProps = (dispatch) => {
  return bindActionCreators({ sendInvite }, dispatch)
};

export default connect(null, mapDispatchToProps)(InviteUsers);