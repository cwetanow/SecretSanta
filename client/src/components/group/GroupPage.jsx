import React, { Component } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import GroupsList from '../group/GroupsList';
import CreateGroupModal from '../group/CreateGroupModal';
import { Redirect } from 'react-router-dom';
import { getGroupUsers, checkGroupOwner } from '../../actions/groupActions.js';
import { distributeGifts, getGift } from '../../actions/giftActions';
import { Button, Container } from 'reactstrap';

import Gift from '../gift/Gift'
import InviteUsers from '../invite/InviteUsers';

class GroupPage extends Component {
  constructor(props, context) {
    super(props, context);

    this.distributeGifts = this.distributeGifts.bind(this);
  }

  componentDidMount() {
    this.props.getGroupUsers(this.props.groupName);
    this.props.checkGroupOwner(this.props.groupName);
    this.props.getGift(this.props.groupName);
  }

  distributeGifts() {
    this.props.distributeGifts(this.props.groupName);
  }

  render() {
    return (
      <Container>
        {!this.props.hasGift && this.props.isUserOwner && <InviteUsers groupName={this.props.groupName} />}
        <hr />
        {!this.props.hasGift && this.props.isUserOwner && <Button type="submit" size="xl" color="primary" onClick={this.distributeGifts} >Distribute gifts</Button>}
        {this.props.hasGift && <Gift gift={this.props.gift} />}
      </Container>
    );
  }
}

const mapStateToProps = (state, ownProps) => {
  return {
    groupName: ownProps.match.params.groupName,
    isUserOwner: state.group.isUserOwner,
    hasGift: state.gift.groupGift && state.gift.groupGift.hasGift,
    gift: state.gift.groupGift && state.gift.groupGift.gift
  };
}

const mapDispatchToProps = (dispatch) => {
  return bindActionCreators({ getGroupUsers, checkGroupOwner, distributeGifts, getGift }, dispatch)
};

export default connect(mapStateToProps, mapDispatchToProps)(GroupPage);