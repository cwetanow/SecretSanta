import React, { Component } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import GroupsList from '../group/GroupsList';
import CreateGroupModal from '../group/CreateGroupModal';
import { Redirect } from 'react-router-dom';
import { getGroupUsers, checkGroupOwner, removeUser } from '../../actions/groupActions.js';
import { distributeGifts, getGift } from '../../actions/giftActions';
import { Button, Container, Row, Col, Card, CardBody, CardTitle, CardSubtitle } from 'reactstrap';
import { Link } from 'react-router-dom';

import Gift from '../gift/Gift'
import InviteUsers from '../invite/InviteUsers';

class GroupPage extends Component {
  constructor(props, context) {
    super(props, context);

    this.distributeGifts = this.distributeGifts.bind(this);
    this.renderGroupUsers = this.renderGroupUsers.bind(this);
  }

  componentDidMount() {
    this.props.getGroupUsers(this.props.groupName);
    this.props.checkGroupOwner(this.props.groupName);
    this.props.getGift(this.props.groupName);
  }

  distributeGifts() {
    this.props.distributeGifts(this.props.groupName);
  }

  renderGroupUsers() {
    return (<div>
      Users
      {this.props.groupUsers.map(user =>
        <Row key={user.username} className="justify-content-center">
          <Col md="6">
            <Card className="mx-4">
              <CardBody className="p-4">
                <CardTitle>
                  <Link to={'/users/' + user.username}>{user.displayName} ({user.username})</Link>
                </CardTitle>
                <CardSubtitle>
                  {this.props.isUserOwner && <Button type="submit" size="xs" color="danger" onClick={() => this.props.removeUser(this.props.groupName, user)} >Remove user</Button>}
                </CardSubtitle>
              </CardBody>
            </Card>
          </Col>
        </Row>)}
    </div>)
  }

  render() {
    return (
      <Container>
        {!this.props.hasGift && this.props.isUserOwner && <InviteUsers groupName={this.props.groupName} />}
        <hr />
        {!this.props.hasGift && this.props.isUserOwner && <Button type="submit" size="xl" color="primary" onClick={this.distributeGifts} >Distribute gifts</Button>}
        {this.props.hasGift && <Gift gift={this.props.gift} />}

        {!this.props.hasGift && this.renderGroupUsers()}
      </Container>
    );
  }
}

const mapStateToProps = (state, ownProps) => {
  return {
    groupName: ownProps.match.params.groupName,
    isUserOwner: state.group.isUserOwner,
    hasGift: state.gift.groupGift && state.gift.groupGift.hasGift,
    gift: state.gift.groupGift && state.gift.groupGift.gift,
    groupUsers: state.group.users || []
  };
}

const mapDispatchToProps = (dispatch) => {
  return bindActionCreators({ getGroupUsers, checkGroupOwner, distributeGifts, getGift, removeUser }, dispatch)
};

export default connect(mapStateToProps, mapDispatchToProps)(GroupPage);