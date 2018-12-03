import React, { Component } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import GroupsList from '../group/GroupsList';
import CreateGroupModal from '../group/CreateGroupModal';
import { Redirect } from 'react-router-dom';
import { getGroups, createGroup } from '../../actions/groupActions.js';
import { Button, Container } from 'reactstrap';

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
    this.props.getGroups();
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

    return (<Container>
      <Button type="submit" size="xl" color="primary" onClick={this.toggleModal} > Create group</Button>
      {this.state.isCreateGroupModalOpen && <CreateGroupModal closeModal={this.toggleModal} createGroup={this.createGroup} />}
      <hr />
      <h5>Your groups</h5>
      <GroupsList groups={this.props.groups} />
    </Container>);
  }
}

const mapStateToProps = (state, ownProps) => {
  return {
    groups: state.group.groups || [],
    createdGroup: state.group.group
  };
}

const mapDispatchToProps = (dispatch) => {
  return bindActionCreators({ getGroups, createGroup }, dispatch)
};

export default connect(mapStateToProps, mapDispatchToProps)(Home);