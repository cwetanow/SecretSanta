import React, { Component } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import GroupsList from '../group/GroupsList';
import CreateGroupModal from '../group/CreateGroupModal';

import { getJoinedGroups, createGroup } from '../../actions/groupActions.js';

import { Button } from 'reactstrap';

class Home extends Component {
  constructor(props, context) {
    super(props, context);

    this.state = {
      isCreateGroupModalOpen: false
    }

    this.toggleModal = this.toggleModal.bind(this);
    this.createGroup = this.createGroup.bind(this);
  }

  componentDidMount() {
    this.props.getJoinedGroups();
  }

  createGroup(group) {
    this.props.createGroup(group)
      .then(() => {
        console.log(group)
      })
  }

  toggleModal() {
    this.setState({ isCreateGroupModalOpen: !this.state.isCreateGroupModalOpen });
  }

  render() {
    return (<div>
      <Button type="submit" size="xl" color="primary" onClick={this.toggleModal} > Create group</Button>
      {this.state.isCreateGroupModalOpen && <CreateGroupModal closeModal={this.toggleModal} createGroup={this.createGroup} />}
      <GroupsList groups={this.props.joinedGroups} />
    </div>);
  }
}

const mapStateToProps = (state, ownProps) => {
  return {
    joinedGroups: state.group.groups || []
  };
}

const mapDispatchToProps = (dispatch) => {
  return bindActionCreators({ getJoinedGroups, createGroup }, dispatch)
};

export default connect(mapStateToProps, mapDispatchToProps)(Home);