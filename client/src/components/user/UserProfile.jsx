import React, { Component } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { getUser } from '../../actions/userActions';

import { Container, Button, Row, Col, Card, CardBody, InputGroup, Input } from 'reactstrap'
import UserRow from './UserRow'

class UserProfile extends Component {
  constructor(props, context) {
    super(props, context);
  }

  componentDidMount() {
    this.props.getUser(this.props.username);
  }

  render() {
    return this.props.user ? (
      <Container>
        <Row>
          <Col xs="12" md="12">
            <strong><em><h2 className="heading-profile">{this.props.user.displayName}</h2></em></strong> (is actually {this.props.user.username})
          </Col>
        </Row>
      </Container>)
      : null;
  }
}

const mapStateToProps = (state, ownProps) => {
  return {
    username: ownProps.match.params.username,
    user: state.user.user
  }
}

const mapDispatchToProps = (dispatch) => {
  return bindActionCreators({ getUser }, dispatch)
};

export default connect(mapStateToProps, mapDispatchToProps)(UserProfile);