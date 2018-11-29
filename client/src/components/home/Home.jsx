import React, { Component } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import GroupsList from '../group/GroupsList';
import CreateGroupModal from '../group/CreateGroupModal';
import { Redirect } from 'react-router-dom';
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

  componentWillReceiveProps(nextProps) {
    if (nextProps.createdGroup) {
      this.toggleModal();
    }
  }

  componentDidMount() {
    this.props.getJoinedGroups();
  }

  createGroup(group) {
    this.props.createGroup(group);
  }

  toggleModal() {
    this.setState({ isCreateGroupModalOpen: !this.state.isCreateGroupModalOpen });
  }

  render() {
    if (this.props.createdGroup) {
      return <Redirect to={`/groups/${this.props.createdGroup.name}`} />
    }

    return (<div>
      <Button type="submit" size="xl" color="primary" onClick={this.toggleModal} > Create group</Button>
      {this.state.isCreateGroupModalOpen && <CreateGroupModal closeModal={this.toggleModal} createGroup={this.createGroup} />}
      <GroupsList groups={this.props.joinedGroups} />
    </div>);
  }
}

const mapStateToProps = (state, ownProps) => {
  return {
    joinedGroups: state.group.groups || [],
    createdGroup: state.group.group
  };
}

const mapDispatchToProps = (dispatch) => {
  return bindActionCreators({ getJoinedGroups, createGroup }, dispatch)
};

export default connect(mapStateToProps, mapDispatchToProps)(Home);