import React, { Component } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import GroupsList from '../group/GroupsList';

import { getJoinedGroups } from '../../actions/groupActions.js';

class Home extends Component {
  componentDidMount() {
    this.props.getJoinedGroups();
  }

  render() {
    return (<div>
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
  return bindActionCreators({ getJoinedGroups }, dispatch)
};

export default connect(mapStateToProps, mapDispatchToProps)(Home);