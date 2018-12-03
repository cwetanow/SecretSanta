import React, { Component } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { getUserInvites, answerInvite } from '../../actions/inviteActions';

import { Container, Button, Row, Col, Card, CardBody, InputGroup, Input } from 'reactstrap'
import Invite from './Invite'

class UserInvites extends Component {
  constructor(props, context) {
    super(props, context);

    this.answerInvite = this.answerInvite.bind(this);
  }

  componentDidMount() {
    this.props.getUserInvites();
  }

  answerInvite(accept, groupName) {
    this.props.answerInvite(groupName, accept);
  }

  render() {
    return (
      <Container>
        Pending invites

        {this.props.invites.map((invite, index) =>
          <Invite invite={invite} key={index} answerInvite={this.answerInvite} />
        )}
      </Container>
    );
  }
}

const mapStateToProps = (state, ownProps) => {
  return { invites: state.invite.invites || [] };
}

const mapDispatchToProps = (dispatch) => {
  return bindActionCreators({ getUserInvites, answerInvite }, dispatch)
};

export default connect(mapStateToProps, mapDispatchToProps)(UserInvites);